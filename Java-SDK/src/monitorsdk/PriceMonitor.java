package monitorsdk;

import com.sun.istack.internal.NotNull;
import com.sun.istack.internal.Nullable;

import javax.net.ssl.HttpsURLConnection;
import java.io.*;
import java.net.URL;
import java.net.URLEncoder;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.util.List;


public class PriceMonitor
{

    public enum Pformat_dec
    {
        INTEGER(1),
        FLOAT(2);

        int value;

        Pformat_dec(int ident)
        {
            value = ident;
        }

        @Override
        public String toString()
        {
            return String.valueOf(value);
        }
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
        LICENSE;

        @Override
        public String toString()
        {
            return super.toString().toLowerCase();
        }


    }

    enum Method
    {
        GET,
        POST
    }

    private static final String BASE_URL = "https://price-monitor.com/api/prm/login/";
    private static final String PATH_SEPAR = "/";
    private static final String QUERY_BEGIN = "?";
    private static final String QUERY_SEPAR = "&";
    private final String apiKey;


    /**
     * Create PriceMonitor instance.
     *
     * @param yourApiKey Api key for PriceMonitor service
     * @throws IllegalArgumentException If bad Api key provided
     */
    public PriceMonitor(String yourApiKey)
    {

        if (yourApiKey == null)
        {
            throw new IllegalArgumentException("Bad value your Api Key");
        }

        this.apiKey = yourApiKey;
    }


    /**
     * Perform GET request to get price updates.
     *
     * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
     * @param id          product Id (optional), get an update only for the product with the specified internal product id (SKU).
     * @param format      {@link Format}, support  Format.JSON and  Format.CSV (optional, if don't set, default Format.JSON)
     * @param pformat_dec {@link Pformat_dec}, support Pformat_dec.INTEGER and Pformat_dec.FLOAT (optional, default Pformat_dec.INTEGER)
     * @param exportAll   {@link ExportAll}, support ExportAll.TRUE and ExportAll.FALSE (optional, default Export.TRUE)
     * @param test        {@link Test}, support Test.TRUE and Test.FALSE (optional, defautl Test.FALSE)
     * @return response from monitor price server
     */
    public String getPriceUpdates(
            @NotNull String marketplace,
            @Nullable String id,
            @Nullable Format format,
            @Nullable Pformat_dec pformat_dec,
            @Nullable ExportAll exportAll,
            @Nullable Test test) throws IOException, NoSuchAlgorithmException, KeyManagementException
    {

        checkMarketplace(marketplace);

        String fullUrl = BASE_URL +
                apiKey +
                PATH_SEPAR +
                Action.GET_PRICE_UPDATES +
                QUERY_BEGIN +
                getQuery(marketplace, "marketplace") +
                getQuery(id, "id") +
                getQuery(format) +
                getQuery(pformat_dec) +
                getQuery(exportAll) +
                getQuery(test);


        return doRequest(Method.GET, fullUrl, null, null);
    }


    /**
     * Perform POST request to update products.
     *
     * @param marketplace  the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
     * @param productsBody string on CSV format or JSON format, with products info.
     * @param format       {@link Format} setting, that show, what format has productsBody. Support Format.JSON and Format.CSV
     * @param separator    {@link Separator}, support  Separator.COMMA, Separator.TAB and Separator.SEMICOLON (optional, if don't set, default Separator.COMMA)
     * @param lineend      {@link Lineend }, support Lineend.WIN and Lineend.UNIX (optional)
     * @param keepold      {@link Keepold}, support Keepold.TRUE and Keepold.FALSE (optional, default Keepold.FALSE)
     * @param test         {@link Test}, support Test.TRUE and Test.FALSE (optional, defautl Test.FALSE)
     * @param cleanold     {@link Cleanold}, support Cleanold.TRUE and Cleanold.FALSE (optional)
     * @return response from monitor price server
     */
    public String updateProducts(
            @NotNull String marketplace,
            @NotNull String productsBody,
            @NotNull Format format,
            @Nullable Separator separator,
            @Nullable Lineend lineend,
            @Nullable Keepold keepold,
            @Nullable Test test,
            @Nullable Cleanold cleanold) throws IOException, NoSuchAlgorithmException, KeyManagementException
    {
        checkMarketplace(marketplace);
        checkPostBody(productsBody);

        String fullUrl = BASE_URL +
                apiKey +
                PATH_SEPAR +
                Action.IMPORT_PRODUCTS +
                QUERY_BEGIN +
                getQuery(marketplace, "marketplace") +
                getQuery(separator) +
                getQuery(lineend) +
                getQuery(keepold) +
                getQuery(test) +
                getQuery(cleanold);


        return doRequest(Method.POST, fullUrl, productsBody, format);

    }


