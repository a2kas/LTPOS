using ExternalServices.CareCloudREST.Models.CortexModels.Customers;
using POS_display.Models.CRM;
using System.Globalization;
using System.Linq;
using static Cortex.Client.Model.CustomAgreements;

namespace POS_display.Profiles
{
    public class CRMProfile : AutoMapper.Profile
    {
        private const string PersonalPharmacistAgreementId = "86de128f5690f653675de13132";
        public CRMProfile()
        {
            CreateMap<Customer, CRMClientData>()
            .ForMember(d => d.ID, o => o.MapFrom(s => s.CustomerId))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.PersonalData.FirstName))
            .ForMember(d => d.Surename, o => o.MapFrom(s => s.PersonalData.LastName))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.PersonalData.Phone))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.PersonalData.Email))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.PersonalData.Address.Address1))
            .ForMember(d => d.City, o => o.MapFrom(s => s.PersonalData.Address.City))
            .ForMember(d => d.PostCode, o => o.MapFrom(s => s.PersonalData.Address.Zip))
            .ForMember(d => d.Country, o => o.MapFrom(s => s.PersonalData.Address.CountryCode))
            .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.PersonalData.BirthDate.ToDateTime()))
            .ForMember(d => d.AgreementMarketingCommunication, o => o.MapFrom(s => s.PersonalData.Agreement.AgreementMarketingCommunication.ToBool()))
            .ForMember(d => d.PersonalPharmacistClient, o => o.MapFrom(s => s.PersonalData.Agreement.CustomAgreements.Any(e => e.AgreementId == PersonalPharmacistAgreementId && e.AgreementValue == 1)));

            CreateMap<Cortex.Client.Model.Customer, CRMClientData>()
            .ForMember(d => d.ID, o => o.MapFrom(s => s.CustomerId))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.PersonalInformation.FirstName))
            .ForMember(d => d.Surename, o => o.MapFrom(s => s.PersonalInformation.LastName))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.PersonalInformation.Phone))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.PersonalInformation.Email))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.PersonalInformation.Address.Address1))
            .ForMember(d => d.City, o => o.MapFrom(s => s.PersonalInformation.Address.City))
            .ForMember(d => d.PostCode, o => o.MapFrom(s => s.PersonalInformation.Address.Zip))
            .ForMember(d => d.Country, o => o.MapFrom(s => s.PersonalInformation.Address.CountryCode))
            .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.PersonalInformation.Birthdate.ToDateTime()))
            .ForMember(d => d.AgreementMarketingCommunication, o => o.MapFrom(s => s.PersonalInformation.Agreement.AgreementMarketingCommunication))
            .ForMember(d => d.PersonalPharmacistClient, o => o.MapFrom(s => s.PersonalInformation.Agreement.CustomAgreements.Any(e => e.AgreementId == PersonalPharmacistAgreementId && e.AgreementValue == AgreementValueEnum.NUMBER_1)));

            CreateMap<Cortex.Client.Model.DiscountItem, CRMDiscountItem>()
            .ForMember(d => d.BillItemId, o => o.MapFrom(s => s.BillItemId))
            .ForMember(d => d.DiscountCode, o => o.MapFrom(s => s.DiscountCode))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
            .ForMember(d => d.DiscountValue, o => o.MapFrom(s => ParseDecimalValue(s.DiscountValue)))
            .ForMember(d => d.DiscountPercent, o => o.MapFrom(s => ParseDecimalValue(s.DiscountPercent)));

            CreateMap<Cortex.Client.Model.PaymentVoucher, CRMDiscountItem>()
            .ForMember(d => d.BillItemId, o => o.MapFrom(s => s.BillItemId))
            .ForMember(d => d.DiscountCode, o => o.MapFrom(s => s.Code))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.DiscountValue, o => o.MapFrom(s => s.DiscountValue))
            .ForMember(d => d.DiscountPercent, o => o.MapFrom(s => s.DiscountPercent));
        }

        private decimal ParseDecimalValue(string value)
        {
            if (decimal.TryParse(value,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
    }
}
