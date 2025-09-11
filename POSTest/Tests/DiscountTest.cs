using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Linq;
using FluentAssertions;
using POS_display.Presenters.Discount;
using POS_display.Views.Discount;
using POS_display.Repository.Discount;
using POS_display.Repository.Pos;
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using POS_display.Models.Discount;
using System.Windows.Forms;
using POS_display.Repository.Loyalty;

namespace POSTest.Tests
{
    [TestClass]
    public class DiscountTest
    {
        private DiscountPresenter _discountPresenter;
        private Mock<IDiscountView> _discountViewMock;
        private Mock<IDiscountRepository> _discountRepositoryMock;
        private Mock<ILoyaltyRepository> _loyaltyRepositoryMock;
        private Mock<IPosRepository> _posRepositoryMock;

        public DiscountTest()
        {
            _discountRepositoryMock = new Mock<IDiscountRepository>();
            _loyaltyRepositoryMock = new Mock<ILoyaltyRepository>();
            _posRepositoryMock = new Mock<IPosRepository>();
            _discountViewMock = new Mock<IDiscountView>();
            //_discountRepositoryMock.Setup(e => e.GetDiscountTypes2(It.IsAny<decimal>(), It.IsAny<string>())).ReturnsAsync(new List<DiscountType>() { new DiscountType() });
            _discountPresenter = new DiscountPresenter(_discountViewMock.Object, _discountRepositoryMock.Object, _loyaltyRepositoryMock.Object);
            _discountPresenter.PosRepository = _posRepositoryMock.Object;

            _discountViewMock.Setup(e => e.DiscountTypes2).Returns(new List<DiscountType>());
            _discountViewMock.Setup(e => e.CardNoTextBox).Returns(new System.Windows.Forms.TextBox());
            _discountViewMock.Setup(e => e.CalcButton).Returns(new System.Windows.Forms.Button());           

        }

        [TestMethod]
        public async Task UpdateSessionTest()
        {
            await _discountPresenter.UpdateSession(It.IsAny<string>(), It.IsAny<decimal>());
            _posRepositoryMock.Verify(e => e.UpdateSession(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);

        }
        [TestMethod]
        public async Task CreateDiscountTest()
        {
            await _discountPresenter.ApplyManualDiscount(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string>());
            _discountRepositoryMock.Verify(e => e.CreateDiscount(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Once);

        }
        [TestMethod]
        public async Task LoadDiscountCategories()
        {
            await _discountPresenter.LoadDiscountCategories();
            _discountRepositoryMock.Verify(e => e.GetDiscountCategories(), Times.Once);
        }

        [TestMethod]
        [DataRow("LAI")]
        [DataRow("LIA")]
        [DataRow("SPK")]
        [DataRow("SPD")]
        [DataRow("W")]
        public async Task LoadDiscountTypes2_IfPrefixValuable(string prefix)
        {
            _discountViewMock.Setup(e => e.SelectedDiscountCategory).Returns(new DiscountH() { Id = It.IsAny<decimal>(), Perfix = prefix });
            await _discountPresenter.LoadDiscountTypes2();
            _discountRepositoryMock.Verify(e => e.GetDiscountTypes2(It.IsAny<decimal>(), It.IsAny<string>()), Times.Once);
            _discountViewMock.Object.CardNoTextBox.Enabled.Should().BeTrue();
            _discountViewMock.Object.CalcButton.Enabled.Should().BeFalse();
        }

        [TestMethod]
        [DataRow("TEST")]
        public async Task LoadDiscountTypes2_IfPrefixNotValuable(string prefix)
        {
            _discountViewMock.Setup(e => e.SelectedDiscountCategory).Returns(new DiscountH() { Id = It.IsAny<decimal>(), Perfix = prefix });
            await _discountPresenter.LoadDiscountTypes2();
            _discountRepositoryMock.Verify(e => e.GetDiscountTypes2(It.IsAny<decimal>(), It.IsAny<string>()), Times.Once);
            _discountViewMock.Object.CardNoTextBox.Enabled.Should().BeFalse();
            _discountViewMock.Object.CalcButton.Enabled.Should().BeTrue();
        }
        [TestMethod]
        [DataRow("4")]
        public async Task LoadDiscounts_IsNotHandInput(string type)
        {
            _discountRepositoryMock.Setup(e => e.GetDiscounts(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<DiscountD>() { new DiscountD() });
            await _discountRepositoryMock.Object.GetDiscounts(It.IsAny<string>(), It.IsAny<string>());
            _discountRepositoryMock.Verify(e => e.GetDiscounts(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            DiscountType discountCategory = _discountViewMock.Object.DiscountTypes2.FirstOrDefault(val => val.Type.Equals(type));
            discountCategory.Should().BeNull();
        }
        [TestMethod]
        public void CalculateDiscount_CardLenght()
        {
            _discountViewMock.Setup(e => e.CardNoTextBox.Text).Returns("");
            _discountViewMock.Object.CardNoTextBox.Text.Length.Should().Be(0);
        }

        [TestMethod]
        public async Task CalculateDiscount_CreateDiscount()
        {
            await _discountPresenter.ApplyManualDiscount(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string>());
            _discountRepositoryMock.Verify(e => e.CreateDiscount(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [DataRow(10.0)]
        public void CalculateDiscount_CanCreateDiscount(double discount)
        {
            _discountViewMock.Setup(e => e.SelectedDiscountType2).Returns(new DiscountType() { Type = "1" });
            decimal discountValue = Convert.ToDecimal(discount);
            discountValue.Should().BeInRange(0, 100);
            _discountViewMock.Object.SelectedDiscountType2.Type.Should().Be("1");
        }

        [TestMethod]
        [DataRow(-1.0)]
        public void CalculateDiscount_CanNotCreateDiscount(double discount)
        {
            _discountViewMock.Setup(e => e.SelectedDiscountType2).Returns(new DiscountType() { Type = "0" });
            decimal discountValue = Convert.ToDecimal(discount);
            discountValue.Should().NotBeInRange(0, 100);
            _discountViewMock.Object.SelectedDiscountType2.Type.Should().NotBe("1");
        }

        [TestMethod]
        public void EnableButtons_CheckTextBox()
        {
            _discountPresenter.EnableButtons(new TextBox());
            _discountViewMock.Object.CalcButton.Enabled.Should().BeTrue();
        }

        [TestMethod]
        public void EnableButtons_CheckComboBox_DiscountLessThan0()
        {
            _discountViewMock.Setup(e => e.DiscountSum.Text).Returns("0");
            _discountPresenter.EnableButtons(new ComboBox());
            _discountViewMock.Object.CalcButton.Enabled.Should().BeFalse();
        }

        [TestMethod]
        public void EnableButtons_CheckComboBox_DiscountOver0()
        {
            _discountViewMock.Setup(e => e.DiscountSum.Text).Returns("1");
            _discountPresenter.EnableButtons(new ComboBox());
            _discountViewMock.Object.CalcButton.Enabled.Should().BeTrue();

        }

    }
}