    /**
     * Perform GET request to get product offers..
     *
     * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
     * @param format      {@link Format}, support  Format.JSON and  Format.CSV (optional, if don't set, default Format.JSON
     * @param sortBy      {@link SortBy }, support SortBy.TOTAL_PRICE, SortBy.SHIPPING_COST, SortBy.RANKING  and SortBy.PRICE (optional, default value SortBy.RANKING)
     * @param offerId     {@link Keepold}, return only the offers with the given index after sorting them. If no offeridx is given, it will return all offers
     * @param productsIds {@link Test},  A (possibly comma- separated list of) product IDs whose data is to be exported. If this parameter is not specified, all products will be exported
     * @param pformat_dec {@link Pformat_dec}, support Pformat_dec.INTEGER and Pformat_dec.FLOAT (optional, default Pformat_dec.INTEGER)
     * @param exportAll   {@link ExportAll}, support ExportAll.TRUE and ExportAll.FALSE (optional, default Export.TRUE)
     * @return response from monitor price server
     */

    public String getProductOffers(
            @NotNull String marketplace,
            @Nullable Format format,
            @Nullable SortBy sortBy,
            @Nullable Integer offerId,
            @Nullable List<String> productsIds,
            @Nullable Pformat_dec pformat_dec,
            @Nullable ExportAll exportAll) throws IOException, NoSuchAlgorithmException, KeyManagementException
    {
        checkMarketplace(marketplace);

        String products = convertListToString(productsIds);

        String fullUrl = BASE_URL +
                apiKey +
                PATH_SEPAR +
                Action.EXPORT +
                QUERY_BEGIN +
                getQuery(marketplace, "marketplace") +
                getQuery(format) +
                getQuery(sortBy) +
                getQuery(offerId, "offeridx") +
                getQuery(products, "ids") +
                getQuery(pformat_dec) +
                getQuery(exportAll);


        return doRequest(Method.GET, fullUrl, null, null);

    }


    /**
     * Perform GET request to get products with an error status.
     *
     * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
     * @param format      {@link Format}, support  Format.JSON and  Format.CSV (optional, if don't set, default Format.JSON
     * @return response from monitor price server
     */

    public String getProductsWithErrors(
            @NotNull String marketplace,
            @Nullable Format format) throws IOException, NoSuchAlgorithmException, KeyManagementException
    {
        checkMarketplace(marketplace);


        String fullUrl = BASE_URL +
                apiKey +
                PATH_SEPAR +
                Action.GET_ERRORS +
                QUERY_BEGIN +
                getQuery(marketplace, "marketplace") +
                getQuery(format);


        return doRequest(Method.GET, fullUrl, null, null);

    }


    /**
     * Perform GET request to delete products.
     *
     * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
     * @param productsIds A  list of product IDs whose data is to be deleted.
     * @return response from monitor price server
     */

    public String deleteProducts(
            @NotNull String marketplace,
            @NotNull List<String> productsIds) throws IOException, NoSuchAlgorithmException, KeyManagementException
    {
        checkMarketplace(marketplace);
        String products = convertListToString(productsIds);


        String fullUrl = BASE_URL +
                apiKey +
                PATH_SEPAR +
                Action.DELETE_PRODUCTS +
                QUERY_BEGIN +
                getQuery(marketplace, "marketplace") +
                getQuery(products, "ids");


        return doRequest(Method.GET, fullUrl, null, null);

    }


     /**
     * Perform GET request to set marketplace settings.
     *
     * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
     * @param url         the marketplace-specific URL of your shop used for identification. On Amazon and ebay, for example, this should be set to your shop URL on the respective marketplace.
     * @param repricing   {@link Repricing }, support Repricing.ON and Repricing.OFF (optional, default value Repricing.ON)
     * @param ean         {@link EAN}, support EAN.ON and EAN.OFF (optional)
     * @return response from monitor price server
     */

    public String setMarketplaceSettings(
            @NotNull String marketplace,
            @Nullable String url,
            @Nullable Repricing repricing,
            @Nullable EAN ean) throws IOException, NoSuchAlgorithmException, KeyManagementException
    {
        checkMarketplace(marketplace);


        String fullUrl = BASE_URL +
                apiKey +
                PATH_SEPAR +
                Action.MARKETPLACE_SETTINGS +
                QUERY_BEGIN +
                getQuery(marketplace, "marketplace") +
                getQuery(url, "url") +
                getQuery(repricing) +
                getQuery(ean);


        return doRequest(Method.GET, fullUrl, null, null);

    }

    /**
     * Perform POST request to set reprice settings.
     *
     * @param marketplace  the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
     * @param settingsBody string on CSV format or JSON format, with settings.
     * @param format       {@link Format} setting, that show, what format has settingsBody. Support Format.JSON and Format.CSV
     * @param separator    {@link Separator}, support  Separator.COMMA, Separator.TAB and Separator.SEMICOLON (optional, if don't set, default Separator.COMMA)
     * @param lineend      {@link Lineend }, support Lineend.WIN and Lineend.UNIX (optional)
     * @return response from monitor price server
     */

