## Price-monitor best practice 

api API HTTP request need to use the url https://price-monitor.com/api/prm/login/​<API-KEY>/

### 1. Get​ ​Price​ ​Updates​ ​for​ ​Products
Query fields options:
*  marketplace​ (“String” Required ) In case required fields not setthe API response :
○ {"message":"Field marketplace invalid","reason":"Bad Request","status":400}
*  id​ (“String”, Optional )
*  pformat_dec​ ( “integer”, Optional ) ->defaultis integer
*  test​ (“boolean”, Optional ) -> defaultis false
*  format​ ( “String”, Optional ) ->defaultis json
*  exportall​ (“boolean”, Optional )

### 1.1​ ​You​ ​want​ ​to​ ​get​ ​the​ ​price monitor​ ​price​ ​suggestion​ ​from​ ​a​ ​ ​dedicated​ ​product
REQUEST: GET

URL: get_price_updates?marketplace=idealo.de&id=<Artikel-ID>"

RESPONSE
```json
[{"id":"<Artikel-ID>","new_price":55884​,"old_price":55885​}]
```
### 1.2​ ​You​ ​want​ ​to​ ​get​ ​the​ ​price monitor​ ​price​ ​suggestion​ ​from​ ​a​ ​ ​dedicated​ ​product​ ​in​ ​decimal​ ​format

REQUEST: GET

URL: get_price_updates?marketplace=idealo.de&id=<Artikel-ID>&pformat_dec=2"

RESPONSE
```json
[{"id":"<Artikel-ID>","new_price":558.84​,"old_price":558.85​}]
```
### 1.3​ ​You​ ​want​ ​to​ ​get​ ​the​ ​price monitor​ ​price​ ​suggestion​ ​from​ ​a​ ​ ​dedicated​ ​product​ ​in​ ​decimal​ ​format​ ​and​ ​as​ ​CSV​ ​output​ ​format

REQUEST: GET

URL: get_price_updates?marketplace=idealo.de&id=<Artikel-ID>&pformat_dec=2&format=csv"

RESPONSE
```csv
Id,new_price,old_price
<Artikel-ID>,558.84,558.85
```
### 1.4​ ​You​ ​want​ ​to​ ​get​ ​the​ ​price monitor​ ​price​ ​suggestion​ ​from​ ​all​ ​products​ ​in​ ​decimal​ ​format​ ​and​ ​as​ ​CSV​ ​output​ ​format​ ​only​ ​if​ ​price monitor
calculate​ ​the​ ​price​ ​suggestion
REQUEST: GET

URL: get_price_updates?marketplace=idealo.de&id=<Artikel-ID>&pformat_dec=2&format=csv&exportall=false"

RESPONSE
```csv
id,new_price,old_price
<Artikel-ID>,200.98,296.98
<Artikel-ID>,451.08,451.01
<Artikel-ID>,436.77,403.95
<Artikel-ID>,298.98,298.98
```


## 2. Import​ ​and​ ​Update​ ​of​ ​Products
Query fields options:
*  marketplace​ ( “String”,Required ) In case required fields not setthe API response :
○ {"message":"Field marketplace invalid","reason":"Bad Request","status":400}
*  separator​ ( “String”, Optional ) -> defaultis comma
*  lineend​ ( “String”,Optional )
*  keepold​ (“boolean”, Optional ) -> defaultis true
*  test​ ​ (“boolean”, Optional ) -> defaultis false


### 2.1​ ​You​ ​want​ ​to​ ​import​ ​5240​ ​articles​ ​as​ ​csv​ ​format​ ​ ​with​ ​comma​ ​separated​ ​)​ ​override​ ​old​ ​entries​ ​and​ ​start​ ​initialisation

The example of article-list.csv​ list:
```csv
Id,ean,id_on_marketplace,category,mpn,manufacturer,model,name_on_marketplace,min_price,max_price
<Artikel-iD>,7611382551115,,Armbanduhr,XS.3051,Luminox,<NAME_ON_MPlace>,16710, 69800
<Artikel-iD>,7611382551122,,Armbanduhr,XS.3059,Luminox,<NAME_ON_MPlace>,15120, 69800
etc ….
```
REQUEST: POST

