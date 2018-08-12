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
REST Service Calls to BENY
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

