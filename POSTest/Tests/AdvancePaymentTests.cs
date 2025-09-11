using CKas.Contracts.PresentCards;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POS_display.Exceptions;
using POS_display.Items;
using POS_display.Presenters.AdvancePayment;
using POS_display.Repository.Pos;
using POS_display.Views.AdvancePayment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;

namespace POSTest.Tests
{
    [TestClass]
    public class AdvancePaymentTests
    {
        private Mock<IAdvancePaymentView> _mockView;
        private Mock<IPosRepository> _mockRepository;
        private Mock<ITamroClient> _mockTamroClient;
        private AdvancePaymentPresenter _presenter;
        private const string CardNumber = "123456";

        public AdvancePaymentTests()
        {
            _mockView = new Mock<IAdvancePaymentView>();
            _mockRepository = new Mock<IPosRepository>();
            _mockTamroClient = new Mock<ITamroClient>();

            _presenter = new AdvancePaymentPresenter(
                _mockView.Object,
                _mockRepository.Object,
                _mockTamroClient.Object
            );
            var comboBox = new ComboBox();
            var comboBoxItems = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("PRESENTCARD", "Dovanų kuponas"),
            };

            foreach (var item in comboBoxItems)
            {
                comboBox.Items.Add(item);
            }

            _mockView.Setup(e => e.AdvancePaymentType).Returns(comboBox);
            _mockView.Setup(e => e.OrderNumber).Returns(new TextBox());
            _mockView.Setup(e => e.AdvanceSum).Returns(new TextBox());
            _mockView.Setup(e => e.Close).Returns(new Button());
            _mockView.Setup(e => e.PosHeader).Returns(new posh());
        }

        [TestMethod]
        public async Task Init_Should_Setup_AdvancePaymentType()
        {
            // Arrange

            // Act
            await _presenter.Init();

            // Assert
            _mockView.Object.AdvancePaymentType.DataSource.Should().NotBeNull();
            _mockView.Object.AdvancePaymentType.DisplayMember.Should().NotBeNull();
            _mockView.Object.AdvancePaymentType.ValueMember.Should().NotBeNull();
            _mockView.Object.AdvancePaymentType.SelectedIndex.Should().Be(0);
        }

        [TestMethod]
        public void Confirm_Should_Throw_PresentCardException_When_PresentCard_Already_Exists_In_Basket()
        {
            // Arrange
            _mockView.Setup(view => view.OrderNumber.Text).Returns(CardNumber);
            _mockView.Setup(view => view.PosHeader).Returns(new posh { PosdItems = new List<posd> { new posd { Type = "ADVANCEPAYMENT", barcodename = CardNumber, PresentCardId = 1 } } });

            // Act + Assert
            Func<Task> act = async () => await _presenter.Confirm();
            act.Should().Throw<PresentCardException>().WithMessage("Šis dovanų kuponas jau yra pirkinių krepšelyje");
        }

        [TestMethod]
        public void Confirm_Should_Throw_PresentCardException_When_PresentCard_Not_Found()
        {
            // Arrange
            _mockView.Setup(view => view.OrderNumber.Text).Returns(CardNumber);
            _mockTamroClient.Setup(client => client.GetAsync<List<PresentCardViewModel>>(It.IsAny<string>(), null)).ReturnsAsync((List<PresentCardViewModel>)null);

            // Act + Assert
            Func<Task> act = async () => await _presenter.Confirm();
            act.Should().Throw<PresentCardException>().WithMessage("Tokio kodo nėra!");
        }

        [TestMethod]
        public void Confirm_Should_Throw_PresentCardException_When_PresentCard_Status_Is_Issued()
        {
            // Arrange
            var presentCard = new PresentCardViewModel { Status = PresentCardStatus.Issued };
            _mockView.Setup(view => view.OrderNumber.Text).Returns(CardNumber);
            _mockTamroClient.Setup(client => client.GetAsync<List<PresentCardViewModel>>(It.IsAny<string>(), null)).ReturnsAsync(new List<PresentCardViewModel> { presentCard });

            // Act + Assert
            Func<Task> act = async () => await _presenter.Confirm();
            act.Should().Throw<PresentCardException>().WithMessage("Dovanų kortelė jau yra išduota ir negali būti parduota");
        }

        [TestMethod]
        public void Confirm_Should_Throw_PresentCardException_When_PresentCard_Status_Is_Expired()
        {
            // Arrange
            var presentCard = new PresentCardViewModel { Status = PresentCardStatus.Expired };
            _mockView.Setup(view => view.OrderNumber.Text).Returns(CardNumber);
            _mockTamroClient.Setup(client => client.GetAsync<List<PresentCardViewModel>>(It.IsAny<string>(), null)).ReturnsAsync(new List<PresentCardViewModel> { presentCard });

            // Act + Assert
            Func<Task> act = async () => await _presenter.Confirm();
            act.Should().Throw<PresentCardException>().WithMessage("Dovanų kortelė nebegalioja!");
        }

        [TestMethod]
        public void Confirm_Should_Throw_PresentCardException_When_PresentCard_Status_Is_Sold()
        {
            // Arrange
            var presentCard = new PresentCardViewModel { Status = PresentCardStatus.Sold };
            _mockView.Setup(view => view.OrderNumber.Text).Returns(CardNumber);
            _mockTamroClient.Setup(client => client.GetAsync<List<PresentCardViewModel>>(It.IsAny<string>(),null)).ReturnsAsync(new List<PresentCardViewModel> { presentCard });

            // Act + Assert
            Func<Task> act = async () => await _presenter.Confirm();
            act.Should().Throw<PresentCardException>().WithMessage("Dovanų kortelė jau yra panaudota!");
        }

        [TestMethod]
        public async Task Confirm_Should_CreateAdvancePayment_With_Valid_Parameters()
        {
            // Arrange
            var presentCard = new PresentCardViewModel { Status = PresentCardStatus.New, Amount = 50 };
            _presenter.CardCache = new List<string>(); 
            _mockView.Setup(view => view.PosHeader).Returns(new posh { Id = 123 });
            _mockView.Setup(view => view.SelectedAdvancePaymentType).Returns("PRESENTCARD");
            _mockView.Setup(view => view.OrderNumber.Text).Returns(CardNumber);
            _mockTamroClient.Setup(client => client.GetAsync<List<PresentCardViewModel>>(It.IsAny<string>(), null)).ReturnsAsync(new List<PresentCardViewModel> { presentCard });
            _mockRepository.Setup(repo => repo.CreateAdvancePayment(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).ReturnsAsync(456);

            // Act
            await _presenter.Confirm();

            // Assert
            _mockRepository.Verify(repo => repo.CreateAdvancePayment(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
        }
    }
}
