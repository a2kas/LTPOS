using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POS_display.Presenters.KAS;
using POS_display.Views.KAS;
using System;
using System.Threading.Tasks;
using POS_display.Repository.KAS;
using POS_display.Repository.Partners;
using POS_display.Models.Partner;
using FluentAssertions;

namespace POSTest.Tests.KAS
{
    [TestClass]
    public class InvoiceTest
    {
        private InvoicePresenter _invoicePresenter;
        private Mock<IInvoiceView> _invoiceViewMock;
        private Mock<IKASRepository> _kasRepositoryMock;
        private Mock<IPartnerRepository> _partnerRepositoryMock;


        public InvoiceTest()
        {
            _kasRepositoryMock = new Mock<IKASRepository>();
            _partnerRepositoryMock = new Mock<IPartnerRepository>();
            _invoiceViewMock = new Mock<IInvoiceView>();

            _invoicePresenter = new InvoicePresenter(_invoiceViewMock.Object, _kasRepositoryMock.Object, _partnerRepositoryMock.Object);

            _invoiceViewMock.Setup(e => e.DebtorEcode).Returns(new System.Windows.Forms.TextBox());
            _invoiceViewMock.Setup(e => e.DebtorName).Returns(new System.Windows.Forms.TextBox());
            _invoiceViewMock.Setup(e => e.CheckNo).Returns(new System.Windows.Forms.TextBox());
            _invoiceViewMock.Setup(e => e.CheckDate).Returns(new System.Windows.Forms.TextBox());
            _invoiceViewMock.Setup(e => e.DocumentDate).Returns(new System.Windows.Forms.TextBox());
            _invoiceViewMock.Setup(e => e.DocumentNo).Returns(new System.Windows.Forms.TextBox());
            _invoiceViewMock.Setup(e => e.Save).Returns(new System.Windows.Forms.Button());
        }

        [TestMethod]
        public async Task Init_Test()
        {
            await _invoicePresenter.Init(new POS_display.Models.KAS.PosHeader() { CheckNo = "", DocumentDate = DateTime.Now });
            _kasRepositoryMock.Verify(e => e.GetBENUMTransaction(It.IsAny<long>(), It.IsAny<int>()), Times.Once);
            _kasRepositoryMock.Verify(e => e.GetSFNumber(It.IsAny<DateTime>()), Times.Once);
        }

        [TestMethod]
        public async Task SetPartnerDataByScala_Test()
        {
            await _invoicePresenter.SetPartnerDataByScala(new POS_display.Models.KAS.ChequePresent() { Buyer = 1 });
            _partnerRepositoryMock.Verify(e => e.GetPartnerByScala(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task CheckSFHeaderExist_Test()
        {
            await _invoicePresenter.CheckSFHeaderExist(It.IsAny<string>());
            _kasRepositoryMock.Verify(e => e.CheckSFHeader(It.IsAny<string>()), Times.Once); 
        }

        [TestMethod]
        public async Task LoadPartnerData_Test()
        {
            await _invoicePresenter.LoadPartnerData(It.IsAny<string>());
            _partnerRepositoryMock.Verify(e => e.GetPartner(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        [DataRow("ecode", "name", 1L)]
        public void SetPartnerData_Test(string ecode, string name, long id)
        {
            Partner partner = new Partner();
            partner.ECode = ecode;
            partner.Name = name;
            partner.Id = id;

            _invoicePresenter.SetPartnerData(partner);
            _invoiceViewMock.Object.DebtorEcode.Text.Should().Be(ecode);
            _invoiceViewMock.Object.DebtorName.Text.Should().Be(name);
            _invoiceViewMock.Setup(e => e.CreditorId).Returns(id);
            _invoiceViewMock.Object.CreditorId.Should().Be(id);
        }

        [TestMethod]
        [DataRow("1.0", "1")]
        public void EnableSaving_DocumentNo_NotEmpty_Test(string docNo, string ecode)
        {
            _invoiceViewMock.Object.DocumentNo.Text = docNo;
            _invoiceViewMock.Object.DebtorEcode.Text = ecode;
             _invoicePresenter.EnableSaving();
            _invoiceViewMock.Object.Save.Enabled.Should().BeTrue();
        }

        [TestMethod]
        [DataRow("0", "")]
        public void EnableSaving_DocumentNo_Ismpty_Test(string docNo, string ecode)
        {
            _invoiceViewMock.Object.DocumentNo.Text = docNo;
            _invoiceViewMock.Object.DebtorEcode.Text = ecode;
            _invoicePresenter.EnableSaving();
            _invoiceViewMock.Object.Save.Enabled.Should().BeFalse();
        }
    }
}