    public String setRepriceSettings(
            @NotNull String marketplace,
            @NotNull String settingsBody,
            @NotNull Format format,
            @Nullable Separator separator,
            @Nullable Lineend lineend) throws IOException, NoSuchAlgorithmException, KeyManagementException
    {
        checkMarketplace(marketplace);
        checkPostBody(settingsBody);

        String fullUrl = BASE_URL +
                apiKey +
                PATH_SEPAR +
                Action.REPRICE_SETTINGS +
                QUERY_BEGIN +
                getQuery(marketplace, "marketplace") +
                getQuery(separator) +
                getQuery(lineend);


        return doRequest(Method.POST, fullUrl, settingsBody, format);

    }

    /**
     * Perform GET request to get reprice settings.
     *
     * @param marketplace the marketplace from which the products shall be exported Please see {@link ListMarketplaces } for choose available marketplace.
     * @param productsIds a  list of of product IDs whose settings are to be exported.
     * @param format      {@link Format}, support  Format.JSON and  Format.CSV (optional, if don't set, default Format.JSON
     * @param pformat_dec {@link Pformat_dec}, support Pformat_dec.INTEGER and Pformat_dec.FLOAT (optional, default Pformat_dec.INTEGER)
     * @return response from monitor price server
     */

    public String getRepriceSettings(
            @NotNull String marketplace,
            @NotNull List<String> productsIds,
            @Nullable Format format,
            @Nullable Pformat_dec pformat_dec) throws IOException, NoSuchAlgorithmException, KeyManagementException
    {
        checkMarketplace(marketplace);
        String products = convertListToString(productsIds);

        String fullUrl = BASE_URL +
                apiKey +
                PATH_SEPAR +
                Action.REPRICE_SETTINGS +
                QUERY_BEGIN +
                getQuery(marketplace, "marketplace") +
                getQuery(products, "ids") +
                getQuery(format) +
                getQuery(pformat_dec);


        return doRequest(Method.GET, fullUrl, null, null);

    }


    /**
     * Perform GET to get license info.
     *
     * @return response from monitor price server
     */
    public String getLicense() throws IOException, NoSuchAlgorithmException, KeyManagementException
    {

        String fullUrl = BASE_URL +
                apiKey +
                PATH_SEPAR +
                Action.LICENSE;


        return doRequest(Method.GET, fullUrl, null, null);

    }


    private void checkMarketplace(String marketplace)
    {
        if (!ListMarketplaces.isValidMarketplace(marketplace))
        {
            throw new IllegalArgumentException("Not valid marketplace value : " + marketplace);
        }
    }

    private void checkPostBody(String postBody)
    {
        if (postBody == null || postBody.length() == 0)
        {
            throw new IllegalArgumentException("Not valid post body text");
        }
    }

    private String convertListToString(List<String> entries)
    {
        String result = null;
        if (entries != null && !entries.isEmpty())
        {
            StringBuilder builder = new StringBuilder();
            for (String product : entries)
            {
                builder.append(product).append(",");
            }
            builder.setLength(builder.length() - 1);
            result = builder.toString();
        }

        return result;
    }

    private static String getQuery(Object query) throws UnsupportedEncodingException
    {
        return getQuery(query, null);
    }

    private static String getQuery(Object query, String key) throws UnsupportedEncodingException
    {
        if (query == null)
        {
            return "";
        }

        if (key != null)
        {
            return (!key.equals("marketplace") ? QUERY_SEPAR : "") + key + "=" + URLEncoder.encode(query.toString(), "UTF-8");
        }

        return QUERY_SEPAR + query.getClass().getSimpleName().toLowerCase() + "=" + query.toString().toLowerCase();

    }


    private String doRequest(Method method, String urlStr, String postBody, Format format) throws IOException, KeyManagementException, NoSuchAlgorithmException
    {
        URL url = new URL(urlStr);

        HttpsURLConnection connect = (HttpsURLConnection) url.openConnection();
        connect.setHostnameVerifier(CertificateUtils.makeHostnameVerifier());
        connect.setSSLSocketFactory(CertificateUtils.makeSSLSocketFactoryTrustAll());

        connect.setRequestMethod(method.toString());

        connect.setConnectTimeout(60000);
        connect.setReadTimeout(60000);


        if (method == Method.POST)
        {
            connect.setDoOutput(true);
            connect.setRequestProperty("Content-type", format == Format.CSV ? "text/csv" : "application/json");
            BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(connect.getOutputStream()));
            writer.write(postBody);
            writer.close();

        }

        BufferedReader reader;
        if (connect.getResponseCode() == 200)
        {
            reader = new BufferedReader(new InputStreamReader(connect.getInputStream()));
        }
        else
        {
            reader = new BufferedReader(new InputStreamReader(connect.getErrorStream()));
        }

        String inputLine;
        StringBuilder response = new StringBuilder();

        while ((inputLine = reader.readLine()) != null)
        {
            response.append(inputLine).append("\n");
        }
        reader.close();

        response.setLength(response.length()-1);
        return response.toString();
    }


}
