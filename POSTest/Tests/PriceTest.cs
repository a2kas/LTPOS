using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Linq;
using FluentAssertions;
using POS_display.Models.Price;
using POS_display.Presenters.Price;
using POS_display.Views.Price;
using POS_display.Repository.Price;
using POS_display.Repository.Pos;
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using POS_display.Models.Discount;
using System.Windows.Forms;


namespace POSTest.Tests
{
    [TestClass]
    public class PriceTest
    {
        private PricePresenter _pricePresenter;
        private Mock<IPriceView> _priceViewMock;
        private Mock<IPriceRepository> _priceRepositoryMock;
        private Mock<IPosRepository> _posRepositoryMock;

        public PriceTest()
        {
            _priceRepositoryMock = new Mock<IPriceRepository>();
            _posRepositoryMock = new Mock<IPosRepository>();
            _priceViewMock = new Mock<IPriceView>();

            _pricePresenter = new PricePresenter(_priceViewMock.Object, _priceRepositoryMock.Object);
            _pricePresenter.PosRepository = _posRepositoryMock.Object;

            _priceViewMock.Setup(e => e.Price).Returns(new TextBox());
            _priceViewMock.Setup(e => e.CalcButton).Returns(new Button());
        }

        [TestMethod]
        public async Task UpdateSessionTest()
        {
            await _pricePresenter.UpdateSession(It.IsAny<string>(), It.IsAny<decimal>());
            _posRepositoryMock.Verify(e => e.UpdateSession(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);

        }
        [TestMethod]
        public async Task ChangePosDPrice()
        {
            var PosDPrice = new PosDPrice
            {
                PosdId = It.IsAny<decimal>(),
                Price = It.IsAny<decimal>()
            };

            await _pricePresenter.ChangePosDPrice(PosDPrice);
            _posRepositoryMock.Verify(e => e.ChangePosDPrice(PosDPrice), Times.Once);
        }

        [TestMethod]
        public void EnableButtons_PriceIsNotEqualZero()
        {
            _priceViewMock.Setup(e => e.Price.Text).Returns("0");
            _pricePresenter.EnableButtons();
            _priceViewMock.Object.CalcButton.Enabled.Should().BeFalse();

        }
        [TestMethod]
        public void EnableButtons_PriceIsEqualZero()
        {
            _priceViewMock.Setup(e => e.Price.Text).Returns("1");
            _pricePresenter.EnableButtons();
            _priceViewMock.Object.CalcButton.Enabled.Should().BeTrue();

        }

    }
}
