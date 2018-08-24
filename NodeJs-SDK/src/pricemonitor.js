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

  makeRequestHeader(uri, options = {}) {
    return {
      rejectUnauthorized: this.strictSSL,
      method: options.method || 'GET',
      uri,
      // json: true,
      ...options,
    };
  }

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

  importProducts({
    file,
    marketplace,
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

  exportProducts({
    marketplace,
    format = null,
    exportall = null,
    sortby = null,
    offeridx = null,
    ids = null,
    pformat_dec = null
  }) {
    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'export',
      query: arguments[0]
    })));
  }

  getErrors({
    marketplace,
    format = null,
  }) {
    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'get_errors',
      query: arguments[0]
    })));
  }

  deleteProducts({
    marketplace,
    ids
  }) {
    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'delete_products',
      query: arguments[0]
    })));
  }

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

  getRepriceSettings({
    marketplace,
    format = null,
    ids = null,
    pformat_dec = null
  }) {
    return this.doRequest(this.makeRequestHeader(this.makeUri({
      pathname: 'reprice_settings',
      query: arguments[0]
    })));
  }

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
