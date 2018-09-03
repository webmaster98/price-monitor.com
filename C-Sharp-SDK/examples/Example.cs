using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonitorSDK.PriceMonitor;

namespace MonitorSDK
{
    class Example
    {

        static String productsCSV_Example = "id,ean,id_on_marketplace,category,mpn,manufacturer,model,name_on_marketplace,min_price,max_price\n" +
                                            "0000001,7611382551115,,Armbanduhr,XS.3051,Luminox,TestName1,,1000,69800\n" +
                                            "0000002,7611382551122,,Armbanduhr,XS.3059,Luminox,TestName2,,1000,69800";

        static String repriceSettingsCSV_Example = "id;min_price;max_price;strategy\n" +
                                                   "0000001;1100;1999;aggressive\n" +
                                                   "0000002;1000;2000;aggressive";

        static String productsJson_Example = "[{\"id\":\"0000003\",\"ean\":7611382551116,\"id_on_marketplace\":\"\",\"category\":\"Armbanduhr\",\"mpn\":\"XS.3051\",\"manufacturer\":\"Luminox\",\"model\":\"TestName1\",\"name_on_marketplace\":\"\",\"min_price\":1000,\"max_price\":69800},{\"id\":\"0000004\",\"ean\":7611382551123,\"id_on_marketplace\":\"\",\"category\":\"Armbanduhr\",\"mpn\":\"XS.3059\",\"manufacturer\":\"Luminox\",\"model\":\"TestName2\",\"name_on_marketplace\":\"\",\"min_price\":1000,\"max_price\":69800}]";

        static String repriceSettingsJson_Example = "[{\"id\":\"0000003\",\"min_price\":1100,\"max_price\":1999,\"strategy\":\"aggressive\"},{\"id\":\"0000004\",\"min_price\":1000,\"max_price\":2000,\"strategy\":\"aggressive\"}]";


        static void Main(string[] args)
        {

            try
            {
                String yourApiKey = "your_Api_key_here";
                PriceMonitor monitor = new PriceMonitor(yourApiKey);

                String marketplace = "google.de";
                String responseFromServer = null;
                monitor.UpdateProducts(ref responseFromServer, marketplace, productsJson_Example, Format.JSON);
                Console.WriteLine(responseFromServer);

                monitor.UpdateProducts(ref responseFromServer, marketplace, productsCSV_Example, Format.CSV, Separator.COMMA, Lineend.UNIX, Keepold.TRUE, Test.FALSE, Cleanold.FALSE);
                Console.WriteLine(responseFromServer);

                monitor.GetPriceUpdates(ref responseFromServer, marketplace, null, Format.CSV, Pformat_dec.FLOAT, ExportAll.TRUE, Test.FALSE);
                Console.WriteLine(responseFromServer);

                monitor.GetProductOffers(ref responseFromServer, marketplace, Format.JSON, SortBy.PRICE, 0, null, Pformat_dec.FLOAT, ExportAll.TRUE);
                Console.WriteLine(responseFromServer);


                monitor.GetProductsWithErrors(ref responseFromServer, marketplace, Format.CSV);
                Console.WriteLine(responseFromServer);

                String urlRepricing = "<SHOP-URL>";
                monitor.SetMarketplaceSettings(ref responseFromServer, marketplace, urlRepricing, Repricing.OFF, EAN.ON);
                Console.WriteLine(responseFromServer);

                monitor.SetRepriceSettings(ref responseFromServer, marketplace, repriceSettingsCSV_Example, Format.CSV, Separator.SEMICOLON, Lineend.UNIX);
                Console.WriteLine(responseFromServer);

                monitor.SetRepriceSettings(ref responseFromServer, marketplace, repriceSettingsJson_Example, Format.JSON);
                Console.WriteLine(responseFromServer);

                List<String> productsListRepricing = new List<String>();
                productsListRepricing.Add("0000001");
                productsListRepricing.Add("0000002");
                monitor.GetRepriceSettings(ref responseFromServer, marketplace, productsListRepricing);
                Console.WriteLine(responseFromServer);


                List<String> productsListDelete = new List<String>();
                productsListDelete.Add("0000001");
                productsListDelete.Add("0000002");
                monitor.DeleteProducts(ref responseFromServer, marketplace, productsListDelete);
                Console.WriteLine(responseFromServer);

                 monitor.GetLicense(ref responseFromServer);
                Console.WriteLine(responseFromServer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