URl: import_products?marketplace=idealo.de&separator=comma&keepold=false&lineend=unix" --data-binary @article-list.csv​ -H 'Content-type: text/csv'

RESPONSE:
```json
{"deletetions":33,"inserts":5240,"notes":[],"unchanged":0,"updates":0}
```

#### NOTES

In case of one or multiple corrupted row/s the output should be :
```json
{"deletetions":5240,"inserts":5239,"notes":["Invalid row 1 : Field min_price invalid"],"unchanged":0,"updates":0}
```
Please note that after importthe articles settings have the default values like :
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
To​ ​avoid​ ​to​ ​initial​ ​articles​ ​with​ ​default​ ​values​ ​please​ ​follow​ ​2.3​ ​topic​ ​!!!


### 2.2​ ​You​ ​want​ ​to​ ​import​ ​5240​ ​articles​ ​as​ ​csv​ ​format​ ​ ​with​ ​comma​ ​separated​ ​)​ ​ ​and​ ​not​ ​override​ ​old​ ​entries​ ​only​ ​update

REQUEST: POST
URL: import_products?marketplace=idealo.de&separator=comma&keepold=true&lineend=unix" --data-binary @article-list.csv​ Content-type:text/csv'

RESPONSE
```json
{"deletetions":0,"inserts":0,"notes":[],"unchanged":0,"updates":5240}
```

### NOTES
In case of one or multiple corrupted row/s the output should be :
```json
{"deletetions":0,"inserts":1,"notes":["Invalid row 3 : Field ean invalid"],"unchanged":1,"updates":5239}
```

### 2.3​ ​You​ ​want​ ​to​ ​import​ ​5240​ ​articles​ ​as​ ​csv​ ​format​ ​ ​set​ ​individual​ ​ ​repricing​ ​and​ ​after​ ​initial​ ​all​ ​items

REQUEST 

1. Turn​ ​repricing​ ​off​ ( see topic 6 ) = marketplace_settings?marketplace=idealo.de&repricing=off
2. Import​ ​articles​ ( see topic 3 ) = import_products?marketplace=idealo.de&separator=comma&keepold=true&lineend=unix"
--data-binary @article-list.csv Content-type:text/csv'
3. Make​ ​settings​ ( see topic 7 ) = reprice_settings?marketplace=idealo.de&lineend=unix" --data-binary @data.csv -H 'Content-type: text/csv' -X POST
4. Turn​ ​repricing​ ​on​ ( see topic 6 ) = marketplace_settings?marketplace=idealo.de&repricing=off


### 3. Export​ ​of​ ​Product​ ​Offers
Query fields options:
*  marketplace​ ( Required ) In case required fields not setthe API response :
○ {"message":"Field marketplace invalid","reason":"Bad Request","status":400}
*  separator​ (“String”,Optional ) -> defaultis comma
*  format​ (“String”,Optional ) -> defaultis json
*  sortby​ ​ (“String”, Optional ) -> defaultis ranking
*  offeridx​ ​ (“integer”,Optional )
*  ids​ ​ (“String”, Optional ) -> param ids could contain multiple products ids separated by comma
*  pformat_dec​ ​ ​ (“integer”, Optional ) -> defaultis decimal
*  exportall​ ​ (“boolean”, Optional ) -> defaultis false

### 3.1​ ​You​ ​want​ ​to​ ​export​ ​a​ ​dedicated​ ​article

REQUEST: GET

URL: /export?marketplace=idealo.de&exportall=true&ids=<Artikel-iD>"


RESPONSE
```json
[
  {
    "AVAILABILITY": "-1​",
    "BEST OFFERER": "alternate.de​",
    "BEST PRICE": 85698​,
    "CATEGORY": "Deutsch>Kochfelder>Induktion>80​ ​CM​",
    "EAN": "4242004216421​",
    "ID": "<Artikel-iD>​",
    "LAST UPDATE": "2017-10-20T21:46:12.785Z​",
    "MANUFACTURER": "Neff​",
    "MODEL": "Neff​ ​TPT6860X​ ​Induktionskochfeld​ ​Autark​ ​T68PT60X0​",
    "MPN": "",
    "NEW PRICE":​ ​91899​,
    "OLD PRICE": 87499​,
    "PRICE CHANGE": "-44.00​ ​(-5.03%)​",
    "PRODUCT NAME": "Neff​ ​TPT6860X​ ​Induktionskochfeld​ ​Autark​ ​T68PT60X0​",
    "RANKING": 5​,
    "SHIPPING COSTS": 0​,
    "SHOP": "<YOUR​ ​SHOP​>",
    "SHOP_URL": "<YOUR​ ​SHOP​ ​URL>​",
    "STATUS": "ON/OK"​,
    "TOTAL PRICE": 87499
  }
]
```

