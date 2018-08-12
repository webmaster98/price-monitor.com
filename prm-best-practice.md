## Intro best practice 

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


2.2​ ​You​ ​want​ ​to​ ​import​ ​5240​ ​articles​ ​as​ ​csv​ ​format​ ​ ​with​ ​comma​ ​separated​ ​)​ ​ ​and​ ​not​ ​override​ ​old​ ​entries​ ​only​ ​update

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

2.3​ ​You​ ​want​ ​to​ ​import​ ​5240​ ​articles​ ​as​ ​csv​ ​format​ ​ ​set​ ​individual​ ​ ​repricing​ ​and​ ​after​ ​initial​ ​all​ ​items

REQUEST 

1. Turn​ ​repricing​ ​off​ ( see topic 6 ) = marketplace_settings?marketplace=idealo.de&repricing=off
2. Import​ ​articles​ ( see topic 3 ) = import_products?marketplace=idealo.de&separator=comma&keepold=true&lineend=unix"
--data-binary @article-list.csv Content-type:text/csv'
3. Make​ ​settings​ ( see topic 7 ) = reprice_settings?marketplace=idealo.de&lineend=unix" --data-binary @data.csv -H 'Content-type: text/csv' -X POST
4. Turn​ ​repricing​ ​on​ ( see topic 6 ) = marketplace_settings?marketplace=idealo.de&repricing=off


3. Export​ ​of​ ​Product​ ​Offers
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

3.1​ ​You​ ​want​ ​to​ ​export​ ​a​ ​dedicated​ ​article

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

3.2​ ​You​ ​want​ ​to​ ​export​ ​all​ ​articles​ ​in​ ​decimal​ ​format

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


