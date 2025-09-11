using Newtonsoft.Json;
using System.Data.Linq.Mapping;

namespace POS_display.Models.Partner
{
    [Table(Name = "search_debtor_v2")]
    public class Partner
    {
        [Column(Name = "id")]
        public long Id { get; set; }
        [Column(Name = "name")]
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [Column(Name = "type")]
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }
        [Column(Name = "ecode")]
        [JsonProperty(PropertyName = "ECode")]
        public string ECode { get; set; }
        [Column(Name = "tcode")]
        [JsonProperty(PropertyName = "TCode")]
        public string TCode { get; set; }
        [Column(Name = "address")]
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }
        [Column(Name = "postindex")]
        [JsonProperty(PropertyName = "PostIndex")]
        public string PostIndex { get; set; }
        [Column(Name = "email")]
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [Column(Name = "agent")]
        [JsonProperty(PropertyName = "Agent")]
        public string Agent { get; set; }
        [Column(Name = "phone")]
        [JsonProperty(PropertyName = "Phone")]
        public string Phone { get; set; }
        [Column(Name = "fax")]
        [JsonProperty(PropertyName = "Fax")]
        public string Fax { get; set; }
        [Column(Name = "credtypeid")]
        [JsonProperty(PropertyName = "CreditorTypeId")]
        public string CredTypeId { get; set; }
        [Column(Name = "debtypeid")]
        [JsonProperty(PropertyName = "DebtorTypeId")]
        public string DebTypeId { get; set; }
        [Column(Name = "balance")]
        [JsonProperty(PropertyName = "Balance")]
        public decimal Balance { get; set; }
        [Column(Name = "credit")]
        [JsonProperty(PropertyName = "Credit")]
        public decimal Credit { get; set; }
        [Column(Name = "descrip")]
        [JsonProperty(PropertyName = "Description")]
        public string Descrip { get; set; }
        [Column(Name = "city")]
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
        [Column(Name = "old_ecode_scala")]
        [JsonIgnore]
        public string Scala { get; set; }
        [Column(Name = "debtortypename")]
        [JsonIgnore]
        public string DebtorTypeName { get; set; }
    }
}
