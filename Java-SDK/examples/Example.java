package examples;



import monitorsdk.PriceMonitor;
import monitorsdk.PriceMonitor.*;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;


public class Example
{

    static String productsCSV_Example = "id,ean,id_on_marketplace,category,mpn,manufacturer,model,name_on_marketplace,min_price,max_price\n" +
                                        "0000001,7611382551115,,Armbanduhr,XS.3051,Luminox,TestName1,,1000,69800\n" +
                                        "0000002,7611382551122,,Armbanduhr,XS.3059,Luminox,TestName2,,1000,69800";

    static String repriceSettingsCSV_Example = "id;min_price;max_price;strategy\n" +
                                               "0000001;1100;1999;aggressive\n" +
                                               "0000002;1000;2000;aggressive";

    static String productsJson_Example = "[{\"id\":\"0000003\",\"ean\":7611382551116,\"id_on_marketplace\":\"\",\"category\":\"Armbanduhr\",\"mpn\":\"XS.3051\",\"manufacturer\":\"Luminox\",\"model\":\"TestName1\",\"name_on_marketplace\":\"\",\"min_price\":1000,\"max_price\":69800},{\"id\":\"0000004\",\"ean\":7611382551123,\"id_on_marketplace\":\"\",\"category\":\"Armbanduhr\",\"mpn\":\"XS.3059\",\"manufacturer\":\"Luminox\",\"model\":\"TestName2\",\"name_on_marketplace\":\"\",\"min_price\":1000,\"max_price\":69800}]";

    static String repriceSettingsJson_Example = "[{\"id\":\"0000003\",\"min_price\":1100,\"max_price\":1999,\"strategy\":\"aggressive\"},{\"id\":\"0000004\",\"min_price\":1000,\"max_price\":2000,\"strategy\":\"aggressive\"}]";

    public static void main(String[] args)
    {
        String yourApiKey = "your_Api_key_here";
        PriceMonitor monitor = new PriceMonitor(yourApiKey);

        try
        {
            String marketplace = "google.de";

            String responseUpdateProducts = monitor.updateProducts(marketplace, productsCSV_Example, Format.CSV,  Separator.COMMA, Lineend.UNIX, Keepold.TRUE, Test.FALSE, Cleanold.TRUE);
            System.out.println(responseUpdateProducts);

            responseUpdateProducts = monitor.updateProducts(marketplace, productsJson_Example, Format.JSON,  null, null, Keepold.TRUE, Test.FALSE, Cleanold.FALSE);
            System.out.println(responseUpdateProducts);

            String responseGetPriceUpdates = monitor.getPriceUpdates(marketplace, null, Format.CSV, Pformat_dec.FLOAT, ExportAll.TRUE, Test.FALSE);
            System.out.println(responseGetPriceUpdates);

            String responseExportProductOffer = monitor.getProductOffers(marketplace, Format.JSON, null, null, null, Pformat_dec.FLOAT, ExportAll.TRUE);
            System.out.println(responseExportProductOffer);


            String responseExportProductsWithErrors = monitor.getProductsWithErrors(marketplace, Format.CSV);
            System.out.println(responseExportProductsWithErrors);

            String urlRepricing = "<SHOP-URL>";
            String resposneSetMarketSettings = monitor.setMarketplaceSettings(marketplace, urlRepricing, Repricing.OFF, EAN.ON);
            System.out.println(resposneSetMarketSettings);

            String responseSetRepriceSettings = monitor.setRepriceSettings(marketplace, repriceSettingsCSV_Example, Format.CSV, Separator.SEMICOLON, Lineend.UNIX);
            System.out.println(responseSetRepriceSettings);

            responseSetRepriceSettings = monitor.setRepriceSettings(marketplace, repriceSettingsJson_Example, Format.JSON, null, null);
            System.out.println(responseSetRepriceSettings);

            List<String> productsListRepricing = new ArrayList<>(Arrays.asList("0000001", "0000002"));
            String responseRetrieveRepriceSettings = monitor.getRepriceSettings(marketplace, productsListRepricing, null, null);
            System.out.println(responseRetrieveRepriceSettings);


            List<String> productsListDelete = new ArrayList<>(Arrays.asList("0000001", "0000002"));
            String responseDeleteProducts = monitor.deleteProducts(marketplace, productsListDelete);
            System.out.println(responseDeleteProducts);

            String responseLicense = monitor.getLicense();
            System.out.println(responseLicense);

        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
    }
}
