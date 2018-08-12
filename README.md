# price-monitor.com RESTfull API

Welcome to price monitor the Reprice Robot’s API
Specification!
price monitor the Reprice Robot provides an Application Programming Interface (API) to enable seamless integration
of price monitor into any shop and/or warehouse computer system. This document describes the interface and provides
exemplary code snippets for how to use it. The API definition is in subject to future changes. For comments, bug
reports, change or feature requests, please refer to https://price-monitor.freshdesk.com/support/tickets/new
or for any samle code issue at https://github.com/webmaster98/price-monitor.com/issues

For more sample please check best practice at https://github.com/webmaster98/price-monitor.com/blob/master/prm-best-practice.md

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

if you need to monitor prices in additional marketplaces like [Marketplace links](https://github.com/webmaster98/price-monitor.com/blob/master/M-list.txt) or at a dedicated one please send us a email at webmaster@price-monitor.com !!

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


---

### Import and Update of Products

This function can be used in order to update product information such as product descriptions, EAN codes,
minimal and maximal prices or the like.

| HTTP request method:      |   Method       
| ------------- |:-------------
| POST          | import_products 

#### Argumets :
*  marketplace: the marketplace for which the products shall
be imported. Please refer to the section Common Data
Types for admissible values.
*  separator (optional): the character separating the
values.
○ comma: comma (,) separated
○ semicolon: semicolon (;) separated
○ tab: tab (\t) separated
*  The default separator is comma.
*  lineend (optional): the line ending
○ win: windows line endings (\r\n)
○ unix: unix lineendings (\n)
*  keepold (optional): true/false; determines whether or
not the old product entries shall be kept or removed
from the database. If only new products are contained in
the data, this parameter has to be set to true. Default is
false.
*  test (optional): true/false; if set to true, the changes
will not be applied to the  price monitor database. This
parameter can be used for developing and testing your import application to see if the data formats are correct
and will be accepted by  price monitor. Default is false.
*  cleanold (true|false) function delete products in database that not includes in csv

### Return Success Value:

A JSON dictionary containing the number of inserts, updates and
deletions that have been performed:
```json
{ 
  "inserts": "<#inserts>",
  "updates": "<#updates>",
  "deletions": "<#deletions>",
  "unchanged": "<#unchanged>"
 }
```

This command updates existing product entries for a particular marketplace, or performs an initial import of a
product list into price monitor. Existing entries are updated according to your own product id. Data to be imported
needs to be in a valid CSV format, containing the following columns:

1. id: the product id in your own shop. This needs to be unique for every item, otherwise  price monitor will
report an error.
2. ean: the EAN code for the product. For some marketplaces, an EAN number is required for robustly
identifying your products on that marketplace.
3. id_on_marketplace: the marketplace-specific ID of this product, e.g. the ASIN number for Amazon.
4. category: the category the respective item belongs to in your shop
5. mpn: Manufacturer Part Number
6. manufacturer: Manufacturer of the item
7. model: Model name of the product given by the manufacturer
8. name_on_marketplace: The product name under which the item appears on the marketplace. The more
this item
9. min_price: the minimal price of the item that will never be undersold.
10. max_price: the maximal price of the item that will never be oversold. The CSV data needs to be transferred in the body of the HTTP POST method.

#### Please note that after import the articles settings have the default values like :
```json 
{ 
  "Strategy": "gentle",
  "round_patterns":null , 
  "undertbet": 0.01 ,
  "target ranking" :1 , 
  "shipping_costs":true,
  "rating":true, 
  "availability":true, 
  "discounts": null 
} 
```
To​ ​avoid​ ​to​ ​initial​ ​articles​ imediatly ​with​ ​default​ ​values​ ​please​ set reprice option to off ( marketplace_settings?marketplace=<MARKETPLACE>&repricing=off ) !!!
  
  
### Export of Product Offers
This function can be used in order to make a full export of product offers available in price monitor.


| HTTP request method:      |   Method       
| ------------- |:-------------
| GET           | export 
  

#### Argumets :
*  marketplace: the marketplace from which the products shall be
exported Please refer to the section Common Data Types for
admissible values.
*  format (optional): the format in which the data shall be returned.
Possible values are:
○ json: the data will be provided in a JSON list of
dictionaries,
○ csv: the data will be provided as a CSV table
*  The default value is json.
*  exportall (optional): {true,false}: If set to true, a list of offers
including all competitors will be made. The default value is true.
*  sortby (optional): a column name by which the offers are to be
sorted. Possible values are
○ total_price: the brut price of an offer.
○ price: the net price of an offer
○ shipping_costs: the shipping of an offer
○ ranking: the ranking on the marketplace.
*  The default value is ranking
*  offeridx (optional): return only the offers with the given index after
sorting them. If no offeridx is given, it will return all offers.
(default).
*  ids (optional): A (possibly comma- separated list of) product IDs
whose data is to be exported. If this parameter is not specified, all
products will be exported.
*  pformat_dec (optional): price format , 1=interger, 2=float default is 1
  
  
### Return Success Value:
Either a table in CSV format or a list of JSON dictionaries containing the
exported data.

The export data comprises the following attributes (depending on the value of the format parameter, either
column in the CSV data or JSON dictionary keys):
*  ID: the internal product ID.
*  PRODUCT NAME: the name of the product.
*  CATEGORY: the category of the product, if any.
*  MANUFACTURER: the manufacturer of the product.
*  MODEL: the model of the product
*  EAN: the EAN code of the product
*  MPN: the Manufacturer Part Number of the product, if any.
*  STATUS: the current status of the product in  price monitor
*  RANKING: the ranking of the offer on the respective marketplace.
*  SHIPPING COSTS: the shipping costs of the offer.
*  SHOP: the name of the competitor (or the name of the own shop)
*  NEW PRICE: the price update suggestion by  price monitor.
*  OLD PRICE: the the current price listed on the marketplace.
*  PRICE CHANGE: the difference between old price and new price (absolute and relative)
*  LAST UPDATE: the timestamp of the most recent update
*  AVAILABILITY: the time until the item will be shipped as a time interval. 00-00means the item is
available and will be shipped without delay. -1 means unvailable. -9999 means unknown availability.
*  BEST OFFERER: the name of the shop with the best offer
*  BEST PRICE: the price of the best offer



### Export Erronoeous Products
This function can be used in order to make an export of products that have an error status:



| HTTP request method:      |   Method       
| ------------- |:-------------
| GET           | get_errors


#### Argumets :
*  marketplace: the marketplace from which the products shall be exported Please refer to the section Common Data Types for admissible values.
*  format (optional): the format in which the data shall be returned. Possible values are:
○ json: the data will be provided in a JSON list
of dictionaries,
○ csv: the data will be provided as a CSV table
*  The default value is json.

### Return Success Value:

Either a table in CSV format or a list of JSON dictionaries
containing the exported data.


## Deletion of Products
This function can be used in order to delete single or multiple products from price monitor.

| HTTP request method:      |   Method       
| ------------- |:-------------
| GET           | delete_products

#### Argumets :
*  marketplace: the marketplace from which the products
shall be exported Please refer to the section Common
Data Types for admissible values.
*  ids: A (possibly comma-separated list of) product IDs
whose data is to be deleted.


## Set Marketplace Settings
This method is used to set or initialize the settings specific to a marketplace. Before a call import_products can be
made, this method must be called once to initialize a marketplace.

| HTTP request method:      |   Method       
| ------------- |:-------------
| GET           | marketplace_settings

#### Argumets :

*  marketplace: the marketplace from which the products shall be exported
Please refer to the section Common Data Types for admissible values.
*  repricing={on,off}: whether the repricing functionality of  price monitor shall
be switched on of off. If you only want to observe the marketplace
without identification and repricing of own products, set this to off.
Default is on.
*  url: the marketplace-specific URL of your shop used for identification.
On Amazon and ebay, for example, this should be set to your shop URL
on the respective marketplace.
*  ean: whether or not to use the EAN for importing new products


On Amazon and ebay, for example, this should be set to your shop URL
on the respective marketplace.
● ean: whether or not to use the EAN for importing new products

### Return Success Value:
A JSON string confirming the settings: {'url':<url>, 'repricing': <on/off>, 'ean':<on/off>}

Note: In case of Amazon, eBay, google or Mercateo.com, the Shop-description must have the following form:
http://www.amazon.de/shops/<YOUR-AMAZON-ID>
http://stores.ebay.de/<YOUR-EBAY-ID>
My shop as a string at google < like test shop >
http://www.mercateo.com/<YOUR-MERCATEO-ID>
For all other marketplaces, the domain of your shop is sufficient.

## Set Reprice Settings
This function can be used in order to update settings for the repricing strategies, such as minimal and maximal
prices or the desired ranking.

| HTTP request method:      |   Method       
| ------------- |:-------------
| POST           | reprice_settings

#### Argumets :
*  marketplace: the marketplace from which the products shall be exported
Please refer to the section Common Data Types for admissible values.
*  separator (optional): the character separating the values.
○ comma: comma (,) separated
○ semicolon: semicolon (;) separated
○ tab: tab (\t) separated
*  The default separator is comma.
*  lineend (optional): the line ending
○ win: windows line endings (\r\n)
○ unix: unix lineendings (\n)

### Return Success Value:

A JSON string containing the number changed and unchanged products:
```json
{
"changed": "<#changed>", 
"unchanged": "<#unchanged>" 
}
```


This command updates reprice settings for existing products on a particular marketplace. Existing entries are
updated according to your own product id. Data to be imported can be either in CSV or in a dictionary in JSON
format. In either case, the HTTP Content-Type header must be set appropriately to text/csv or application/json,
respectively.
The CSV/JSON data may contain the following columns:
1. id: the product id in your shop.
2. min_price: the minimal price of the item in the smallest unit of the currency, e.g. cents.
3. max_price: the maximal price of the item in the smallest unit of the currency, e.g. cents.
4. strategy: the strategy to follow in repricing. Possible values are
○ aggressive: always chooses the minimal reasonable price, even in cases where the
ranking cannot be improved.
○ gentle: always chooses the prices optimizing the profit margin
5. underbet: the amount that a competitor’s price shall be underbid, in the smallest currency unit, e.g.
cents
6. round_patterns: a comma-separated list of round patterns, for instance *5,*0 for prices. Caution: If
the CSV file itself is comma-separated, this must be escaped by a string sequence!
7. If set to none, rounding is disabled.
8. ranking: the desired ranking that  price monitor tries to achieve, e.g. 1 for the highest rank.
9. shipping_costs: (true/false) whether or not shipping costs should be taken into account
10.rating: (true/false) whether or not the customer ratings should be taken into account
11. availability: (integer) the number of days a competitor needs for shipping an item until the item gets
considered “unavailable” by  price monitor. If none, availability will not be taken into account at all.
12. discounts: how discounts or additions should be treated:
○ none: discounts and additions will not be taken into account.
○ absolute: discounts and additions will be considered absolute numbers
○ relative/netto: discounts and additions will be considered relatively to the net price of
an item (i.e. without shipping costs)
○ relative/brutto: discounts and additions will be considered relatively to the brut price
of an item (i.e. with shipping costs included).
13. sellerurl: the marketplace-specific URL of your shop used “http://myshop.de”

### Example (CSV)
```csv
id;min_price;max_price;strategy
00-11-22-33;1199;1999;aggressive
00-11-22-34;;aggressive
```
```json
### Example (JSON)
[
  {"id": "00-11-22-33",
  "min_price": 1199, 
  "max_price": 1999, 
  "strategy": "aggressive"
  }
]
```

The two examples are equivalent. They will set the repricing strategy for the two items with IDs 00-11-22-33 and
00-11-22-34 to “aggressive” mode, set the minimal price of the former product to 11.99, the maximal price to
19.99, and leaves the min and max prices of the latter item unchanged.

## Retrieve Reprice Settings
This method is used to retrieve the reprice settings for products stored in  price monitor.


| HTTP request method:      |   Method       
| ------------- |:-------------
| GET           | reprice_settings

#### Argumets :

* arketplace: the marketplace for which new price
adaptations shall be queried. Please refer to the section
Common Data Types for admissible values.
* format (optional): the format in which the data shall be
returned. Possible values are:
○ json: the data will be provided in JSON
key-value pairs,
○ csv: the data will be provided in a CSV table
* The default value is json.
* ids: a (comma-separated) list of of product IDs whose
settings are to be exported.
* pformat_dec (optional): price format , 1=interger, 2=float default is 1


### Return Success Value:

the settings data in CSV or JSON format as specified in Set Reprice Settings

## Get License Information
This method can be used to retrieve general information about the  price monitor license and booked marketplaces.

| HTTP request method:      |   Method       
| ------------- |:-------------
| GET           | license

#### Argumets :

no arguments

### Return Success Value:

the license data in JSON format The values of a license include:
* customer_id: Your  price monitor customer ID.
* license: Your  price monitor license
* start_date: The first day of validity of your license
* end_date: The date when your  price monitor license will expire.
* max_products: The maximum number of products that your allowed to manager with your  price monitor
license.
* marketplaces: A list of marketplaces that you have booked.

