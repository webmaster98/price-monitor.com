<?php

namespace PriceMonitor;

use PriceMonitor\PriceMonitorException;

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
     * Perform get price updates GET request.
	 * @param  string  $marketplace
     * @param  string  $format (optional) - json or csv
     * @param  bool    $exportAll (optional) - get all products or products with a price update
     * @param  bool    $test (optional) - get random prices
	 * @param  int     $priceFormat (optional) - integer (1) or decimal (2)
     * 
     * @return string
     */
    public function getPriceUpdates(
        string $marketplace,
        string $format = null,
        bool $exportAll = true,
        bool $test = false,
        int $priceFormat = 1
    ) {
        return $this->get("get_price_updates", [
            'marketplace' => $marketplace,
            'format' => $format,
            'exportall' => $this->booleanToString($exportAll),
            'test' => $this->booleanToString($test),
            'pformatDec' => $priceFormat
        ]);
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
}