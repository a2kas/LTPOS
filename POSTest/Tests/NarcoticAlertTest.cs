using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using POS_display.Presenters.NarcoticAlert;
using POS_display.Repository.NarcoticAlert;
using POS_display.Views.NarcoticAlert;
using Moq;
using System.Threading.Tasks;

namespace POSTest.Tests
{
    [TestClass]
    public class NarcoticAlertTest
    {
        private NarcoticAlertPresenter _narcoticAlertPresenter;
        private Mock<INarcoticAlertView> _narcoticAlertViewMock;
        private Mock<INarcoticAlertRepository> _narcoticAlertRepositoryMock;

        public NarcoticAlertTest()
        {
            _narcoticAlertViewMock = new Mock<INarcoticAlertView>();
            _narcoticAlertRepositoryMock = new Mock<INarcoticAlertRepository>();

            _narcoticAlertPresenter = new NarcoticAlertPresenter(_narcoticAlertViewMock.Object, _narcoticAlertRepositoryMock.Object);

            _narcoticAlertViewMock.Setup(e => e.Header).Returns(new System.Windows.Forms.Label());
            _narcoticAlertViewMock.Setup(e => e.Notification).Returns(new POS_display.Helpers.RichTextBoxEx());
            _narcoticAlertViewMock.Setup(e => e.DrugMaterials).Returns(new System.Windows.Forms.DataGridView());
        }

        [TestMethod]
        public async Task InitTest()
        {
            await _narcoticAlertPresenter.Init(It.IsAny<POS_display.Enumerator.DrugType>(), It.IsAny<string>());
            _narcoticAlertRepositoryMock.Verify(e => e.GetATCCodifiersByATC(It.IsAny<string>()), Times.Once);
        }
    }
}
