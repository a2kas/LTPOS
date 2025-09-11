using POS_display.Utils.Insurance.Gjensidige;
using System;
using System.Linq;

namespace POS_display.Items
{
    public class Insurance
    {
        public Insurance(string company, string cardNoLong)
        {
            this.Company = company;
            this.CompanyString = Session.Params.FirstOrDefault(x => x.system == "INSURANCE" && x.par == this.Company)?.value;
            this.CardNoLong = cardNoLong;
            this.PartnerId = GetSettings(this.Company, Session.SystemData.ecode, "ID");
            switch (this.Company)
            {
                case "IF":
                    this.Utils = new Utils.Insurance.IF(this);
                    break;
                case "ERG":
                    this.Utils = new Utils.Insurance.HDIS(this);
                    break;
                case "GJN":
                    if(Session.Params.FirstOrDefault(x => x.system == "GJN" && x.par == "INTEGRATION_SWITCH")?.value == "1")
                    {
                        this.Utils = new Gjensidige(
                        Session.Params.FirstOrDefault(x => x.system == "GJN" && x.par == "URL" + (Session.Develop == true ? "_TEST" : ""))?.value,
                        GetSettings(this.Company, Session.SystemData.ecode, "USER"),
                        GetSettings(this.Company, Session.SystemData.ecode, "PASS"),
                        GetSettings(this.Company, Session.SystemData.ecode, "APP_USERNAME"));
                    }
                    else
                        this.Utils = new Utils.Insurance.Offline(this);
                    break;
                case "SAM":
                    this.Utils = new Utils.Insurance.SAM(this);
                    break;
                default:
                    this.Utils = new Utils.Insurance.EPS(this);
                    break;
            }
        }

        public Utils.Insurance.InsuranceBase Utils { get; set; }
        public string Company { get; set; }
        public string CompanyString { get; set; }
        public string PartnerId { get; set; }
        public string CardNoLong { get; set; }
        public string CardSessionId { get; set; }
        public object Receipt { get; set; }
        public bool ConfirmedTransaction { get; set; }

        public string GetSettings(string insurance_code, string code, string value)
        {
            string res = "";
            try
            {
                var sessionResult = Session.Params.FirstOrDefault(p => p.system == $"{insurance_code}_{value}" && p.par == (Session.Develop == true ? "TEST" : code));
                res = sessionResult?.value != null ? sessionResult.value : "";
                //res = _settings[insurance_code + (Session.Develop == true ? "_TEST" : code != "" ? "_" + code : "") + "_" + value]?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                res = "";
            }
            return res;
        }
    }
}
