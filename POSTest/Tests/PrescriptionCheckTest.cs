using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POS_display.Presenters.PrescriptionCheck;
using POS_display.Repository.Pos;
using POS_display.Repository.Recipe;
using POS_display.Views.PrescriptionCheck;
using System.Threading.Tasks;

namespace POSTest.Tests
{
	[TestClass]
    public class PrescriptionCheckTest
    {
        private PrescriptionCheckPresenter _prescriptionCheckPresenter;
        private Mock<IPrescriptionCheckView> _prescriptionCheckViewMock;
        private Mock<IPosRepository> _posRepositoryMock;
		private Mock<IRecipeRepository> _recipeRepositoryMock;
		private Mock<IPrescriptionCheckPresenter> _prescriptionCheckPresenterMock;

        public PrescriptionCheckTest()
        {
            _posRepositoryMock = new Mock<IPosRepository>();
			_recipeRepositoryMock = new Mock<IRecipeRepository>();
			_prescriptionCheckViewMock = new Mock<IPrescriptionCheckView>();
            _prescriptionCheckPresenterMock = new Mock<IPrescriptionCheckPresenter>();         

            _prescriptionCheckPresenter = new PrescriptionCheckPresenter(_prescriptionCheckViewMock.Object, _recipeRepositoryMock.Object);
            _prescriptionCheckPresenter.PosRepository = _posRepositoryMock.Object;
        }

        [TestMethod]
        public async Task UpdateSessionTest()
        {
            await _prescriptionCheckPresenter.UpdateSession(It.IsAny<string>(), It.IsAny<decimal>());
            _posRepositoryMock.Verify(e => e.UpdateSession(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);

        }

		[TestMethod]
		public void Init_Initiates_Called()
		{
			_prescriptionCheckPresenterMock.Setup(e => e.Init()).Verifiable();
		}

		[TestMethod]
        public void Save_Saves_Called()
        {
			_prescriptionCheckPresenterMock.Setup(e => e.Save()).Verifiable();
		}

    }
}
