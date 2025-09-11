using POS_display.Models.Pos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_display.Repository.Payment
{
    public interface IPaymentRepository
    {
        Task UpdatePoshAdvancePayment(decimal id, string type, decimal totalsum);

        Task UpdateAdvancePayment(decimal advancePaymentId, decimal paymentId, decimal paidSum);

        Task<List<PaymentMethod>> GetPayment();
    }
}
