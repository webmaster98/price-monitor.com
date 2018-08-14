<?php

require_once __DIR__ . '/src/PriceMonitor.php';
require_once __DIR__ . '/src/PriceMonitorException.php';

$apiKey = "API_KEY";

$priceMonitor = new PriceMonitor\PriceMonitor($apiKey);

var_dump($priceMonitor->getPriceUpdates("google.de"));
var_dump($priceMonitor->getPriceUpdates("google.de", "csv"));

