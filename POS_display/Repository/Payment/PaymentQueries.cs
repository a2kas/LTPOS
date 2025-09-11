namespace POS_display.Repository.Payment
{
    public static class PaymentQueries
    {
        public static string UpdatePoshAdvancePayment => "UPDATE posh SET type = @type, status = 'T', totalsum = @totalsum, deviceno='', checkno='' WHERE id = @id;";

        public static string UpdateAdvancePayment => @"SELECT update_advancepayment(@advancepaymentid, @paymentId, @paidSum);";

        public static string GetPayment => "SELECT * FROM payment";
    }
}