### NOTES
Price formatis integer as default

### 3.2​ ​You​ ​want​ ​to​ ​export​ ​all​ ​articles​ ​in​ ​decimal​ ​format

REQUEST: GET

URL: /export?marketplace=idealo.de&exportall=true&pformat_dec=2"


RESPONSE
```json
{
    "AVAILABILITY": "-1",
    "BEST OFFERER": "elektroshopwagner.de",
    "BEST PRICE": 450.58,
    "CATEGORY": "Deutsch>Geschirrspüler>Einbau-​ ​Vollintegrierbar>Breite:​ ​60cm"​,
    "EAN": "4242004187004​",
    "ID": "<Artikel-iD>​",
    "LAST UPDATE": "2017-10-20T21:47:37.726Z​",
    "MANUFACTURER": "Constructa​",
    "MODEL": "Constructa​ ​CG4A52V8​ ​Einbaugeschirrspüler​",
    "MPN": "",
    "NEW PRICE": 450.57​,
    "OLD PRICE": 450.63​,
    "PRICE CHANGE": "0.06​ ​(0.01%)​",
    "PRODUCT NAME": "Constructa​ ​CG4A52V8​ ​Einbaugeschirrspüler​",
    "RANKING": 4,
    "SHIPPING COSTS": 0,
    "SHOP": "etrona.at",
    "SHOP_URL": "<YOUR​ ​SHOP​ ​URL>​",
    "STATUS": "ON/OK",
    "TOTAL PRICE": 450.63
}
]
```

### 3.3​ ​You​ ​want​ ​to​ ​export​ ​ ​a​ ​offer​ ​from​ ​competitor​ ​with​ ​ranking​ ​3

REQUEST : GET

URL: /export?marketplace=idealo.de&exportall=false&offeridx=3&ids=<Artikel-iD>


RESPONSE
``` 
[
{
      "AVAILABILITY": "00-00",
      "BEST OFFERER": "crowdfox.com",           ->BEST​ ​OFFERER​ ​NAME
      "BEST PRICE": 133869,                     ->BEST​ ​TOTAL​ ​ ​PRICE​ ​OF​ ​COMPETITOR
      "CATEGORY": "Deutsch>Waschen & Trocknen>Waschmaschinen>Frontlader",
      "EAN": "7332543382378",
      "ID": "<Artikel-iD>​",
      "LAST UPDATE": "2017-10-20T21:45:08.684Z",
      "MANUFACTURER": "Electrolux Professional",
      "MODEL": "Electrolux MyPro WE170P Gewerbliche Waschvollautomat",
      "MPN": "",
      "NEW PRICE": 13386800,                    ->​ ​OWN​ ​ARTICLE​ ​PRICE​ ​SUGGESTION
      "OLD PRICE": 133869,                      ->​ ​COMPETITOR​ ​PRICE
      "PRICE CHANGE": "0.17 (0.01%)",           ->​ ​OWN​ ​PRICE​ ​CHANGES
      "PRODUCT NAME": "MyPro WE 170 P",         ->​ ​COMPETITOR​ ​NAME
      "RANKING": 3,                             ->​ ​COMPETITOR​ ​RANKING
      "SHIPPING COSTS": 0,                      ->​ ​COMPETITOR​ ​SHIPPING​ ​COSTS
      "SHOP": "mediadeal.de",                    ->​ ​ ​COMPETITOR​ ​NAME
      "SHOP_URL": "mediadeal.de",                ->​ ​ ​COMPETITOR​ ​URL
      "STATUS": "ON/OK",                         ->​ ​ ​OWN​ ​ARTICLE​ ​STATUS
      "TOTAL PRICE": 133869 -                     >​ ​ ​COMPETITOR​ ​TOTAl​ ​PRICE
      }
]
``` 

## 4. Export​ ​Erronoeous​ ​Products

