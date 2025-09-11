using Dapper;
using POS_display.Models.Pos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_display.Repository.Payment
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        public async Task UpdatePoshAdvancePayment(decimal id, string type, decimal totalsum)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(PaymentQueries.UpdatePoshAdvancePayment, new { id, type, totalsum });
            }
        }

        public async Task UpdateAdvancePayment(decimal advancePaymentId, decimal paymentId, decimal paidSum)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.QueryFirstAsync<bool>(PaymentQueries.UpdateAdvancePayment, new { advancePaymentId, paymentId, paidSum });
            }
        }

        public async Task<List<PaymentMethod>> GetPayment()
        {
            var payments = new List<PaymentMethod>();
            using (var connection = DB_Base.GetConnection())
            {
                payments = (await connection.QueryAsync<PaymentMethod>(PaymentQueries.GetPayment)).ToList();
            }
            return payments;
        }
    }
}
