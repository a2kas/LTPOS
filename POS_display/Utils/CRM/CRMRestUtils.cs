using AutoMapper;
using Cortex.Client.Api;
using Cortex.Client.Model;
using Microsoft.Extensions.DependencyInjection;
using POS_display.Exceptions;
using POS_display.Models.CRM;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Cortex.Client.Model.PostPurchaseAcceptPaymentRequest;
using static Cortex.Client.Model.PostPurchaseRecommendedRewardsRequest;
using static POS_display.Enumerator;

namespace POS_display.Utils.CRM
{
    public class CRMRestUtils : ICRMRestUtils
    {

        #region Members
        private Dictionary<string, PurchaseType> _purchaseTypes;
        private Dictionary<string, PurchaseItemType> _purchaseItemTypes;
        private Dictionary<string, Currency> _currencies;
        private Dictionary<string, Store> _stores;
        private Dictionary<string, PointType> _pointTypes;
        private Dictionary<string, EventType> _eventTypes;
        private Dictionary<string, Property> _eventProperties;
        private readonly Dictionary<string, CRMClientData> _clients;
        private const string _donatePointTypeName = "Taškų aukos";
        private const string _nonRxPurchaseItemTypeId = "80dce3f83d1d19d7cf698c5efb";
        private const string _rxPurchaseItemTypeId = "80d68499bd9ac89ef054db1a08";
        private const string _manualDiscountPurchaseItemTypeId = "8adab563200d432b464b42db11";
        private const string _manualDiscountRxPurchaseItemTypeId = "81d02532737a91408afcc2526e";
        #endregion

        #region Constructor
        public CRMRestUtils(string endPoint, string username, string password, string version)
        {
            Cortex.Client.Client.Configuration.Default.BasePath = $"{endPoint}/{version}";
            Cortex.Client.Client.Configuration.Default.Username = username;
            Cortex.Client.Client.Configuration.Default.Password = password;
            Cortex.Client.Client.Configuration.Default.AddDefaultHeader("Connection", Session.getParam("CRM_REST", "KEEPALIVE") == "1" ? "keep-alive" : "close");
            _clients = new Dictionary<string, CRMClientData>();
        }
        #endregion

