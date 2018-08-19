<?php

require_once('../src/PriceMonitor.php');

$apiKey = "API_KEY";

$priceMonitor = new WebMaster\PriceMonitor\PriceMonitor($apiKey);

echo $priceMonitor->getPriceUpdates("google.de");
echo $priceMonitor->getProductOffers("google.de", "csv");
echo $priceMonitor->getProductsWithErrors("google.de", "json");
echo $priceMonitor->getLicense();

echo $priceMonitor->setMarketplaceSettings("google.de", "test.com", "on", "on");
echo $priceMonitor->setRepriceSettings("reprice-settings.csv", "google.de");
echo $priceMonitor->getRepriceSettings("google.de");

echo $priceMonitor->updateProducts("products.csv", "google.de");
echo $priceMonitor->deleteProducts("google.de", ['ID#1', 'ID#2', 'ID#3']);