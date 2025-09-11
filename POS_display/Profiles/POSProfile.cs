using POS_display.Models.ECRReports;
using System;
using TamroUtilities.HL7.Models;

namespace POS_display.Profiles
{
    public class POSProfile : AutoMapper.Profile
    {
        public POSProfile()
        {
            CreateMap<(EKJEntry ekj, ZReportEntry report, DeviceSettingValue settingValue), PosZLine>()
            .ForMember(d => d.DateString, o => o.MapFrom(s => s.ekj.Date.ToString("yyyy-MM-dd")))
            .ForMember(d => d.ZNr, o => o.MapFrom(s => s.ekj.ZNumber))
            .ForMember(d => d.Pos, o => o.MapFrom(s => s.settingValue.Value))
            .ForMember(d => d.Cash, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.PaidP)))
            .ForMember(d => d.Credit, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.PaidN)))
            .ForMember(d => d.Card, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.PaidC)))
            .ForMember(d => d.DiscQty, o => o.MapFrom(s => s.report.DiscountCount))
            .ForMember(d => d.DiscSum, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.DiscountTotal)))
            .ForMember(d => d.CashIn, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.FinancialInP)))
            .ForMember(d => d.CashOut, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.FinancialOutP)))
            .ForMember(d => d.Pay1, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.PaidI)))
            .ForMember(d => d.Pay2, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.PaidJ)))
            .ForMember(d => d.Pay3, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.PaidK)))
            .ForMember(d => d.AdvanceIn1, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.FinancialInA)))
            .ForMember(d => d.AdvanceIn2, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.FinancialInC)))
            .ForMember(d => d.CashRest, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.DrawerCash)))
            .ForMember(d => d.ATaxPercent, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.ATaxPercent)))
            .ForMember(d => d.ATotal, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.ATotal)))
            .ForMember(d => d.ATotalTax, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.ATotalTax)))
            .ForMember(d => d.ATotalWTax, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.ATotalWTax)))
            .ForMember(d => d.CTaxPercent, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.CTaxPercent)))
            .ForMember(d => d.CTotal, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.CTotal)))
            .ForMember(d => d.CTotalTax, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.CTotalTax)))
            .ForMember(d => d.CTotalWTax, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.CTotalWTax)))
            .ForMember(d => d.TotalIncomeWTax, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.TotalIncomeWTax)))
            .ForMember(d => d.BTaxPercent, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.BTaxPercent)))
            .ForMember(d => d.BTotal, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.BTotal)))
            .ForMember(d => d.BTotalTax, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.BTotalTax)))
            .ForMember(d => d.BTotalWTax, o => o.MapFrom(s => ConvertIntAmountToDecimal(s.report.BTotalWTax)));

            CreateMap<Items.eRecipe.Issue, IssueDto>()
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.Display, o => o.MapFrom(s => s.Details))
                .ForMember(d => d.IgnoreReason, o => o.MapFrom(s => s.IgnoreReason))
                .ForMember(d => d.IssueSeverity, o => o.MapFrom(s => ParseSeverity(s.Severity)));

            CreateMap<IssueDto, Items.eRecipe.Issue>()
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.Details, o => o.MapFrom(s => s.Display))
                .ForMember(d => d.IgnoreReason, o => o.MapFrom(s => s.IgnoreReason))
                .ForMember(d => d.Severity, o => o.MapFrom(s => s.IssueSeverity.ToString().ToLowerInvariant()));
        }

        private IssueSeverity ParseSeverity(string severity)
        {
            if (Enum.TryParse<IssueSeverity>(severity, out var result))
            {
                return result;
            }
            return IssueSeverity.Unknown;
        }

        private decimal ConvertIntAmountToDecimal(int value) 
        { 
            if (value <= 0)
                return 0;

            return Math.Round((decimal)value / 100, 2);

        }
    }
}
