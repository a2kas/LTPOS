using System;
using System.Xml.Serialization;

namespace POS_display.Models.ECRReports
{
    [Serializable]
    [XmlRoot("Line")]
    public class PosZLine
    {
        [XmlIgnore]
        public DateTime Date
        {
            get  { return DateTime.Parse(DateString); }
        }
        [XmlAttribute(AttributeName = "Date")]
        public string DateString { get; set; }
        [XmlAttribute(AttributeName = "Znr")]
        public int ZNr { get; set; }
        [XmlAttribute(AttributeName = "Pos")]
        public string Pos { get; set; }
        [XmlAttribute(AttributeName = "Cash")]
        public decimal Cash { get; set; }
        [XmlAttribute(AttributeName = "Credit")]
        public decimal Credit { get; set; }
        [XmlAttribute(AttributeName = "Card")]
        public decimal Card { get; set; }
        [XmlAttribute(AttributeName = "Cheque")]
        public decimal Cheque { get; set; }
        [XmlAttribute(AttributeName = "Vata")]
        public decimal VatA { get; set; }
        [XmlAttribute(AttributeName = "Vatb")]
        public decimal VatB { get; set; }
        [XmlAttribute(AttributeName = "Vatc")]
        public decimal VatC { get; set; }
        [XmlAttribute(AttributeName = "Vatd")]
        public decimal VatD { get; set; }
        [XmlAttribute(AttributeName = "Vate")]
        public decimal VatE { get; set; }
        [XmlAttribute(AttributeName = "Vatf")]
        public decimal VatF { get; set; }
        [XmlAttribute(AttributeName = "Totala")]
        public decimal TotalA { get; set; }
        [XmlAttribute(AttributeName = "Totalb")]
        public decimal TotalB { get; set; }
        [XmlAttribute(AttributeName = "Totalc")]
        public decimal TotalC { get; set; }
        [XmlAttribute(AttributeName = "Totald")]
        public decimal TotalD { get; set; }
        [XmlAttribute(AttributeName = "Totale")]
        public decimal TotalE { get; set; }
        [XmlAttribute(AttributeName = "Totalf")]
        public decimal TotalF { get; set; }
        [XmlAttribute(AttributeName = "Packa")]
        public decimal PackA { get; set; }
        [XmlAttribute(AttributeName = "Packb")]
        public decimal PackB { get; set; }
        [XmlAttribute(AttributeName = "Packc")]
        public decimal PackC { get; set; }
        [XmlAttribute(AttributeName = "Packd")]
        public decimal PackD { get; set; }
        [XmlAttribute(AttributeName = "Packe")]
        public decimal PackE { get; set; }
        [XmlAttribute(AttributeName = "Packf")]
        public decimal PackF { get; set; }
        [XmlAttribute(AttributeName = "Gpack")]
        public decimal GPack { get; set; }
        [XmlAttribute(AttributeName = "Gcred")]
        public decimal GCred { get; set; }
        [XmlAttribute(AttributeName = "Gcash")]
        public decimal GCash { get; set; }
        [XmlAttribute(AttributeName = "Discqty")]
        public decimal DiscQty { get; set; }
        [XmlAttribute(AttributeName = "Overqty")]
        public decimal OverQty { get; set; }
        [XmlAttribute(AttributeName = "Discsum")]
        public decimal DiscSum { get; set; }
        [XmlAttribute(AttributeName = "Oversum")]
        public decimal OverSum { get; set; }
        [XmlAttribute(AttributeName = "Cashin")]
        public decimal CashIn { get; set; }
        [XmlAttribute(AttributeName = "Cashout")]
        public decimal CashOut { get; set; }
        [XmlAttribute(AttributeName = "Euro")]
        public decimal Euro { get; set; }
        [XmlAttribute(AttributeName = "Pay1")]
        public decimal Pay1 { get; set; }
        [XmlAttribute(AttributeName = "Pay2")]
        public decimal Pay2 { get; set; }
        [XmlAttribute(AttributeName = "Pay3")]
        public decimal Pay3 { get; set; }
        [XmlAttribute(AttributeName = "Pay4")]
        public decimal Pay4 { get; set; }
        [XmlAttribute(AttributeName = "Advancein1")]
        public decimal AdvanceIn1 { get; set; }
        [XmlAttribute(AttributeName = "Advanceout1")]
        public decimal AdvanceOut1 { get; set; }
        [XmlAttribute(AttributeName = "Advancein2")]
        public decimal AdvanceIn2 { get; set; }
        [XmlAttribute(AttributeName = "Advanceout2")]
        public decimal AdvanceOut2 { get; set; }
        [XmlAttribute(AttributeName = "Advancein3")]
        public decimal AdvanceIn3 { get; set; }
        [XmlAttribute(AttributeName = "Advanceout3")]
        public decimal AdvanceOut3 { get; set; }
        [XmlAttribute(AttributeName = "Cashrest")]
        public decimal CashRest { get; set; }
        [XmlAttribute(AttributeName = "Euroin")]
        public decimal EuroIn { get; set; }
        [XmlAttribute(AttributeName = "Euroout")]
        public decimal EuroOut { get; set; }
        [XmlAttribute(AttributeName = "Lastfdocnr")]
        public decimal LastfDocNr { get; set; }
        [XmlIgnore]
        public decimal ATaxPercent { get; set; }
        [XmlIgnore]
        public decimal ATotal { get; set; }
        [XmlIgnore]
        public decimal ATotalTax { get; set; }
        [XmlIgnore]
        public decimal ATotalWTax{ get; set; }
        [XmlIgnore]
        public decimal CTaxPercent { get; set; }
        [XmlIgnore]
        public decimal CTotal { get; set; }
        [XmlIgnore]
        public decimal CTotalTax { get; set; }
        [XmlIgnore]
        public decimal CTotalWTax { get; set; }
        [XmlIgnore]
        public decimal TotalIncomeWTax { get; set; }
        [XmlIgnore]
        public decimal BTaxPercent { get; set; }
        [XmlIgnore]
        public decimal BTotal { get; set; }
        [XmlIgnore]
        public decimal BTotalTax { get; set; }
        [XmlIgnore]
        public decimal BTotalWTax { get; set; }
        [XmlIgnore]
        public DateTime Created { get; set; }
    }
}
