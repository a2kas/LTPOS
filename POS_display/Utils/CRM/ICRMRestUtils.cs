using Cortex.Client.Model;
using POS_display.Models.CRM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_display.Utils.CRM
{
    public interface ICRMRestUtils
    {
        Task<PostPurchaseAcceptPayment200Response> AcceptPayment(Items.posh posHeader, float maxPoints = 0, float maxCredits = 0);

        Task<PostPurchaseRecommendedRewards200Response> RecommendedBestRewards(Items.posh posHeader, string rewardListType);

        Task<bool> SendPurchase(Items.posh posHeader, bool canceled);

        Task<bool> TransferPoints(float points, string cardNr);

        Task<bool> TestConnection();

        Task<CRMClientData> CollectClientData(string cardNr);

        Task<decimal> GetCustomerPoints(string cardNr);

        Task<bool> CreateEvent(string eventTypeId, string customerId, string externalID, List<PropertyRecord> propertyRecords = null);

        Property GetEventPropertyById(string propertyId);

        string GetEventTypeByName(string name);

        Task<List<Customer>> GetCustomers(int count, string email, string phone, string firstName, string lastName, DateTime? birthDate);

        Task<string> GetCardByCustomerId(string customerID);
    }
}
