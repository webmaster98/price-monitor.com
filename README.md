# price-monitor.com

Welcome to price monitor the Reprice Robot’s API
Specification!
price monitor the Reprice Robot provides an Application Programming Interface (API) to enable seamless integration
of price monitor into any shop and/or warehouse computer system. This document describes the interface and provides
exemplary code snippets for how to use it. The API definition is in subject to future changes. For comments, bug
reports, change or feature requests, please refer to https://price-monitor.freshdesk.com/support/tickets/new
or for any samle code issue at https://github.com/webmaster98/price-monitor.com/issues


## General Information
The API is based on the paradigm of REST (Representational State Transfer) service models
REST Service Calls to price monitor
Every call to a function of the  price monitor API is being done by GET and POST methods of the HTTP protocol. The
general template of a URL for service calls to  price monitor looks like the following:


<b>https://price-monitor.com/api/prm/login/<api-key>/<func>[?arg0=val0[&arg1=val1[&...]]]</b>

Every price monitor customer gets a 36-digit API key, which authenticates them and gives access to the price monitor
functionality. The <api-key> field in the above URL is to be replaced by this key. The API key in the URL is
followed by the name of the service function to be called, such as get_price_updates, which will return the latest
price recalculations by price monitor. Some functions need to be parameterized by additional information about the
service. Function arguments can be handed over in the remainder of the HTTP query which has to be separated
by a question mark (“?”) followed by a list of key-value pairs. The key-value pairs themselves are separated by an
ampersand (“&”)

## Data Formats
The data exchanged between any  price monitor  client and the  price monitor  API servers are to be transmitted in the JSON or
CSV (Character/Comma Separated Value) formats. Which format is being used is to be determined by the
Content-Type keyword in the HTTP header of the respective request or response. All data need to be transferred
in the UTF-8 string encoding.

## Security and Encryption
All transactions are SSL encrypted.

## Common Data Types

Marketplaces
Some of the API functions need to be parametrized by the marketplace they are supposed to operate on. In most
cases, this can be specified by a HTTP query argument called marketplace. Admissible values are:

1. google.de
2. amazon.de
3. ebay.de
4. idealo.de
5. ladenzeile.de
6. rakuten.de
7. toppreise.de
8. mercateo.de
9. billiger.de
10. geizhals.de
11. B2B Marketplaces

Whenever there are prices in some of the variables in any request, they are to be specified as integer numbers in
the smallest unit of the respective currency. ***Example: A price of EUR 100.99 needs to be specified by the integer
10099.***


Function Specification of the API

### Get Price Updates for Products

This function can be used in order to retrieve price recommendations by price monitor , which in turn can be imported
into a warehouse system.

| HTTP request method:      |   Method       
| ------------- |:-------------
| GET           | get_price_updates 

#### Argumets :
* marketplace: the marketplace for which new price adaptations shall be
queried. Please refer to the section Common Data Types for admissible
values.
*  format (optional): the format in which the data shall be returned.
Possible values are:
○ json: the data will be provided in JSON key-value pairs,
○ csv: the data will be provided in a CSV table
* The default value is json.
* exportall={true,false}: determines whether or not a full export of all
products is made or only an export of those products that price monitor has a
price update suggestion for. The default value is true.
* id: get an update only for the product with the specified internal
product id (SKU).
* test (optional): If set to true, the call will return random prices and
updates. This is only for early integration and testing purposes.
* pformat_dec (optional): price format , 1=interger, 2=float default is 1

### Return Success Value:

The new prices of items for the given marketplace. Only items for which a new
price could be calculated are returned. JSON format: key-value pairs of the form:
[{'id': '<item_id>','new_price': <newPrice>}]
CSV format: Column-separated by semicolons (;). The column names are id (the ite
new price update) and old_price (the old current price)
  
```
curl "https://price-monitor.com/api/prm/login/<KEY>/get_price_updates?marketplace=google.de" -k | jq
[
  {
    "id": "325572",
    "new_price": "55884",
    "old_price": "52884"
  },
  {
    "id": "325575",
    "new_price": "51821",
    "old_price": "53385"
  }
]
```
##### with pformat_dec=2 paramenter
```
curl "https://price-monitor.com/api/prm/login/<KEY>/get_price_updates?marketplace=google.de&pformat_dec=2" -k | jq
[
  {
    "id": "325572",
    "new_price": "558.84",
    "old_price": "528.84"
  },
  {
    "id": "325575",
    "new_price": "518.21",
    "old_price": "533.85"
  }
]
```

###  Return Error Value
```json
{"message":"Field marketplace invalid","reason":"Bad Request","status":400}
```
