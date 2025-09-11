using POS_display.Models.KAS;
using System;

namespace POS_display.Profiles
{
    public class KASProfile : AutoMapper.Profile
    {
        public KASProfile()
        {
            CreateMap<Items.KAS.posd, ReturningItem>()
            .ForMember(d => d.PosDetailId, o => o.MapFrom(s => s.id))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.barcodename))
            .ForMember(d => d.Qty, o => o.MapFrom(s => s.qty))
            .ForMember(d => d.CompensationSum, o => o.MapFrom(s => s.compensationsum))
            .ForMember(d => d.PrepaymentCompensation, o => o.MapFrom(s => s.prepayment_compensation))
            .ForMember(d => d.PriceWithVAT, o => o.MapFrom(s => s.priceincvat))
            .ForMember(d => d.Qty, o => o.MapFrom(s => s.qty))
            .ForMember(d => d.Discount, o => o.MapFrom(s => s.discount))
            .ForMember(d => d.SumWithVAT, o => o.MapFrom(s => SumWithVAT(s.priceincvat, s.qty, s.discount, s.cheque_sum)))
            .ForMember(d => d.VAT5Value, o => o.MapFrom(s => s.vatsize == 5 ?
            CalculateVATValue(s.vatsize, SumWithVAT(s.priceincvat, s.qty, s.discount, s.cheque_sum), s.compensationsum, s.cheque_sum) : 0 ))
            .ForMember(d => d.VAT21Value, o => o.MapFrom(s => s.vatsize == 21 ?
            CalculateVATValue(s.vatsize, SumWithVAT(s.priceincvat, s.qty, s.discount, s.cheque_sum), s.compensationsum, s.cheque_sum) : 0))
            .ForMember(d => d.InsuranceSum, o => o.MapFrom(s => s.cheque_sum))
            .ForMember(d => d.TotalSum, o => o.MapFrom((s, d) => d.SumWithVAT + s.compensationsum + s.prepayment_compensation + s.cheque_sum));
        }

        private decimal SumWithVAT(decimal priceincvat, decimal qty, decimal discount, decimal cheque_sum)
        {
            return (priceincvat * qty) - Math.Round(((priceincvat * discount) / 100) * qty, 2) - cheque_sum;
        }

        private decimal CalculateVATValue(decimal vatRate, decimal price, decimal compensationSum, decimal insuranceSum)
        {
            return CalculateVATForAmount(vatRate, price) +
                   CalculateVATForAmount(vatRate, compensationSum) +
                   CalculateVATForAmount(vatRate, insuranceSum);
        }

        private decimal CalculateVATForAmount(decimal vatRate, decimal amount)
        {
            return amount - Math.Round(amount / (1 + (vatRate / 100)), 2);
        }
    }
}
