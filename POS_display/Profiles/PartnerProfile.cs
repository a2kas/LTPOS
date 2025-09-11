using POS_display.Models.Partner;

namespace POS_display.Profiles
{
    public class PartnerProfile : AutoMapper.Profile
    {
        public PartnerProfile()
        {
            CreateMap<Partner, GetPartnersRequest>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.ECode, o => o.MapFrom(s => s.ECode))
            .ForMember(d => d.TCode, o => o.MapFrom(s => s.TCode))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
            .ForMember(d => d.City, o => o.MapFrom(s => s.City));

            CreateMap<Partner, PartnerViewData>();
        }
    }
}
