using MonitorSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonitorSDK
{
    class PriceMonitor
    {

        public enum Pformat_dec
        {
            INTEGER = 1,
            FLOAT
        }

        public enum Format
        {
            JSON,
            CSV
        }

        public enum ExportAll
        {
            TRUE,
            FALSE

        }

        public enum Test
        {
            TRUE,
            FALSE

        }

        public enum Separator
        {
            COMMA,
            SEMICOLON,
            TAB

        }

        public enum Lineend
        {
            WIN,
            UNIX

        }

        public enum Keepold
        {
            TRUE,
            FALSE
        }

        public enum Cleanold
        {
            TRUE,
            FALSE

        }

        public enum SortBy
        {
            TOTAL_PRICE,
            PRICE,
            SHIPPING_COST,
            RANKING
        }


        public enum Repricing
        {
            ON,
            OFF
        }

        public enum EAN
        {
            ON,
            OFF
        }


        enum Action
        {
            GET_PRICE_UPDATES,
            IMPORT_PRODUCTS,
            EXPORT,
            GET_ERRORS,
            DELETE_PRODUCTS,
            MARKETPLACE_SETTINGS,
            REPRICE_SETTINGS,
            LICENSE

        }

        enum Method
        {
            GET,
            POST
        }


        private static String BASE_URL = "https://price-monitor.com/api/prm/login/";
        private static String PATH_SEPAR = "/";
        private static String QUERY_BEGIN = "?";
        private static String QUERY_SEPAR = "&";
        private String apiKey;


        /**
         * Create PriceMonitor instance.
         *
         * @param yourApiKey Api key for PriceMonitor service
         * @throws ArgumentException If null or empty Api key provided
         */
        public PriceMonitor(String yourApiKey)
        {

            if (yourApiKey == null || yourApiKey.Length == 0)
            {
                throw new ArgumentException("Bad value your Api Key");
            }

            this.apiKey = yourApiKey;
        }


        /**
         * Perform GET request to get price updates.
         *
         * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
         * @param id          product Id (optional), get an update only for the product with the specified internal product id (SKU).
         * @param format      support  Format.JSON and  Format.CSV (optional, if don't set, default Format.JSON)
         * @param pformat_dec support Pformat_dec.INTEGER and Pformat_dec.FLOAT (optional, default Pformat_dec.INTEGER)
         * @param exportAll   support ExportAll.TRUE and ExportAll.FALSE (optional, default Export.TRUE)
         * @param test        support Test.TRUE and Test.FALSE (optional, defautl Test.FALSE)
         * @return response from monitor price server
         */
        public void GetPriceUpdates(ref String response,
            String marketplace,
            String id = null,
            Format format = Format.JSON,
            Pformat_dec pformat_dec = Pformat_dec.INTEGER,
            ExportAll exportAll = ExportAll.TRUE,
            Test test = Test.FALSE)
        {

            CheckMarketplace(marketplace);

            String fullUrl = BASE_URL +
                             apiKey +
                             PATH_SEPAR +
                             Action.GET_PRICE_UPDATES.ToString().ToLower() +
                             QUERY_BEGIN +
                             GetQuery(marketplace, "marketplace") +
                             GetQuery(id, "id") +
                             GetQuery(format) +
                             GetQuery(pformat_dec) +
                             GetQuery(exportAll) +
                             GetQuery(test);


           DoRequest(ref response, Method.GET, fullUrl);
        }


        /**
    * Perform POST request to update products.
    *
    * @param marketplace  the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
    * @param productsBody string on CSV format or JSON format, with products info.
    * @param format       setting, that show, what format has productsBody. Support Format.JSON and Format.CSV
    * @param separator    support  Separator.COMMA, Separator.TAB and Separator.SEMICOLON (optional, if don't set, default Separator.COMMA)
    * @param lineend      support Lineend.WIN and Lineend.UNIX (optional)
    * @param keepold      support Keepold.TRUE and Keepold.FALSE (optional, default Keepold.FALSE)
    * @param test         support Test.TRUE and Test.FALSE (optional, defautl Test.FALSE)
    * @param cleanold     support Cleanold.TRUE and Cleanold.FALSE (optional)
    * @return response from monitor price server
    */
        public void UpdateProducts(ref String response,
            String marketplace,
            String productsBody,
            Format format = Format.JSON,
            Separator separator = Separator.COMMA,
            Lineend lineend = Lineend.UNIX,
            Keepold keepold = Keepold.FALSE,
            Test test = Test.FALSE,
            Cleanold cleanold = Cleanold.FALSE
        )
        {
            CheckMarketplace(marketplace);
            CheckPostBody(productsBody);

            String fullUrl = BASE_URL +
                             apiKey +
                             PATH_SEPAR +
                             Action.IMPORT_PRODUCTS.ToString().ToLower() +
                             QUERY_BEGIN +
                             GetQuery(marketplace, "marketplace") +
                             GetQuery(separator) +
                             GetQuery(lineend) +
                             GetQuery(keepold) +
                             GetQuery(test) +
                             GetQuery(cleanold);


            DoRequest(ref response, Method.POST, fullUrl, productsBody, format);

        }


        /**
            * Perform GET request to get product offers..
            *
            * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
            * @param format      support  Format.JSON and  Format.CSV (optional, if don't set, default Format.JSON
            * @param sortBy      support SortBy.TOTAL_PRICE, SortBy.SHIPPING_COST, SortBy.RANKING  and SortBy.PRICE (optional, default value SortBy.RANKING)
            * @param offerId     return only the offers with the given index after sorting them. If no offeridx is given, it will return all offers
            * @param productsIds A (possibly comma- separated list of) product IDs whose data is to be exported. If this parameter is not specified, all products will be exported
            * @param pformat_dec support Pformat_dec.INTEGER and Pformat_dec.FLOAT (optional, default Pformat_dec.INTEGER)
            * @param exportAll   support ExportAll.TRUE and ExportAll.FALSE (optional, default Export.TRUE)
            * @return response from monitor price server
            */

        public void GetProductOffers(ref String response,
            String marketplace,
            Format format = Format.JSON,
            SortBy sortBy = SortBy.RANKING,
            int offerId = 0,
            List<String> productsIds = null,
            Pformat_dec pformat_dec = Pformat_dec.INTEGER,
            ExportAll exportAll = ExportAll.TRUE)
        {
            CheckMarketplace(marketplace);

            String products = ConvertListToString(productsIds);

            String fullUrl = BASE_URL +
                             apiKey +
                             PATH_SEPAR +
                             Action.EXPORT.ToString().ToLower() +
                             QUERY_BEGIN +
                             GetQuery(marketplace, "marketplace") +
                             GetQuery(format) +
                             GetQuery(sortBy) +
                             (offerId > 0 ? GetQuery(offerId, "offeridx") : "") +
                             GetQuery(products, "ids") +
                             GetQuery(pformat_dec) +
                             GetQuery(exportAll);


            DoRequest(ref response, Method.GET, fullUrl);

        }

        /**
          * Perform GET request to get products with an error status.
          *
          * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
          * @param format     support  Format.JSON and  Format.CSV (optional, if don't set, default Format.JSON
          * @return response from monitor price server
          */

        public void GetProductsWithErrors(ref String response,
            String marketplace,
            Format format = Format.JSON)
        {
            CheckMarketplace(marketplace);


            String fullUrl = BASE_URL +
                             apiKey +
                             PATH_SEPAR +
                             Action.GET_ERRORS.ToString().ToLower() +
                             QUERY_BEGIN +
                             GetQuery(marketplace, "marketplace") +
                             GetQuery(format);


            DoRequest(ref response, Method.GET, fullUrl);

        }


        /**
           * Perform GET request to delete products.
           *
           * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
           * @param productsIds A  list of product IDs whose data is to be deleted.
           * @return response from monitor price server
           */

        public void DeleteProducts(ref String response,
            String marketplace,
            List<String> productsIds)
        {
            CheckMarketplace(marketplace);
            String products = ConvertListToString(productsIds);


            String fullUrl = BASE_URL +
                             apiKey +
                             PATH_SEPAR +
                             Action.DELETE_PRODUCTS.ToString().ToLower() +
                             QUERY_BEGIN +
                             GetQuery(marketplace, "marketplace") +
                             GetQuery(products, "ids");


            DoRequest(ref response, Method.GET, fullUrl);

        }



        /**
  * Perform GET request to set marketplace settings.
  *
  * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
  * @param url         the marketplace-specific URL of your shop used for identification. On Amazon and ebay, for example, this should be set to your shop URL on the respective marketplace.
  * @param repricing   support Repricing.ON and Repricing.OFF (optional, default value Repricing.ON)
  * @param ean         support EAN.ON and EAN.OFF (optional)
  * @return response from monitor price server
  */

        public void SetMarketplaceSettings(ref String response,
            String marketplace,
            String url,
            Repricing repricing = Repricing.ON,
            EAN ean = EAN.OFF)
        {
            CheckMarketplace(marketplace);


            String fullUrl = BASE_URL +
                             apiKey +
                             PATH_SEPAR +
                             Action.MARKETPLACE_SETTINGS.ToString().ToLower() +
                             QUERY_BEGIN +
                             GetQuery(marketplace, "marketplace") +
                             GetQuery(url, "url") +
                             GetQuery(repricing) +
                             GetQuery(ean);


            DoRequest(ref response, Method.GET, fullUrl);

        }


        /**
    * Perform POST request to set reprice settings.
    *
    * @param marketplace  the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
    * @param settingsBody string on CSV format or JSON format, with settings.
    * @param format       setting, that show, what format has settingsBody. Support Format.JSON and Format.CSV
    * @param separator    support  Separator.COMMA, Separator.TAB and Separator.SEMICOLON (optional, if don't set, default Separator.COMMA)
    * @param lineend      support Lineend.WIN and Lineend.UNIX (optional)
    * @return response from monitor price server
    */

        public void SetRepriceSettings(ref String response,
            String marketplace,
            String settingsBody,
            Format format = Format.JSON,
            Separator separator = Separator.COMMA,
            Lineend lineend = Lineend.UNIX)
        {
            CheckMarketplace(marketplace);
            CheckPostBody(settingsBody);

            String fullUrl = BASE_URL +
                             apiKey +
                             PATH_SEPAR +
                             Action.REPRICE_SETTINGS.ToString().ToLower() +
                             QUERY_BEGIN +
                             GetQuery(marketplace, "marketplace") +
                             GetQuery(separator) +
                             GetQuery(lineend);


            DoRequest(ref response, Method.POST, fullUrl, settingsBody, format);

        }

        /**
          * Perform GET request to get reprice settings.
          *
          * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
          * @param productsIds a  list of of product IDs whose settings are to be exported.
          * @param format      support  Format.JSON and  Format.CSV (optional, if don't set, default Format.JSON
          * @param pformat_dec support Pformat_dec.INTEGER and Pformat_dec.FLOAT (optional, default Pformat_dec.INTEGER)
          * @return response from monitor price server
          */

        public void GetRepriceSettings(ref String response,
            String marketplace,
            List<String> productsIds,
            Format format = Format.JSON,
            Pformat_dec pformat_dec = Pformat_dec.INTEGER)
        {

            CheckMarketplace(marketplace);
            String products = ConvertListToString(productsIds);

            String fullUrl = BASE_URL +
                    apiKey +
                    PATH_SEPAR +
                    Action.REPRICE_SETTINGS.ToString().ToLower() +
                    QUERY_BEGIN +
                    GetQuery(marketplace, "marketplace") +
                    GetQuery(products, "ids") +
                    GetQuery(format) +
                    GetQuery(pformat_dec);


            DoRequest(ref response, Method.GET, fullUrl);

        }


        /**
         * Perform GET to get license info.
         *
         * @return response from monitor price server
         */
        public void GetLicense(ref String response)
        {

            String fullUrl = BASE_URL +
                    apiKey +
                    PATH_SEPAR +
                    Action.LICENSE.ToString().ToLower();


            DoRequest(ref response, Method.GET, fullUrl);

        }



        private void CheckMarketplace(String marketplace)
        {
            if (!ListMarketplaces.IsValidMarketplace(marketplace))
            {
                throw new ArgumentException("Not valid marketplace value : " + marketplace);
            }
        }

        private void CheckPostBody(String postBody)
        {
            if (postBody == null || postBody.Length == 0)
            {
                throw new ArgumentException("Not valid post body text");
            }
        }

        private String ConvertListToString(List<String> entries)
        {
            String result = null;
            if (entries != null && entries.Count != 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (String product in entries)
                {
                    builder.Append(product).Append(",");
                }
                builder.Length = builder.Length - 1;
                result = builder.ToString();
            }

            return result;
        }


        private static String GetQuery(Object query, String key = null)
        {
            if (query == null)
            {
                return "";
            }

            if (key != null)
            {
                return (!key.Equals("marketplace") ? QUERY_SEPAR : "") + key + "=" + System.Web.HttpUtility.UrlEncode(query.ToString(), Encoding.UTF8);
            }

            return QUERY_SEPAR + query.GetType().Name.ToLower() + "=" + (query is Pformat_dec ? ((int)query).ToString() : query.ToString().ToLower());

        }


        private void DoRequest(ref String responseServer, Method method, String urlStr, String postBody = null, Format format = Format.JSON)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlStr);        

            request.Method = method.ToString();

            if (method == Method.POST)
            {

                using (var stream = request.GetRequestStream())
                {
                    request.ContentType = format == Format.JSON ? "application/json" : "text/csv";
                    byte[] postBodyBytes = Encoding.UTF8.GetBytes(postBody);
                    stream.Write(postBodyBytes, 0, postBodyBytes.Length);
                    stream.Close();
                }
            }


            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

            }
            catch (WebException e)
            {
                response = e.Response as HttpWebResponse;
                if (response == null)
                {
                    throw e;
                }
            }

            Stream streamResponse = response.GetResponseStream();
            responseServer = new StreamReader(streamResponse).ReadToEnd();
            streamResponse.Close();


        }


    }
}