Query fields options:

*  marketplace​ (“String”, Required ) In case required fields not setthe API response :
○ {"message":"Field marketplace invalid","reason":"Bad Request","status":400}
*  format​ (“String”, Optional ) -> defaultis json

### 4.1​ ​You​ ​want​ ​to​ ​export​ ​all​ ​errors​ ​related​ ​to​ ​your​ ​articles

REQUEST: GET

URL: /get_errors?marketplace=idealo.de"


RESPONSE
```
{
"AVAILABILITY": "",
"BEST OFFERER": "",
"BEST PRICE": null,
"CATEGORY": "Deutsch>Haushalt Kleingeräte>Haushalt",
"EAN": " 5KSM125PSEOB",
"ID": "<Artikel-iD>​",
"LAST UPDATE": "2017-10-20T21:48:15.711Z",
"MANUFACTURER": "KitchenAid",
"MODEL": "KitchenAid Artisan 5KSM125EOB Küchenmaschine Onyx-Schwarz",
"MPN": "",
"NEW PRICE": null,
"OLD PRICE": null,
"PRICE CHANGE": "",
"PRODUCT NAME": "KitchenAid Artisan 5KSM125EOB Küchenmaschine Onyx-Schwarz",
"RANKING": "",
"SHIPPING COSTS": null,
"SHOP": "etrona.at",
"SHOP_URL": "http://www.etrona.at",
>>>>> "STATUS":​ ​"ON/NO_RESULT" <<<<<
"TOTAL PRICE": null
14
},
```

## NOTES

The following error messages occurs in dirrentern cases:

*  "ERROR_EAN", -> missing ean in product
*  "ON/MISSING_MANDATORIES", -> missing shop or seller_url
*  "ON/NO_RESULT", -> there are no competitors and productfound
*  "ON/OWN_PRODUCT_NOT_FOUND", -> competitors exists buttarget product notfound
*  "ON/UNKNOWN_ERROR", -> data transfer error (proxy error,timeout error, bad response from marketplace, etc .. )



## 5. Deletion​ ​of​ ​Products

Query fields options:
*  marketplace​ (“String”,Required ) In case required fields not setthe API response :
○ {"message":"Field marketplace invalid","reason":"Bad Request","status":400}
*  ids​ (“String”, Required ) -> param ids could contain multiple products ids separated by comma

### 5.1​ ​You​ ​want​ ​to​ ​delete​ ​a​ ​single​ ​article​ ​related​ ​to​ ​a​ ​marketplace

REQUEST: GET

URL: delete_products?marketplace=idealo.de&ids=<Artikel-iD>

RESPONSE
```
{"deleted":1}
```
## 5.2​ ​You​ ​want​ ​to​ ​delete​ ​a​ ​multiple​ ​articles​ ​related​ ​to​ ​a​ ​marketplace

REQUEST: GET

URL: delete_products?marketplace=idealo.de&ids=<Artikel-iD1>,<Artikel-iD2>

RESPONSE
```
{"deleted":2}
```


## 6. Set​ ​Marketplace​ ​Settings

Query fields options:
*  marketplace​ (“String”, Required ) In case required fields not setthe API response :
○ {"message":"Field marketplace invalid","reason":"Bad Request","status":400}
*  repricing​ (“boolean”,Optional ) -> defaultis on
*  url​ (“String”, Optional )
*  ean​ ​(“String”, Optional ) -> defaultis on

### 6.1​ ​You​ ​want​ ​to​ ​set​ ​a​ ​default​ ​url​ ​for​ ​you​ ​marketplace

REQUEST: GET

URL: marketplace_settings?marketplace=idealo.de&url=<SHOP-URL>

RESPONSE
```
{"ean":"on","repricing":"on","url":"<SHOP-URL>​"}
```

### 6.2​ ​You​ ​want​ ​to​ ​set​ ​your​ ​shop​ ​the​ ​repricing​ ​to​ ​off

REQUEST: GET

URL: marketplace_settings?marketplace=idealo.de&repricing=off

RESPONSE
```
{"ean":"on","repricing":"off","url":"<SHOP-URL>​"}
```


