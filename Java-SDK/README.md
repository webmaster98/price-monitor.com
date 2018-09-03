
	Price Monitor JAVA SDK
	This is the JAVA sdk for the price monitor api

	Installation
	Include PriceMonitor.java, ListMarketplaces.java and CertificateUtils.java  in your project.


	Usage
	Pass your api key to the price monitor object and you are ready to go!

    String apiKey = "your api key here";
    PriceMonitor monitor = new PriceMonitor(apiKey);
	String responseGetPriceUpdates = monitor.getPriceUpdates("idealo.de", "your id here", Format.JSON, Pformat_dec.FLOAT, ExportAll.TRUE, Test.FALSE);
    System.out.println(responseGetPriceUpdates);
    String responseUpdateProducts = monitor.updateProducts("idealo.de", "your CSV products list here", Separator.COMMA, Lineend.WIN, Keepold.TRUE, Test.FALSE, Cleanold.TRUE);
    System.out.println(responseUpdateProducts);

  
	Available Methods
	getPriceUpdates()
	updateProducts()
	getProductOffers()
	getProductsWithErrors()
	deleteProducts()
	setMarketplaceSettings()
	setRepriceSettings()
	getRepriceSettings()
	getLicense()
	These methods return the response from the api as a string (either json or csv depending on format selected - the default is json).
