<?php

namespace WebMaster\PriceMonitor;

use PriceMonitorException;

class PriceMonitor {

    /**
     * @const string
     */
    const BASE_URL = 'https://price-monitor.com/api/prm/login/';

    /**
     * @var string
     */
    private $apiKey;

    /**
     * @param string $apiKey
     * 
     * @throws PriceMonitorException When cURL isn't available
     */
    public function __construct(string $apiKey)
    {
        $this->apiKey = $apiKey;

        // check if curl is enabled
        if(!function_exists("curl_init")) {
            throw new PriceMonitorException('PriceMonitor requires the use of cURL extension. Please install and enable the extension');
        }
    }

    /**
     * Perform GET request to get price updates.
	 * @param  string  $marketplace
     * @param  string  $format (optional) - json or csv
     * @param  bool    $exportAll (optional) - get all products or products with a price update
     * @param  int     $productId (optional)
     * @param  bool    $test (optional) - get random prices
	 * @param  int     $priceFormat (optional) - integer (1) or decimal (2)
     * 
     * @return string
     */
    public function getPriceUpdates(
        string $marketplace,
        string $format = null,
        bool $exportAll = true,
        int $productId = null,
        bool $test = false,
        int $priceFormat = 1
    ) {
        return $this->get("get_price_updates", [
            'marketplace' => $marketplace,
            'format' => $format,
            'exportall' => $this->booleanToString($exportAll),
            'id' => $productId,
            'test' => $this->booleanToString($test),
            'pformat_dec' => $priceFormat
        ]);
    }

    /**
     * Perform POST request to update products.
     * @param  string  $productsFile - csv filename containing product list
	 * @param  string  $marketplace
     * @param  string  $separator (optional) - comma, semicolon or tab
     * @param  string  $lineEnd (optional) - windows \r\n (win) or unix \n (unix)
     * @param  bool    $keepOld (optional) - deletes old products from database
	 * @param  bool    $test (optional) - data won't be saved to database
     * @param  bool    $cleanOld (optional) - deletes products not included in csv file from database
     * 
     * @return string
     */
    public function updateProducts(
        string $productsFile,
        string $marketplace,
        string $separator = null,
        string $lineEnd = null,
        bool $keepOld = false,
        bool $test = false,
        bool $cleanOld = false
    ) {
        return $this->postFile("import_products", $productsFile, [
            'marketplace' => $marketplace,
            'separator' => $separator,
            'lineend' => $lineEnd,
            'keepold' => $this->booleanToString($keepOld),
            'test' => $this->booleanToString($test),
            'cleanold' => $this->booleanToString($cleanOld),
        ]);
    }

    /**
     * Perform GET request to get product offers.
	 * @param  string  $marketplace
     * @param  string  $format (optional) - json or csv
     * @param  bool    $exportAll (optional) - get all products or products with a price update
     * @param  string  $sortBy (optional) - total_price or price or shipping_costs or ranking
	 * @param  int     $offerId (optional)
     * @param  array   $productIds (optional)
     * @param  int     $priceFormat (optional) - integer (1) or decimal (2)
     * 
     * @return string
     */
    public function getProductOffers(
        string $marketplace,
        string $format = null,
        bool $exportAll = true,
        string $sortBy = null,
        int $offerId = null,
        array $productIds = null,
        int $priceFormat = 1
    ) {
        if($productIds != null) {
            $productIds = $this->arrayToCommaSeparated($productIds);
        }

        return $this->get("export", [
            'marketplace' => $marketplace,
            'format' => $format,
            'exportall' => $this->booleanToString($exportAll),
            'sortby' => $sortBy,
            'offeridx' => $offerId,
            'ids' => $productIds,
            'pformat_dec' => $priceFormat
        ]);
    }

    /**
     * Perform GET request to get products with an error status.
	 * @param  string  $marketplace
     * @param  string  $format (optional) - json or csv
     * 
     * @return string
     */
    public function getProductsWithErrors(
        string $marketplace,
        string $format = null
    ) {
        return $this->get("get_errors", [
            'marketplace' => $marketplace,
            'format' => $format,
        ]);
    }

    /**
     * Perform GET request to delete products.
	 * @param  string $marketplace
     * @param  array  $productIds
     * 
     * @return string
     */
    public function deleteProducts(
        string $marketplace,
        array $productIds
    ) {
        return $this->get("delete_products", [
            'marketplace' => $marketplace,
            'ids' => $this->arrayToCommaSeparated($productIds),
        ]);
    }