### Notes
In case of Amazon, eBay, google or Mercateo.com,the Shop-URL must have the following form:
http://www.amazon.de/shops/<Your-shop-ID>
http://stores.ebay.de/ <Your-shop-ID>
google shop string format like: my shop-online 
http://www.mercateo.com/<Your-shop-ID>

## 7. Set​ ​Reprice​ ​Settings
Query fields options:
*  marketplace​ (“String”, Required ) In case required fields not setthe API response :
17
○ {"message":"Field marketplace invalid","reason":"Bad Request","status":400}
*  separator​ (“String”,Optional ) -> defaultis comma
*  lineend​ ​ (“String”, Optional )

### 7.1​ ​do​ ​you​ ​want​ ​to​ ​know​ ​the​ ​ ​available​ ​fields​ ​and​ ​errors​ ​in​ ​case​ ​wrong​ ​filed​ ​types​ ​or​ ​names​ ​:
1. id:​ ​ ​(​ ​“String”,​ ​Required​ ​)​ ​the productid in your shop.
In case of missing id value the error message should be:
By error API response {"changed":0,"notes":["Invalid row 0 : Field id invalid"],"unchanged":xx}
2. min_price​ ​(“float​ ​or​ ​integer”,​ ​Optional​ ​)​:the minimal price ofthe item in the smallest unit ofthe currency, e.g. cents.
We try to convertinteger to float and show entries in UI as decimal
3. max_price​ ​(“float​ ​or​ ​integer”,​ ​Optional​ ​)​:the maximal price ofthe item in the smallest unit ofthe currency, e.g. cents.
We try to convertinteger to float and show entries in UI as decimal
4. strategy​ ​ ​(​ ​“String​ ​(only​ ​two​ ​options​ ​available​ ​aggressive or gentle )​ ​”,​ ​Optional​ ​)​:the strategy to follow in repricing. Possible values are ○
aggressive: always chooses the minimal reasonable price, even in cases where the ranking cannot be improved. ○ gentle: always chooses the prices optimizing the profit margin
By error API response 
```
{"changed":0,"notes":["Invalid row 0 : Field strategy invalid"],"unchanged":xxx}
```
5. underbet​ ​ ​(“float”,​ ​Optional​ ​)​:the amountthat a competitor’s price shall be underbid, in the smallest currency unit, e.g. cents
By error API response {"changed":0,"notes":["Invalid row 0 : Field underbetinvalid"],"unchanged":xxx}
The solution in this case is to change underbettype to floadtin your csv row
6. round_patterns​ ​ ​(“String”,​ ​Optional​ ​)​:
With BENY, you have the possibility to round off the new price propositions according to certain rules, which you can get. With this rule, it
is possible, for example, to instruct BENY to round off price proposals always on "smooth" amounts (price with extensively zeros behind
the comma).
A rule always consists of a pattern of 2 characters, at least one of which must be a number (0-9) and can be a ~ wildcard ~ (*) up to one
character. Such a pattern determines which amounts are left behind the comma of a price forecast. Price proposals are then rounded so
that the numbers in the pattern match exactly with the numbers in the price proposal, whereby the wildcard / character allows an arbitrary
number. You can specify a number of times, always with the highest price, which one of the regulars will allow, however, your minimum
price and your sub-amount.
Examples:
a. The pattern * 0 allows only ~ smooth ~ cent amounts, ie 00, 10, 20, 30, 40, 50, ... 90 behind the comma of a price proposal.
b. The two patterns * 0 and * 5 allow 0 rounded to 0 and 5, ie 00, 05, 10, 15, 20, 25, 30, 35, .... 95 behind the comma.
c. The two patterns 0 0 and 5 0 allow 'smooth' Euro and ~ rounded to 50 cents, ie 00 and 50 behind the comma.
d. The pattern * * allows any amount after the decimal point. If this pattern is specified, there is no rounding
You can specify multiple patterns in the repricing settings for your person, separated by comma, which are allowed when repricing. For the
above examples:
• Example 1 * 0
• Examples 2 * 0, * 5
• Examples 3 00, 50
• Example 4: ** (no rounding)
7. If setto none, rounding is disabled.
19
8. ranking​ ​(​ ​“integer”,​ ​Optional​ ​)​:the desired ranking that BENY tries to achieve, e.g. 1 for the highest rank.
9. shipping_costs​: (“boolean​”, Optional​ ​) whether or not shipping costs should be taken into account
10.rating​:(“boolean​”, Optional​ ​) whether or notthe customer ratings should be taken into account
11. availability​: ​ ​(​ ​“integer”,​ ​Optional​ ​)​ ​the number of days a competitor needs for shipping an item untilthe item gets considered
“unavailable” by BENY. If none, availability will not be taken into account at all.
12. discounts​ ​ ​ ​(​ ​“String”,​ ​Optional​ ​)​ ​: how discounts or additions should be treated:
○ none: discounts and additions will not be taken into account.
○ absolute: discounts and additions will be considered absolute numbers
○ relative/netto: discounts and additions will be considered relatively to the net price of an item (i.e. without shipping costs)
○ relative/brutto: discounts and additions will be considered relatively to the brut price of an item (i.e. with shipping costs included).
13. sellerurl​ ​(​ ​“String”,​ ​Optional​ ​-​ ​ ​but​ ​if​ ​you​ ​want​ ​to​ ​initial​ ​immediately​ ​please​ ​set​ ​this​ ​value​ ​too​ ​ ​)​:the marketplace-specific URL of your
shop used “http://myshop.de”
If​ ​not​ ​of​ ​this​ ​ ​13​ ​fields​ ​names​ ​will​ ​be​ ​set​ ​the​ ​API​ ​response​ ​a​ ​default​ ​error​ ​like:
{"changed":0,"notes":["Invalid row 0 : Field id invalid"],"unchanged":xx}