        #region Public methods
        public async Task<PostPurchaseAcceptPayment200Response> AcceptPayment(Items.posh posHeader, float maxPoints = 0, float maxCredits = 0)
        {
            if (!Session.CRM)
                return null;

            try
            {
                Bill bill = await GetBill(posHeader);
                PostPurchaseAcceptPaymentRequest postPurchaseAcceptPaymentRequest = new PostPurchaseAcceptPaymentRequest(
                    GetStoreBySystemId(Session.SystemData.kas_client_id.ToString()),
                    helpers.getIntID(Session.Devices.debtorid),
                    posHeader.CRMItem.Account.CardNumber,
                    null,
                     posHeader.LoyaltyCardType == "BENU" ? ((posHeader.CRMItem.AccruePoints == 1 || posHeader.CRMItem.AccruePoints == 5) ?
                    PaymentTypeEnum.S : PaymentTypeEnum.D) : PaymentTypeEnum.S,
                    maxPoints.ToString(),
                    maxCredits.ToString(),
                    posHeader.CRMItem?.ManualVouchers?.ConvertAll(e => e.Code),
                    bill
                    );

                return await new PurchasesApi().PostPurchaseAcceptPaymentAsync(postPurchaseAcceptPaymentRequest);
            }
            catch (WebException ex)
            {
                var error = Session.POSErrors.Where(e => e.system == "CRM" && e.code == "502").DefaultIfEmpty(new Items.Error() { description = "", type = "" }).First();
                helpers.alert(Enumerator.alert.error, error.description + "\n" + ex.Message);
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
            return new PostPurchaseAcceptPayment200Response(new AcceptedPayment());
        }

        public async Task<PostPurchaseRecommendedRewards200Response> RecommendedBestRewards(Items.posh posHeader, string rewardListType)
        {
            if (!Session.CRM)
                return null;

            try
            {
                PostPurchaseRecommendedRewardsRequest postPurchaseRecommendedRewardsRequest = new PostPurchaseRecommendedRewardsRequest
                (
                    GetStoreBySystemId(Session.SystemData.kas_client_id.ToString()),
                    helpers.getIntID(Session.Devices.debtorid),
                    posHeader.CRMItem.Account.CardNumber,
                    null,
                    rewardListType == "F" ? RewardListTypeEnum.F : RewardListTypeEnum.C,
                    await GetBill(posHeader)
                );

                return await new PurchasesApi().PostPurchaseRecommendedRewardsAsync(postPurchaseRecommendedRewardsRequest);

            }
            catch (WebException ex)
            {
                var error = Session.POSErrors.Where(e => e.system == "CRM" && e.code == "502").DefaultIfEmpty(new Items.Error() { description = "", type = "" }).First();
                helpers.alert(Enumerator.alert.error, error.description + "\n" + ex.Message);
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
                Serilogger.GetLogger().Error(ex, ex.Message);
            }

            return new PostPurchaseRecommendedRewards200Response(new PostPurchaseRecommendedRewards200ResponseData()
            {
                RecommendedBestRewards = new List<RecommendedReward>(),
                TotalItems = 0}
            );
        }

        public async Task<bool> SendPurchase(Items.posh posHeader, bool canceled)
        {
            if (!Session.CRM)
                return false;

            try
            {
                PostPurchaseSendRequest postPurchaseSendRequest = new PostPurchaseSendRequest
                (
                    GetStoreBySystemId(Session.SystemData.kas_client_id.ToString()),
                    helpers.getIntID(Session.Devices.debtorid),
                    posHeader.CRMItem.Account.CardNumber,
                    null,
                    await GetFinalBill(posHeader, canceled)
                );

                await new PurchasesApi().PostPurchaseSendAsync(postPurchaseSendRequest);
                return true;
            }
            catch (WebException ex)
            {
                var error = Session.POSErrors.Where(e => e.system == "CRM" && e.code == "502").DefaultIfEmpty(new Items.Error() { description = "", type = "" }).First();
                helpers.alert(Enumerator.alert.error, error.description + "\n" + ex.Message);
                Serilogger.GetLogger().Error(ex, ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
                Serilogger.GetLogger().Error(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> TransferPoints(float points, string cardNr)
        {
            try
            {
                GetCards200Response response = new CardsApi().GetCards(cardNumber: cardNr);              
                if (response.Data.Cards == null || response.Data.Cards.Count == 0) return false;

                string originalCustomerId = response.Data.Cards[0].CustomerId;

                response = new CardsApi().GetCards(cardNumber: Session.getParam("CRM", "DONATION_CARD_NUMBER"));
                if (response.Data.Cards == null || response.Data.Cards.Count == 0) return false;

                string newCustomerId = response.Data.Cards[0].CustomerId;

                PostPointsTransferRequest postPointsTransferRequest = new PostPointsTransferRequest
                (
                    originalCustomerId,
                    newCustomerId,
                    points,
                    GetPointTypeIdByName(_donatePointTypeName)
                );

                await new PointsApi().PostPointsTransferAsync(postPointsTransferRequest);
                return true;

            }
            catch (WebException ex)
            {
                var error = Session.POSErrors.Where(e => e.system == "CRM" && e.code == "502").DefaultIfEmpty(new Items.Error() { description = "", type = "" }).First();
                helpers.alert(Enumerator.alert.error, error.description + "\n" + ex.Message);
                Serilogger.GetLogger().Error(ex, ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
                Serilogger.GetLogger().Error(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> TestConnection()
        {
            try
            {
                string testKey = "test";
                var result = await new TestsApi().GetTestsConnectionAsync(testKey);
                bool enableCRM = result != null && result.Data.TestString == Reverse(testKey);
                Session.CRM = enableCRM;
                return enableCRM;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return false;
            }
        }

        public async Task<CRMClientData> CollectClientData(string cardNr)
        {
            CRMClientData clientData;
            try
            {
                if (_clients.ContainsKey(cardNr))
                    return _clients[cardNr];

                GetCards200Response cardsResponse = await new CardsApi().GetCardsAsync(cardNumber: cardNr);
                if (cardsResponse.Data.Cards == null || cardsResponse.Data.Cards.Count == 0)
                    throw new CRMException($"Lojalumo kortelė {cardNr} neegizstuoja");

                if (cardsResponse.Data.Cards[0].CustomerId == null)
                    throw new CRMException($"Lojalumo kortelė {cardNr} nėra priskirta jokiam klientui");

                GetCustomer200Response customerResponse = await new CustomersApi().GetCustomerAsync(cardsResponse.Data.Cards[0].CustomerId);
                var customer = customerResponse.Data;
                var mapper = Program.ServiceProvider.GetRequiredService<IMapper>();
                clientData = mapper.Map<CRMClientData>(customerResponse.Data);
                clientData.CardNumber = cardNr;
                clientData.IsCardActive = cardsResponse.Data.Cards[0].State == Card.StateEnum.NUMBER_1;

                _clients.Add(cardNr, clientData);
                return clientData;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                throw;
            }
        }

        public async Task<decimal> GetCustomerPoints(string cardNr) 
        {
            decimal points = 0;
            string customerId = string.Empty;
            try
            {
                if (_clients.ContainsKey(cardNr))
                    customerId = _clients[cardNr].ID;
                else 
                {
                    var clientData = await CollectClientData(cardNr);
                    customerId = clientData.ID;
                }

                GetWalletPoints200Response response = await new WalletApi().GetWalletPointsAsync(customerId);
                return (decimal)response.Data.AvailablePoints;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return points;
            }
        }

        public async Task<bool> CreateEvent(string eventTypeId, string customerId, string externalID, List<PropertyRecord> propertyRecords = null)
        {
            try
            {
                Event evt = new Event(eventTypeId, customerId, externalID);
                PostEventRequest postEventRequest = new PostEventRequest(evt, propertyRecords);
                PostEvent201Response response = await new EventsApi().PostEventAsync(postEventRequest);
                return response?.Data != null && !string.IsNullOrEmpty(response.Data.EventId);
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return false;
            
            }
        }

        public string GetEventTypeByName(string name)
        {
            try
            {
                if (_eventTypes == null)
                {
                    GetEventTypes200Response response = new EventTypesApi().GetEventTypes();
                    _eventTypes = new Dictionary<string, EventType>();
                    foreach (EventType eventType in response.Data.EventTypes)
                    {
                        if (string.IsNullOrEmpty(eventType.Name))
                            continue;
                        if (!_eventTypes.ContainsKey(eventType.Name))
                            _eventTypes.Add(eventType.Name, eventType);
                    }
                }

                if (_eventTypes.ContainsKey(name))
                    return _eventTypes[name].EventTypeId;

                return string.Empty;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return string.Empty;
            }
        }

        public Property GetEventPropertyById(string propertyId)
        {
            try
            {
                if (_eventProperties == null)
                {
                    GetEventProperties200Response response = new EventPropertiesApi().GetEventProperties(count: 500);
                    _eventProperties = new Dictionary<string, Property>();
                    foreach (Property property in response.Data.EventProperties)
                    {
                        if (string.IsNullOrEmpty(property.PropertyId))
                            continue;
                        if (!_eventProperties.ContainsKey(property.PropertyId))
                            _eventProperties.Add(property.PropertyId, property);
                    }
                }

                if (_eventProperties.ContainsKey(propertyId))
                    return _eventProperties[propertyId];

                return null;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return null;
            }
        }

        public async Task<List<Customer>> GetCustomers(int count, string email, string phone, string firstName, string lastName, DateTime? birthDate)
        {
            try
            {
                GetCustomers200Response response = await new CustomersApi().GetCustomersAsync(
                    null, count, null, null, null, !string.IsNullOrEmpty(email) ? email : null,
                    !string.IsNullOrEmpty(phone) ? phone : null, null,
                    !string.IsNullOrEmpty(firstName) ? firstName : null,
                    !string.IsNullOrEmpty(lastName) ? lastName : null,
                    birthDate.HasValue ? birthDate.Value.ToString("yyyy-MM-dd") : null);
                return response?.Data?.Customers ?? new List<Customer>();
            }
            catch (CRMException ex) 
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return new List<Customer>();
            }
        }

        public async Task<string> GetCardByCustomerId(string customerID)
        {
            try
            {
                GetCards200Response response = await new CardsApi().GetCardsAsync(customerId: customerID, isValid: true);
                return response?.Data?.Cards.FirstOrDefault()?.CardNumber ?? string.Empty;
            }
            catch (CRMException ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return string.Empty;
            }
        }
        #endregion


        #region Private methods
        public string GetId(string combID)
        {
            string NewId = combID;
            NewId = NewId.Replace("C", "");
            if (NewId.IndexOf(Session.SystemData.kas_client_id.ToString()) == 0)
                NewId = NewId.Substring(Session.SystemData.kas_client_id.ToString().Length);
            if (NewId.IndexOf('_') > 0)
                NewId = NewId.Substring(0, NewId.IndexOf('_'));

            return NewId;
        }

        private string GetStoreBySystemId(string systemId)
        {
            try
            {
                if (_stores == null)
                {
                    GetStores200Response response = new StoresApi().GetStores();
                    _stores = new Dictionary<string, Store>();
                    foreach (Store store in response.Data.Stores)
                    {
                        if (string.IsNullOrEmpty(store.SystemId))
                            continue;
                        if (!_stores.ContainsKey(store.SystemId))
                            _stores.Add(store.SystemId, store);
                    }
                }

                if (_stores.ContainsKey(systemId))
                    return _stores[systemId].StoreId;

                return string.Empty;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return string.Empty;
            }
        }

        private string GetCurrencyIdByName(string name)
        {
            try
            {
                if (_currencies == null)
                {
                    GetCurrencies200Response response = new CurrenciesApi().GetCurrencies();
                    _currencies = new Dictionary<string, Currency>();
                    foreach (Currency currency in response.Data.Currencies)
                    {
                        if (string.IsNullOrEmpty(currency.Name))
                            continue;
                        if (!_currencies.ContainsKey(currency.Name))
                            _currencies.Add(currency.Name, currency);
                    }
                }

                if (_currencies.ContainsKey(name))
                    return _currencies[name].CurrencyId;

                return string.Empty;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return string.Empty;
            }
        }

        private string GetPurchaseItemTypeIdByName(string name)
        {
            try
            {
                if (_purchaseItemTypes == null)
                {
                    GetPurchaseItemTypes200Response response = new PurchaseItemTypesApi().GetPurchaseItemTypes();
                    _purchaseItemTypes = new Dictionary<string, PurchaseItemType>();
                    foreach (PurchaseItemType purchaseItemType in response.Data.PurchaseItemTypes)
                    {
                        if (string.IsNullOrEmpty(purchaseItemType.Name))
                            continue;
                        if (!_purchaseItemTypes.ContainsKey(purchaseItemType.Name))
                            _purchaseItemTypes.Add(purchaseItemType.Name, purchaseItemType);
                    }
                }

                if (_purchaseItemTypes.ContainsKey(name))
                    return _purchaseItemTypes[name].TypeId;

                return string.Empty;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return string.Empty;
            }
        }

        private string GetPurchaseTypedByName(string name)
        {
            try
            {
                if (_purchaseTypes == null)
                {
                    GetPurchaseTypes200Response response = new PurchaseTypesApi().GetPurchaseTypes();
                    _purchaseTypes = new Dictionary<string, PurchaseType>();
                    foreach (PurchaseType purchaseType in response.Data.PurchaseTypes)
                    {
                        if (string.IsNullOrEmpty(purchaseType.Name))
                            continue;
                        if (!_purchaseTypes.ContainsKey(purchaseType.Name))
                            _purchaseTypes.Add(purchaseType.Name, purchaseType);
                    }
                }

                if (_purchaseTypes.ContainsKey(name))
                    return _purchaseTypes[name].TypeId;

                return string.Empty;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return string.Empty;
            }
        }

        private string GetPointTypeIdByName(string name)
        {
            try
            {
                if (_pointTypes == null)
                {
                    GetPointsTypes200Response response = new PointTypesApi().GetPointsTypes();
                    _pointTypes = new Dictionary<string, PointType>();
                    foreach (PointType pointType in response.Data.PointTypes)
                    {
                        if (string.IsNullOrEmpty(pointType.Name))
                            continue;
                        if (!_pointTypes.ContainsKey(pointType.Name))
                            _pointTypes.Add(pointType.Name, pointType);
                    }
                }

                if (_pointTypes.ContainsKey(name))
                    return _pointTypes[name].PointTypeId;

                return string.Empty;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                return string.Empty;
            }
        }

        private async Task<Bill> GetBill(Items.posh posHeader, CRMBillType billType = CRMBillType.None)
        {
            List<BillItem> billItems = FillBillItem(posHeader, billType);
            return new Bill
            (
                Session.SystemData.kas_client_id + posHeader.Id.ToString() + "_" + (await DB.Loyalty.getCounter(posHeader.Id)),
                posHeader.Id.ToString(),
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Session.User.DisplayName,
                GetCurrencyIdByName(Session.SystemData.currencycode),
                (float)posHeader.TotalSum,
                billItems
            );
        }

        private List<BillItem> FillBillItem(Items.posh posHeader, CRMBillType billType)
        {
            SR_TamroWS.TamroWSSoapClient scTamroWS = new SR_TamroWS.TamroWSSoapClient();
            List<BillItem> billItems = new List<BillItem>();
            foreach (var posdItem in posHeader.PosdItems) 
            {
                var purchaseItemTypeId = ResolvePurchaseItemTypeId(posdItem);
                var price = ResolvePriceValue(purchaseItemTypeId, posdItem);
                var paidAmount = ResolvePaidAmountValue(billType, purchaseItemTypeId, posdItem);

                var billItem = new BillItem
                (
                    new List<PluId> { new PluId("GLOBAL", posdItem.productid.ToString()) },
                    posdItem.barcodename,
                    posdItem.baltic_category_id != 0 ? posdItem.baltic_category_id.ToString() :
                    scTamroWS.GetBalticCategory(posdItem.productid).c0_id.ToString(),
                    (float)posdItem.vatsize,
                    (float)posdItem.qty,
                    (float)Math.Round(paidAmount, 2),
                    (float)Math.Round(price, 2),
                    Session.SystemData.kas_client_id + posdItem.id.ToString(),
                    NeedTurnOffLoyalty(posdItem),
                    purchaseItemTypeId,
                    null
                );
                billItems.Add(billItem);
            }
            scTamroWS.Close();
            return billItems.Count != 0 ? billItems : new List<BillItem>();
        }

        private async Task<FinalBill> GetFinalBill(Items.posh posHeader, bool canceled)
        {
            string purchaseTypeId = GetPurchaseTypedByName(GetPurchaseTypeName(posHeader));

            PaymentRecap paymentRecap = new PaymentRecap()
            {
                CreditPoints = posHeader.CRMItem?.AcceptedPaymentResponse?.Data?.CreditPoints ?? 0,
                RecommendedDiscounts = posHeader.CRMItem?.AcceptedPaymentResponse?.Data?.RecommendedDiscounts ?? new List<DiscountItem>(),
                Vouchers = posHeader.CRMItem?.AcceptedPaymentResponse?.Data?.Vouchers ?? new List<PaymentVoucher>()
            };

            List<BillItem> billItems = FillBillItem(posHeader, CRMBillType.SendPurchase);

            return new FinalBill
            (
                Session.Devices.fiscal == 1m ? FinalBill.FiscalEnum.True : FinalBill.FiscalEnum.False,
                purchaseTypeId,
                canceled ? FinalBill.CanceledEnum.True : FinalBill.CanceledEnum.False,
                FinalBill.PaymentTypeEnum.S,
                paymentRecap,
                (canceled ? "C" : "") + Session.SystemData.kas_client_id + posHeader.Id.ToString() + "_" + (await DB.Loyalty.getCounter(posHeader.Id) - (canceled ? 1 : 0)),
                Session.SystemData.kas_client_id + posHeader.Id.ToString() + "_" + (await DB.Loyalty.getCounter(posHeader.Id) - (canceled ? 1 : 0)),
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Session.User.DisplayName,
                GetCurrencyIdByName(Session.SystemData.currencycode),
                (float)posHeader.TotalSum,
                null,
                billItems
            );
        }

        private string Reverse(string Input)
        {
            char[] charArray = Input.ToCharArray();
            string reversedString = string.Empty;

            for (int i = charArray.Length - 1; i > -1; i--)
                reversedString += charArray[i];

            return reversedString;
        }

        private BillItem.LoyaltyOffEnum NeedTurnOffLoyalty(Items.posd posDetailItem)
        {
            if (posDetailItem.gr4.ToLowerInvariant() == "rx-z" && posDetailItem.recipeid != 0 && posDetailItem.compensationsum != 0m)
                return BillItem.LoyaltyOffEnum.False;
            if (posDetailItem.recipeid != 0 && posDetailItem.compensationsum != 0m)
                return BillItem.LoyaltyOffEnum.True;
            if (posDetailItem.is_deposit)
                return BillItem.LoyaltyOffEnum.True;

            return BillItem.LoyaltyOffEnum.False;
        }

        private string GetPurchaseTypeName(Items.posh posHeader)
        {
            return posHeader.CRMItem.AccruePoints == 5 ?
                "Dovana" :
                "Pirkimo vaistinėje sąskaita";
        }

        private string ResolvePurchaseItemTypeId(Items.posd posDetailItem)
        {
            List<string> rxGr4List = new List<string>() { "rx", "g", "vac" };
            if (posDetailItem.loyalty_type == "P" && posDetailItem.discount_sum > 0)
            {
                if (rxGr4List.Any(prefix => posDetailItem.gr4.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase)))
                    return _manualDiscountRxPurchaseItemTypeId;
                else
                    return _manualDiscountPurchaseItemTypeId;
            }
            else if (rxGr4List.Any(prefix => posDetailItem.gr4.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase)))
                return _rxPurchaseItemTypeId;
            else
                return _nonRxPurchaseItemTypeId;
        }

        private decimal ResolvePaidAmountValue(CRMBillType billType, string purchaseItemTypeId, Items.posd posDetailItem)
        {
            if (purchaseItemTypeId == _manualDiscountPurchaseItemTypeId)
                return posDetailItem.qty * posDetailItem.pricediscounted;

            return billType == CRMBillType.SendPurchase ? 
                posDetailItem.qty * posDetailItem.pricediscounted :
                posDetailItem.qty * posDetailItem.price;
        }

        private decimal ResolvePriceValue(string purchaseItemTypeId, Items.posd posDetailItem)
        {
            return purchaseItemTypeId == _manualDiscountPurchaseItemTypeId ?
                posDetailItem.qty * posDetailItem.pricediscounted :
                posDetailItem.qty * posDetailItem.price;
        }
        #endregion
    }
}
