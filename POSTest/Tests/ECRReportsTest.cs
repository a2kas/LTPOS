using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POS_display.Presenters.ECRReports;
using POS_display.Repository.Pos;
using POS_display.Views.ECRReports;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_display.Repository.ECRReports;
using AutoMapper;

namespace POSTest.Tests
{
    [TestClass]
    public class ECRReportsTest
    {
        private ECRReportsPresenter _ecrReportsPresenter;
        private Mock<IECRReportsPresenter> _ecrReportsPresenterMock;
        private Mock<IECRReportsView> _ecrReportsViewMock;
        private Mock<IPosRepository> _posRepositoryMock;
        private Mock<IECRReportsRepository> _ecrReportsRepositoryMock;
        private Mock<IMapper> _mapperMock;

        public ECRReportsTest()
        {
            _posRepositoryMock = new Mock<IPosRepository>();
            _ecrReportsRepositoryMock = new Mock<IECRReportsRepository>();
            _ecrReportsViewMock = new Mock<IECRReportsView>();
            _ecrReportsPresenterMock = new Mock<IECRReportsPresenter>();
            _mapperMock = new Mock<IMapper>();
            _ecrReportsPresenter = new ECRReportsPresenter(
                _ecrReportsViewMock.Object,
                _posRepositoryMock.Object,
                _ecrReportsRepositoryMock.Object,
                _mapperMock.Object);

            _ecrReportsViewMock.Setup(e => e.DateFrom).Returns(new DateTimePicker());
            _ecrReportsViewMock.Setup(e => e.DateTo).Returns(new DateTimePicker());
            _ecrReportsViewMock.Setup(e => e.SetDate).Returns(new DateTimePicker());
            _ecrReportsViewMock.Setup(e => e.SetTime).Returns(new DateTimePicker());
            _ecrReportsViewMock.Setup(e => e.Change).Returns(new TextBox());
            _ecrReportsViewMock.Setup(e => e.Calc).Returns(new Button());
            _ecrReportsViewMock.Setup(e => e.Report).Returns(new ComboBox());
        }

        [TestMethod]
        public async Task InitOperationsData_Test()
        {
            await _ecrReportsPresenter.InitOperationsData();
            _ecrReportsRepositoryMock.Verify(e => e.Get(), Times.Once);
        }

        [TestMethod]
        [DataRow(4)]
        public async Task PerformExecute_ReportZ_Test(int index)
        {
            await _ecrReportsPresenter.PerformExecute(index.ToString());
            _posRepositoryMock.Verify(e => e.GetECRReports(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
        }

        [TestMethod]
        [DataRow(0)]
        public async Task PerformExecute_AnyOtherThanReportZ_Test(int index)
        {
            await _ecrReportsPresenter.PerformExecute(index.ToString());
            _posRepositoryMock.Verify(e => e.GetECRReports(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Once);
        }

        [TestMethod]
        [DataRow(1)]
        public void EnableControls_1_Index_Test(int index)
        {
            _ecrReportsViewMock.Setup(e => e.Report.Text).Returns(index.ToString());
            _ecrReportsPresenter.EnableControls(index.ToString());
            _ecrReportsViewMock.Object.DateFrom.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.DateTo.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.SetDate.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.SetTime.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.Change.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.Calc.Enabled.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(3)]
        public void EnableControls_3_Index_Test(int index)
        {
            _ecrReportsViewMock.Setup(e => e.Report.Text).Returns(index.ToString());
            _ecrReportsPresenter.EnableControls(index.ToString());
            _ecrReportsViewMock.Object.DateFrom.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.DateTo.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.SetDate.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.SetTime.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.Change.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.Calc.Enabled.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(13)]
        [DataRow(14)]
        public void EnableControls_5_Or_6_Or_13_Or_14_Index_Test(int index)
        {
            _ecrReportsViewMock.Setup(e => e.Report.Text).Returns(index.ToString());
            _ecrReportsPresenter.EnableControls(index.ToString());
            _ecrReportsViewMock.Object.DateFrom.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.DateTo.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.SetDate.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.SetTime.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.Change.Enabled.Should().BeTrue();
            _ecrReportsViewMock.Object.Calc.Enabled.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(7)]
        [DataRow(8)]
        public void EnableControls_7_Or_8_Index_Test(int index)
        {
            _ecrReportsPresenter.EnableControls(index.ToString());
            _ecrReportsViewMock.Object.DateFrom.Enabled.Should().BeTrue();
            _ecrReportsViewMock.Object.DateTo.Enabled.Should().BeTrue();
            _ecrReportsViewMock.Object.SetDate.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.SetTime.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.Change.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.Calc.Enabled.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(11)]
        public void EnableControls_11_Index_Test(int index)
        {
            _ecrReportsPresenter.EnableControls(index.ToString());
            _ecrReportsViewMock.Object.DateFrom.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.DateTo.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.SetDate.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.SetTime.Enabled.Should().BeTrue();
            _ecrReportsViewMock.Object.Change.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.Calc.Enabled.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(12)]
        public void EnableControls_12_Index_Test(int index)
        {
            _ecrReportsPresenter.EnableControls(index.ToString());
            _ecrReportsViewMock.Object.DateFrom.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.DateTo.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.SetDate.Enabled.Should().BeTrue();
            _ecrReportsViewMock.Object.SetTime.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.Change.Enabled.Should().BeFalse();
            _ecrReportsViewMock.Object.Calc.Enabled.Should().BeTrue();
        }

        [TestMethod]
        public void ChangeTextChanged_OverZero_Test()
        {
            TextBox tb = new TextBox();
            tb.Text = "1";
            _ecrReportsPresenter.ChangeTextChanged(tb);
            _ecrReportsViewMock.Object.Calc.Enabled.Should().BeTrue();
        }

        [TestMethod]
        public void ChangeTextChanged_EqualZero_Test()
        {
            TextBox tb = new TextBox();
            tb.Text = "0";
            _ecrReportsPresenter.ChangeTextChanged(tb);
            _ecrReportsViewMock.Object.Calc.Enabled.Should().BeFalse();
        }
    }
}
