using Dapper;
using POS_display.Models.ECRReports;
using POS_display.Models.Pos;
using POS_display.Models.Price;
using POS_display.Models.TLK;
using POS_display.wpf.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace POS_display.Repository.Pos
{
    public class PosRepository : BaseRepository, IPosRepository
    {
        public delegate void dbCallback(bool success, List<string> data);
        public async Task CreatePosPayment(PosPayment posPayment)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(PosQueries.CreatePosPayment, posPayment);
            }
        }

        public async Task<List<PosPayment>> GetPosPayment(decimal hid, bool deleted = false)
        {
            var posPayments = new List<PosPayment>();
            using (var connection = DB_Base.GetConnection())
            {
                posPayments = (await connection.QueryAsync<PosPayment>(PosQueries.GetPosPayment, new { Hid = hid, deleted })).ToList();
            }

            return posPayments;
        }

        public async Task VoidPosPayment(PosPayment posPayment)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(PosQueries.VoidPosPayment, posPayment);
            }
        }

        public async Task UpdateSession(string action, decimal f_mode)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.QueryAsync(PosQueries.UpdateSession,
                    new { 
                        db_code = Session.SystemData.code,                             
                        db_userid = Session.User.id,
                        Host = Session.LocalIP, 
                        Action = action, 
                        Fmode = f_mode 
                    });
            }
        }

        public async Task<bool> ChangePosDPrice(PosDPrice posDPrice)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<bool>(PosQueries.ChangePosdPrice, new { id = posDPrice?.PosdId, price_new = posDPrice?.Price });
            }
        }

        public async Task AsyncAutoComplete(string tableName, string columnName, dbCallback callback)
        {
            using (var connection = DB_Base.GetConnection())
            {
                bool success = false;
                var data = new List<string>();
                // Can not pass table and column names via parameters, because dapper does not support it due to sql injection protection
                string query = PosQueries.AsyncAutoComplete;
                query = query.Replace("@tableName", tableName);
                query = query.Replace("@columnName", columnName);

                try
                {
                    data = (await connection.QueryAsync<string>(query)).ToList();
                    success = true;
                }
                finally
                {
                    callback(success, data);
                }
            }
        }

        public async Task<decimal> GetECRReports(decimal cmb_report, decimal edt_change)
        {
            using (var connection = DB_Base.GetConnectionWithoutCommandTimeout())
            {
                return await connection.QueryFirstAsync<decimal>(PosQueries.AsyncECRReports,
                    new
                    {
                        db_code = Session.SystemData.code,
                        cmb_report,
                        Session.Devices.debtorid,
                        db_userid= Session.User.id,
                        edt_change
                    });
            }
        }

        public async Task CreatePosZ(PosZLine posZData)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.QueryAsync(PosQueries.InsertPosZ, posZData);
            }
        }

        public async Task<string> GetProductGr4(long productId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<string>(PosQueries.GetProductGr4, new { productId });
            }
        }

        public async Task<List<fmd>> GetFMDItem(long posh_id, long posd_id)
        {
            var fmdItems = new List<fmd>();
            using (var connection = DB_Base.GetConnection())
            {
                fmdItems = (await connection.QueryAsync<fmd>(PosQueries.GetFMDItemByPosIds, new { posh_id, posd_id })).ToList();
            }

            return fmdItems;
        }

        public async Task<List<fmd>> GetFMDItem(long id)
        {
            var fmdItems = new List<fmd>();
            using (var connection = DB_Base.GetConnection())
            {
                fmdItems = (await connection.QueryAsync<fmd>(PosQueries.GetFMDItem, new { id })).ToList();
            }

            return fmdItems;
        }

        public async Task<long> DeleteFMDItem(long id)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<long>(PosQueries.DeleteFMDItem, new { id });
            }
        }

        public async Task<long> CreateFMDtrans(fmd model)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<long>(PosQueries.CreateFMDItem, new
                {
                    posh_id = model.posDetail?.hid ?? 0,
                    posd_id = model.posDetail?.id ?? 0,
                    documentdate = DateTime.Now,
                    debtorid = Session.Devices.debtorid,
                    userid = Session.User.id,
                    product_code_scheme = model.productCodeScheme,
                    product_code = model.productCode,
                    serial_number = model.serialNumber,
                    batch_id = model.batchId,
                    expiry_date = model.expiryDate,
                    type = model.type,
                    operation_code = model.Response.operationCode,
                    state = model.Response.state,
                    information = model.Response.information,
                    warning = model.Response.warning,
                    alert_id = model.Response.alertId,
                    isvalidforsale = model.Response.Success,
                    referencenumber = model.referencenumber,
                    deleted = model.deleted
                });
            }
        }

        public async Task<long> UpdateFMDtrans(fmd model)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<long>(PosQueries.UpdateFMDItem, new
                {
                    id = model.id,
                    operation_code = model.Response.operationCode,
                    state = model.Response.state,
                    information = model.Response.information,
                    warning = model.Response.warning,
                    alert_id = model.Response.alertId,
                    isvalidforsale = model.Response.Success,
                    referencenumber = model.referencenumber,
                    deleted = model.deleted
                });
            }
        }

        public async Task<bool> CheckIfSuppliedFMDItemExist(long productId, string productCode, string batchId, string serialNumber)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<bool>(PosQueries.CheckIfSuppliedFMDItemExist,
                    new
                    {
                        product_id = productId,
                        product_code = productCode,
                        batch_id = batchId,
                        serial_number = serialNumber
                    });
            }
        }

        public async Task<bool> TransferChequesFromPOS(decimal dbcode, decimal debtorId, string date)
        {
            using (var connection = DB_Base.GetConnectionWithoutCommandTimeout())
            {
                return await connection.QueryFirstAsync<bool>(PosQueries.CreateSaledFromPOS,
                    new
                    {
                        db_code = dbcode,
                        debtor_id = debtorId,
                        date = date
                    });
            }
        }

        public async Task<bool> IsCompensated(long productId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<bool>(PosQueries.IsCompensated, new  { product_id = productId  });
            }
        }

        public async Task<bool> IsCheapest(string npkaid7)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<bool>(PosQueries.IsCheapest, new { npkaid7 });
            }
        }

        public async Task<List<TLKPriceDetail>> GetTLKPricesByNpakid7List(List<string> listOfNpkaid7)
        {
            var tLKPriceDetails = new List<TLKPriceDetail>();
            if(listOfNpkaid7 == null || listOfNpkaid7.Count == 0)
                return tLKPriceDetails;

            string args = string.Join(", ", listOfNpkaid7.Select(n => $"'{n}'"));
            using (var connection = DB_Base.GetConnection())
            {
                tLKPriceDetails = (await connection.QueryAsync<TLKPriceDetail>(string.Format(PosQueries.GetTLKPricesByNpakid7List, args))).ToList();
            }

            return tLKPriceDetails;
        }

        public async Task<List<TLKPriceDetail>> GetTLKPricesByProductsIds(List<long> productIDs)
        {
            var tLKPriceDetails = new List<TLKPriceDetail>();
            if (productIDs == null || productIDs.Count == 0)
                return tLKPriceDetails;

            string args = string.Join(", ", productIDs.Select(n => $"'{n}'"));
            using (var connection = DB_Base.GetConnection())
            {
                tLKPriceDetails = (await connection.QueryAsync<TLKPriceDetail>(string.Format(PosQueries.GetTLKPricesByProductIDs, args))).ToList();
            }

            return tLKPriceDetails;
        }

        public async Task<List<PromotionCheque>> GetPromotionCheques()
        {
            var promotionCheques = new List<PromotionCheque>();
            using (var connection = DB_Base.GetConnection())
            {
                promotionCheques = (await connection.QueryAsync<PromotionCheque>(PosQueries.GetPromotionCheques)).ToList();
            }

            return promotionCheques;
        }

        public async Task<List<GenericItemAdditionalData>> GetGenericItemDataByProductIds(List<long> productIDs)
        {
            var genericItemsData = new List<GenericItemAdditionalData>();
            if (productIDs == null || productIDs.Count == 0)
                return genericItemsData;

            string args = string.Join(", ", productIDs.Select(n => $"'{n}'"));
            using (var connection = DB_Base.GetConnection())
            {
                genericItemsData = (await connection.QueryAsync<GenericItemAdditionalData>(string.Format(PosQueries.GetGenericItemsData, args))).ToList();
            }

            return genericItemsData;
        }

        public async Task<decimal> GetUserPrioritiesRationInPeriod(long userId, DateTime from, DateTime to)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(PosQueries.GetUserPrioritesRatioInPeriod,
                    new 
                    {
                        userId, 
                        from,
                        to
                    });
            }
        }

        public async Task<Dictionary<decimal, ProductLocation>> LoadProductsLocations() 
        {
            Dictionary<decimal, ProductLocation> productDictionary = new Dictionary<decimal, ProductLocation>();
            using (var connection = DB_Base.GetConnection())
            {
                var productList = (await connection.QueryAsync<ProductLocation>(PosQueries.GetProductsLocations)).ToList();
                productDictionary = productList.ToDictionary(product => product.ProductId, product => product);
            }
            return productDictionary;
        }

        public async Task<decimal> CreateAdvancePayment(decimal poshId, string advancePaymentType, string orderNumber, decimal price, decimal presentCardId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(PosQueries.CreateAdvancePayment,
                new
                    {
                    poshId,
                    advancePaymentType,
                    orderNumber,
                    price,
                    presentCardId
                });
            }
        }

        public async Task<decimal> DeleteAdvancePayment(decimal advancepaymentid)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(PosQueries.DeleteAdvancePayment, new { advancepaymentid });
            }
        }

        public async Task<decimal> CreateAdvancePaymentDuplicate(decimal poshId, decimal advancepaymentid)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(PosQueries.CreateAdvancePaymentDuplicate, new { poshId, advancepaymentid });
            }
        }

        public async Task<decimal> CreateSaleDetail(decimal hid, decimal productid, decimal barcodeid, decimal serialid, decimal qty, decimal price, 
            decimal discount, decimal pricediscounted, decimal sum, decimal vatamount, 
            decimal costprice, decimal fifoid, decimal objectid, decimal jobhid, decimal vatproc, decimal returned_posd_id)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(PosQueries.CreateSaleDetail, 
                    new 
                    {
                        db_code = Session.SystemData.code,
                        hid,
                        Session.Devices.debtorid,
                        productid,
                        barcodeid,
                        Session.SystemData.storeid,
                        serialid,
                        qty,
                        price,
                        discount,
                        pricediscounted,
                        sum,
                        vatamount,
                        costprice,
                        Session.SystemData.currencyid,
                        fifoid,
                        objectid,
                        jobhid,
                        vatproc,
                        returned_posd_id
                    });
            }
        }

        public async Task<decimal> CreateSaleHeader()
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(PosQueries.CreateSaleHeader,
                    new
                    {
                        db_code = Session.SystemData.code 
                    });
            }
        }

        public async Task UpdateSaleHeader(decimal id, string type, string documentno, DateTime documentdate, 
            decimal currate, decimal payedfor, decimal corect0, decimal corect5, 
            decimal corect18, decimal corectdis, decimal corect19, decimal corect21)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(PosQueries.UpdateSaleHeader,
                    new
                    {
                        id,
                        Session.Devices.debtorid,
                        type,
                        documentno,
                        documentdate = documentdate.ToString("yyyy.MM.dd"),
                        Session.SystemData.currencyid,
                        currate,
                        payedfor,
                        corect0,
                        corect5,
                        corect18,
                        corectdis,
                        corect19,
                        corect21
                    });
            }
        }

        public async Task SetSaleHeaderTransId(decimal id, decimal transId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(PosQueries.SetSaleHeaderTransId, new { id, transId });
            }
        }

        public async Task<decimal> ReturnSaleHeader(decimal id)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(PosQueries.ReturnSaleHeader, new 
                { 
                    id,
                    db_code = Session.SystemData.code
                });
            }
        }

        public async Task<EKJEntry> GetLastEKJEntryByType(Enumerator.EKJType ekjType) 
        {
            using (var connection = DB_Base.GetSqliteConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<EKJEntry>(PosQueries.GetLastEKJEntryByType, new { ekjType });
            }
        }

        public async Task<ZReportEntry> GetZReportEntryByEkjId(int id)
        {
            using (var connection = DB_Base.GetSqliteConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<ZReportEntry>(PosQueries.GetZReportEntryByEkjId, new { ekj_id = id });
            }
        }
		
        public async Task<DeviceSettingValue> GetDeviceSetttingValueByKey(int key)
        {
            using (var connection = DB_Base.GetSqliteConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<DeviceSettingValue>(PosQueries.GetDeviceSetttingValueByKey, new { key });
            }
        }

        public async Task<decimal> CalculateRimiDiscount(decimal posHeaderId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(PosQueries.CalculateRimiDiscount, new { posHeaderId });
            }
        }

        public async Task<bool> HasChequePresentCard(decimal posHeaderId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<bool>(PosQueries.HasChequePresentCard, new { posHeaderId });
            }
        }

        public async Task InsertPosMemo(decimal debtorId, Enumerator.POSMemoParamter parameter, string value)
        {
            using (var connection = DB_Base.GetConnection())
            {
                var paramKey = parameter.ToString();
                await connection.QueryAsync(PosQueries.InsertPosMemo, new { debtorId, paramKey, value });
            }
        }

        public async Task UpdatePosMemo(decimal debtorId, Enumerator.POSMemoParamter parameter, string value)
        {
            using (var connection = DB_Base.GetConnection())
            {
                var paramKey = parameter.ToString();
                await connection.QueryAsync(PosQueries.UpdatePosMemo, new { debtorId, paramKey, value });
            }
        }

        public async Task<string> GetPosMemoValue(decimal debtorId, Enumerator.POSMemoParamter parameter)
        {
            using (var connection = DB_Base.GetConnection())
            {
                var paramKey = parameter.ToString();
                return await connection.QueryFirstOrDefaultAsync<string>(PosQueries.GetPosMemoValue, new { debtorId, paramKey });
            }
        }

        public async Task<Dictionary<decimal, Enumerator.ProductFlag>> LoadProductFlags()
        {
            Dictionary<decimal, Enumerator.ProductFlag> productFlags = new Dictionary<decimal, Enumerator.ProductFlag>();
            using (var connection = DB_Base.GetConnection())
            {
                var productFlagsList = (await connection.QueryAsync<ProductFlag>(PosQueries.GetProductFalgs)).ToList();
                productFlags = productFlagsList.ToDictionary(f => f.ProductId, f => f.Flag);
            }
            return productFlags;
        }

        public async Task<Dictionary<decimal, decimal>> LoadExclusiveProducts() 
        {
            Dictionary<decimal, decimal> exclusiveProducts = new Dictionary<decimal, decimal>();
            using (var connection = DB_Base.GetConnection())
            {
                var exclusiveProductsList = (await connection.QueryAsync<ExclusiveProduct>(PosQueries.GetExclusiveProducts)).ToList();
                exclusiveProducts = exclusiveProductsList.ToDictionary(f => f.ProductId, f => f.Npakid7);
            }
            return exclusiveProducts;
        }

        public async Task<List<TLKCheapest>> GetTLKCheapests()
        {
            var tlkCheapests = new List<TLKCheapest>();

            using (var connection = DB_Base.GetConnection())
            {
                tlkCheapests = (await connection.QueryAsync<TLKCheapest>(PosQueries.GetCheapests)).ToList();
            }

            return tlkCheapests;
        }

        public async Task<List<LackOfSale>> GetLackOfSales()
        {
            var lackOfSales = new List<LackOfSale>();

            using (var connection = DB_Base.GetConnection())
            {
                lackOfSales = (await connection.QueryAsync<LackOfSale>(PosQueries.GetLackOfSales)).ToList();
            }

            return lackOfSales;
        }

        public async Task WriteDBLog(string log)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(PosQueries.WriteDBLog, new { log });
            }
        }

        public async Task<string> GetBarcodeByProductId(decimal productId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<string>(PosQueries.GetBarcodeByProductId, new { productId });
            }
        }

        public async Task<List<Items.posd>> GetPosDetails(decimal HID)
        {
            var posDetails = new List<Items.posd>();
            using (var connection = DB_Base.GetConnection())
            {
                posDetails = (await connection.QueryAsync<Items.posd>(PosQueries.GetPosd, new { HID })).ToList();
            }
            return posDetails;
        }
    }
}
