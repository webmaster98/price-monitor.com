import request from 'request-promise';
import url from 'url';
import fs from 'fs';


export default class PriceMonitorApi {
  constructor(options) {
    options = options || {};
    if (!options.apiKey) {
      throw 'apiKey is required';
    }
    this.apiKey = options.apiKey;
    this.protocol = options.protocol || 'https';
    this.host = options.host || 'price-monitor.com';
    this.port = options.port || null;
    this.base = options.base || '/api/prm/login';
    this.strictSSL = options.hasOwnProperty('strictSSL') ? options.strictSSL : false;
    this.request = options.request || request;
    this.baseOptions = {};

    if (options.timeout) {
      // base options for request
      this.baseOptions.timeout = options.timeout;
    }
  }

  /**
   * @ignore
   */
  async doRequest(requestOptions) {
    const options = {
      ...this.baseOptions,
      ...requestOptions,
    };
    console.log(options);
    const response = await this.request(options);
    console.log(2);
    if (response) {
      console.log(response);
      if (Array.isArray(response.message) && response.message.length > 0) {
        throw new Error(response.message.join(', '));
      }
    }

    return response;
  }

  /**
   * @ignore
   */
  makeRequestHeader(uri, options = {}) {
    return {
      rejectUnauthorized: this.strictSSL,
      method: options.method || 'GET',
      uri,
      // json: true,
      ...options,
    };
  }

  /**
   * @ignore
   */
  makeUri({
    pathname,
    query,
  }) {
    const uri = url.format({
      protocol: this.protocol,
      hostname: this.host,
      port: this.port,
      pathname: `${this.base}/${this.apiKey}/${pathname}`,
      query,
    });
    console.log(uri);
    return decodeURIComponent(uri);
  }

