# Price Monitor PHP SDK

This is the PHP sdk for the price monitor api

## Installation

Include ``PriceMonitor.php`` in your application and require it at the top of your file.

```
require_once('PriceMonitor.php');
```

## Usage

Pass your api key to the price monitor object and you are ready to go!

```
$apiKey = "";

$priceMonitor = new PriceMonitor\PriceMonitor($apiKey);
$priceMonitor->getLicense();
```

## Available Methods

1. getPriceUpdates
1. updateProducts
1. getProductOffers
1. getProductsWithErrors
1. deleteProducts
1. setMarketplaceSettings
1. setRepriceSettings
1. getRepriceSettings
1. getLicense