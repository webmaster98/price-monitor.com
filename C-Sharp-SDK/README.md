
## Price Monitor C# SDK
This is the C# sdk for the price monitor api

### Installation
Include PriceMonitor.cs and ListMarketplaces.cs  in your project.


### Usage
Pass your api key to the price monitor object and you are ready to go!

```
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

  
	Available Methods
	GetPriceUpdates()
	UpdateProducts()
	GetProductOffers()
	GetProductsWithErrors()
	DeleteProducts()
	SetMarketplaceSettings()
	SetRepriceSettings()
	GetRepriceSettings()
	GetLicense()
	These methods return the response from the api as a string (either json or csv depending on format selected - the default is json).
```