    /**
     * Perform GET request to set marketplace settings.
	 * @param  string  $marketplace
     * @param  string  $url
     * @param  bool    $repricing - on or off
     * @param  bool    $ean - on or off
     * 
     * @return string
     */
    public function setMarketplaceSettings(
        string $marketplace,
        string $url,
        string $repricing,
        string $ean
    ) {
        return $this->get("marketplace_settings", [
            'marketplace' => $marketplace,
            'url' => $url,
            'repricing' => $repricing,
            'ean' => $ean,
        ]);
    }

    /**
     * Perform POST request to set reprice settings.
     * @param  string  $settingsFile - csv filename containing reprice settings
	 * @param  string  $marketplace
     * @param  string  $separator (optional) - comma, semicolon or tab
     * @param  string  $lineEnd (optional) - windows \r\n (win) or unix \n (unix)
     * 
     * @return string
     */
    public function setRepriceSettings(
        string $settingsFile,
        string $marketplace,
        string $separator = null,
        string $lineEnd = null
    ) {
        return $this->postFile("reprice_settings", $settingsFile, [
            'marketplace' => $marketplace,
            'separator' => $separator,
            'lineend' => $lineEnd,
        ]);
    }

    /**
     * Perform GET request to get reprice settings.
	 * @param  string  $marketplace
     * @param  string  $format (optional) - json or csv
     * @param  array   $productIds
	 * @param  int     $priceFormat (optional) - integer (1) or decimal (2)
     * 
     * @return string
     */
    public function getRepriceSettings(
        string $marketplace,
        string $format = null,
        array $productIds = null,
        int $priceFormat = 1
    ) {
        if($productIds != null) {
            $productIds = $this->arrayToCommaSeparated($productIds);
        }

        return $this->get("reprice_settings", [
            'marketplace' => $marketplace,
            'format' => $format,
            'ids' => $productIds,
            'pformat_dec' => $priceFormat
        ]);
    }

    /**
     * Perform GET to get license info.
     * 
     * @return string
     */
    public function getLicense() {
        return $this->get("license");
    }

    /**
     * GET request.
	 * @param  string  $path
	 * @param  array   $parameters
     * 
     * @return string
     */
    private function get(string $path, array $parameters = null)
    {
        if($parameters != null) {
            $path .= '?'. http_build_query($parameters);
        }
        
        return $this->request($path, []);
    }

    /**
     * POST request.
	 * @param  string  $path
	 * @param  array   $parameters
     * 
     * @return string
     */
    private function postFile(string $path, string $filename, array $parameters)
    {
        if($parameters != null) {
            $path .= '?'. http_build_query($parameters);
        }

        $curlOptions = [
            CURLOPT_POST => 1,
            CURLOPT_HTTPHEADER => ['Content-Type: text/csv'],
            CURLOPT_POSTFIELDS => file_get_contents($filename)
        ];

        return $this->request($path, $curlOptions);
    }

    /**
     * Process request.
	 * @param  string  $path
	 * @param  array   $options (for curl)
     * 
     * @return string
     */
    private function request(string $path, array $options)
    {
        $url = self::BASE_URL . $this->apiKey . '/' . $path;
        
        // combine curl options
        $options += [
            CURLOPT_RETURNTRANSFER => true,
            CURLOPT_HEADER => false,
            CURLOPT_USERAGENT => 'PriceMonitor PHP SDK',
            CURLOPT_SSL_VERIFYPEER => false
        ];

        // make curl request
		$curl = curl_init($url);
		curl_setopt_array($curl, $options);
        $result = curl_exec($curl);
        
        // Check for an error
		if (curl_errno($curl)) {
			throw new PriceMonitorException('Server error: ' . curl_error($curl));
		}

        curl_close($curl);
        
		return $result;
    }

    /**
     * Convert boolean to string.
	 * @param  bool  $boolean
     * 
     * @return string
     */
    private function booleanToString(bool $boolean)
    {
        return ($boolean) ? 'true' : 'false';
    }

    /**
     * Convert array to string.
	 * @param  array  $list
     * 
     * @return string
     */
    private function arrayToCommaSeparated(array $list)
    {
        return implode(",", $list);
    }
}