## 7.2​ ​you​ ​want​ ​to​ ​set​ ​all​ ​settings​ ​in​ ​for​ ​a​ ​dedicated​ ​id​ ​in​ ​csv​ ​format
```
data.csv
Id,min_price,max_price,strategy,underbet,round_patterns,rating,shipping_costs,ranking,availability,discounts,sellerurl
xid-7369,12272,1919,gentle,0.01,05,true,true,1,7,none,http://<your-hop-url>.com
```
REQUEST: POST
URL: reprice_settings?marketplace=idealo.de&lineend=unix" --data-binary @data.csv -H 'Content-type:text/csv' -X POST

RESPONSE
```
{"changed":1,"notes":[],"unchanged":xx}
```

## 8. GET​ ​Reprice​ ​Settings

Query fields options:

*  marketplace​ (“String”, Required ) In case required fields not setthe API response :
○ {"message":"Field marketplace invalid","reason":"Bad Request","status":400}
*  format​ (“String”,Optional ) -> defaultis json
*  ids​ ​ (“String”, Optional )
*  pformat_dec​ (“integer”, Optional ) -> defaultis integer

### 8.1​ ​you​ ​want​ ​to​ ​get​ ​all​ ​settings​ ​in​ ​for​ ​a​ ​dedicated​ ​id​ ​in​ ​csv​ ​format

REQUEST: GET
URL: reprice_settings?marketplace=idealo.de&ids=<ARTICLE-ID>"

RESPONSE
```
id,min_price,max_price,strategy,underbet,round_patterns,ranking,shipping_costs,rating,availability,discounts
<ARTICLE-ID>,2272,3419,gentle,0.01,05,1,true,true,7,none
```

### 8.2​ ​you​ ​want​ ​to​ ​get​ ​all​ ​settings​ ​in​ ​for​ ​a​ ​dedicated​ ​id​ ​in​ ​csv​ ​format​ ​and​ ​the​ ​price​ ​in​ ​dedical​ ​format

REQUEST: GET

URL: reprice_settings?marketplace=idealo.de&ids=<ARTICLE-ID>&pfomat_dec=2"

RESPONSE
```
id,min_price,max_price,strategy,underbet,round_patterns,ranking,shipping_costs,rating,availability,discounts
<ARTICLE-ID>,22.72,34.19,gentle,0.01,05,1,true,true,7,none
```

## 9. Get​ ​License​ ​Information

This method can be used to retrieve general information aboutthe BENY license and booked marketplaces.

REQUEST: GET

URL: ?license

RESPONSE
```
[
    {
    "customer_id": "xxx",
    "end_date": "2021-07-23",
    "license": "Ultimate",
    "marketplaces": [
    "IdealoDE"
    ],
    "max_products": "7500",
    "start_date": "2017-07-23"
    }
]
```