  /**
   * Perform GET request to get price updates.
   * @param {Object} param - param
   * @param  {!string} param.marketplace -  ex: `idealo.de`, `google.de`
   * @param  {?string} param.format - `json` or `csv`
   * 
   * @return {Promise}
   *
   * @example
   * var api = new PriceMonitorApi({ apiKey: 'abcdef123123'});
   * api.getPriceUpdates({ marketplace: 'idealo.de', format: "json"})
   *    .then(function(response){ ... });
   *
   */
  getPriceUpdates({
    marketplace,
    format = null,
    exportall = null,
    id = null,
    test = null,
    pformat_dec = null
  }) {
    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'get_price_updates',
      query: arguments[0]
    })));
  }

  /**
   * Perform POST request to update products.
   * @param {Object} param - param
   * @param  {!string}  param.marketplace - ex: `idealo.de`, `google.de`
   * @param  {!string}  param.file - csv filename containing product list
   * @param  {?string}  param.separator  - `comma`, `semicolon` or `tab`
   * @param  {?string}  param.lineend  - windows \r\n (`win`) or unix \n (`unix`)
   * @param  {?boolean}    param.keepold  - deletes old products from database
   * @param  {?boolean}    param.cleanold  - deletes products not included in csv file from database
   * @param  {?boolean}    param.test  - data won't be saved to database
   *
   * 
   * @return {Promise}
   *
   * @example
   * var api = new PriceMonitorApi({ apiKey: 'abcdef123123'});
   * api.importProducts({ marketplace: 'idealo.de', file: '/path/to/file.csv', separator: 'comma'})
   *    .then(function(response){ ... });
   *
   */
  importProducts({
    marketplace,
    file,
    separator = null,
    lineend = null,
    keepold = null,
    cleanold = null,
    test = null
  }) {
    var options = arguments[0];
    var upload = options.file;
    delete options.file;
    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'import_products',
      query: options
    }), {
      method: 'POST',
      headers: [{
        name: 'content-type',
        value: 'text/csv'
      }],
      body: fs.ReadStream(upload)
    }));
  }

  /**
   * Perform GET request to get product offers.
   * @param  {Object} param - param   
   * @param  {!string} param.marketplace - ex: `idealo.de`, `google.de`
   * @param  {?string} param.format  - `json` or `csv`
   * @param  {?boolean}   param.exportall  - get all products or products with a price update
   * @param  {?string} param.sortby  - `total_price` or `price` or `shipping_costs` or `ranking`
   * @param  {?number}    param.offerid  - integer id
   * @param  {?(string|number[])} param.ids - comma separated list if ids or array of ids 
   * @param  {?number}    param.pformat_dec - integer (1) or decimal (2)
   * 
   * @return {Promise}
   *
   * @example <caption>Using array of ids</caption>
   * var api = new PriceMonitorApi({ apiKey: 'abcdef123123'});
   * api.exportProducts({ marketplace: 'idealo.de', format: 'json', ids: [33,981,32]})
   *    .then(function(response){ ... });
   *
   * @example <caption>Using of string with comma separated ids</caption>
   * var api = new PriceMonitorApi({ apiKey: 'abcdef123123'});
   * api.exportProducts({ marketplace: 'idealo.de', format: 'json', ids: '33,981,32'})
   *    .then(function(response){ ... });
   *
   */
  exportProducts({
    marketplace,
    format = null,
    exportall = null,
    sortby = null,
    offeridx = null,
    ids = null,
    pformat_dec = null
  }) {
    var args = arguments[0];
    if (args.ids && args.ids instanceof Array) {
      args.ids = args.ids.join(',');
    }
    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'export',
      query: args
    })));
  }

  /**
   * Perform GET request to get products with an error status.
   * @param  {Object} param - param      
   * @param  {!string} param.marketplace - ex: `idealo.de`, `google.de`
   * @param  {?string} param.format - `json` or `csv`
   * 
   * @return {Promise}
   */
  getErrors({
    marketplace,
    format = null,
  }) {
    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'get_errors',
      query: arguments[0]
    })));
  }

  /**
   * Perform GET request to delete products.
   * @param  {Object} param - param      
   * @param  {!string} param.marketplace - ex: `idealo.de`, `google.de`
   * @param  {!(string|number[])} param.ids - comma separated list if ids or array of ids
   * 
   * @return {Promise}
   */

  deleteProducts({
    marketplace,
    ids
  }) {
    var args = arguments[0];
    if (args.ids instanceof Array) {
      args.ids = args.ids.join(',');
    }

    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'delete_products',
      query: args
    })));
  }


  /**
   * Perform GET request to set marketplace settings.
   * @param  {Object} param - param         
   * @param  {!string}  param.marketplace - ex: `idealo.de`, `google.de`
   * @param  {!string}  param.url
   * @param  {!boolean}    param.repricing - `on` or `off`
   * @param  {!boolean}    param.ean 
   * 
   * @return {Promise}
   */

  marketplaceSettings({
    marketplace,
    url,
    repricing,
    ean
  }) {
    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'marketplace_settings',
      query: arguments[0]
    })));
  }

  /**
   * Perform GET request to get reprice settings.
   * @param  {Object} param - param         
   * @param  {!string}  param.marketplace - ex: `idealo.de`, `google.de`
   * @param  {?string}  param.format - `json` or `csv`
   * @param  {?(string|number[])}   param.ids - comma separated list if ids or array of ids 
   * @param  {?number}     param.pformat_dec - integer (1) or decimal (2)
   * 
   * @return {Promise}
   * @example
   * var api = new PriceMonitorApi({ apiKey: 'abcdef123123'});
   * api.getRepriceSettings({ marketplace: 'idealo.de', format : 'csv', pformat_dec: 2})
   *    .then(function(response){ ... });
   *
   */
  getRepriceSettings({
    marketplace,
    format = null,
    ids = null,
    pformat_dec = null
  }) {
    var args = arguments[0];
    if (args.ids && args.ids instanceof Array) {
      args.ids = args.ids.join(',');
    }

    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'reprice_settings',
      query: args
    })));
  }

  /**
   * Perform POST request to get reprice settings.
   * @param  {Object} param - param         
   * @param  {!string}  param.marketplace - ex: `idealo.de`, `google.de`
   * @param  {?string}  param.separator  - `comma`, `semicolon` or `tab`
   * @param  {?string}  param.lineend  - windows \r\n (`win`) or unix \n (`unix`)
   *
   * @return {Promise}
   */
  setRepriceSettings({
    marketplace,
    separator = null,
    lineend = null
  }) {
    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'reprice_settings',
      query: arguments[0]
    }), {
      method: 'POST'
    }));
  }
}
