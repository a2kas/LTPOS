using POS_display.Models.ECRReports;
using POS_display.Models.Pos;
using POS_display.Models.Price;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS_display.wpf.Model;
using System;
using POS_display.Models.TLK;

namespace POS_display.Repository.Pos
{
    public interface IPosRepository
    {
        Task CreatePosPayment(PosPayment posPayment);

        Task<List<PosPayment>> GetPosPayment(decimal hid, bool deleted = false);

        Task VoidPosPayment(PosPayment posPayment);

        Task UpdateSession(string action, decimal f_mode);

        Task<bool> ChangePosDPrice(PosDPrice posDPrice);

        Task<decimal> GetECRReports(decimal cmb_report, decimal edt_change);

        Task CreatePosZ(PosZLine posZData);

        Task<string> GetProductGr4(long productId);

        Task<List<fmd>> GetFMDItem(long posh_id, long posd_id);

        Task<List<fmd>> GetFMDItem(long id);

        Task<long> DeleteFMDItem(long id);

        Task<long> CreateFMDtrans(fmd model);

        Task<long> UpdateFMDtrans(fmd model);

        Task<bool> TransferChequesFromPOS(decimal dbcode, decimal debtorId, string date);

        Task<bool> IsCompensated(long productId);

        Task<bool> IsCheapest(string npkaid7);

        Task<List<TLKPriceDetail>> GetTLKPricesByNpakid7List(List<string> listOfNpkaid7);

        Task<List<PromotionCheque>> GetPromotionCheques();

        Task<List<GenericItemAdditionalData>> GetGenericItemDataByProductIds(List<long> productIDs);

        Task<List<TLKPriceDetail>> GetTLKPricesByProductsIds(List<long> productIDs);

        Task<decimal> GetUserPrioritiesRationInPeriod(long userId, DateTime from, DateTime to);

        Task<Dictionary<decimal, ProductLocation>> LoadProductsLocations();

        Task<decimal> CreateAdvancePayment(decimal poshId, string advancePaymentType, string orderNumber, decimal price, decimal presentCardId);

        Task<decimal> DeleteAdvancePayment(decimal advancepaymentid);

        Task<decimal> CreateAdvancePaymentDuplicate(decimal poshId, decimal advancepaymentid);

        Task<decimal> CreateSaleDetail(decimal hid, decimal productid, decimal barcodeid, decimal serialid, decimal qty, decimal price,
            decimal discount, decimal pricediscounted, decimal sum, decimal vatamount,
            decimal costprice, decimal fifoid, decimal objectid, decimal jobhid, decimal vatproc, decimal returned_posd_id);

        Task<decimal> CreateSaleHeader();

       Task UpdateSaleHeader(decimal id, string type, string documentno, DateTime documentdate,
            decimal currate, decimal payedfor, decimal corect0, decimal corect5,
            decimal corect18, decimal corectdis, decimal corect19, decimal corect21);

        Task<decimal> ReturnSaleHeader(decimal id);

        Task<EKJEntry> GetLastEKJEntryByType(Enumerator.EKJType ekjType);

        Task<ZReportEntry> GetZReportEntryByEkjId(int id);

        Task<DeviceSettingValue> GetDeviceSetttingValueByKey(int key);

        Task<decimal> CalculateRimiDiscount(decimal posHeaderId);

        Task<bool> HasChequePresentCard(decimal posHeaderId);

        Task InsertPosMemo(decimal debtorId, Enumerator.POSMemoParamter parameter, string value);

        Task UpdatePosMemo(decimal debtorId, Enumerator.POSMemoParamter parameter, string value);

        Task<string> GetPosMemoValue(decimal debtorId, Enumerator.POSMemoParamter parameter);

        Task<Dictionary<decimal, Enumerator.ProductFlag>> LoadProductFlags();

        Task<Dictionary<decimal, decimal>> LoadExclusiveProducts();

        Task<List<TLKCheapest>> GetTLKCheapests();

        Task WriteDBLog(string log);

        Task<List<LackOfSale>> GetLackOfSales();

        Task<string> GetBarcodeByProductId(decimal productId);

        Task<List<Items.posd>> GetPosDetails(decimal posh_id);

        Task SetSaleHeaderTransId(decimal id, decimal transId);
    }
}