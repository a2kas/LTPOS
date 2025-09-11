
namespace POS_display.Models.BarcodeData
{
    public class BarcodeData
    {
        private decimal _productId;
        private decimal _barcodeId;
        private string _gr4;

        public decimal ProductID
        {
            get => _productId;
            set => _productId = value;
        }

        public decimal BarcodeID
        {
            get => _barcodeId;
            set => _barcodeId = value;
        }

        public string Gr4
        {
            get => _gr4;
            set => _gr4 = value;
        }
    }
}
