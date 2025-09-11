namespace POS_display.Models.HomeMode
{
    public class HomeModeOrderDetail
    {
        public decimal PosHeaderId { get; set; }
        public decimal PosDetailId { get; set; }
        public decimal HomeQty { get; set; }
        public decimal RealQty { get; set; }
        public decimal HomeQtyByRatio { get; set; }
        public decimal RealQtyByRatio { get; set; }
    }
}
