using POS_display.Models.Recipe;
using System;
using TamroUtilities.HL7.Models;

namespace POS_display.Profiles
{
    public class ERecipeProfile : AutoMapper.Profile
    {
        public ERecipeProfile()
        {
            CreateMap<DispenseDto, MainDispenseData>()
            .ForMember(d => d.CompositionId, o => o.MapFrom(s => s.CompositionId))
            .ForMember(d => d.ProprietaryName, o => o.MapFrom(s => s.ProprietaryName))
            .ForMember(d => d.QuantityValue, o => o.MapFrom(s => s.QuantityValue))
            .ForMember(d => d.DueDate, o => o.MapFrom(s => s.DueDate))
            .ForMember(d => d.DateDueDate, o => o.MapFrom(s => ParseDate(s.DueDate)));
        }

        private DateTime ParseDate(string dueDate)
        {
            DateTime.TryParse(dueDate, out var parsedDate);
            return parsedDate;
        }
    }
}
