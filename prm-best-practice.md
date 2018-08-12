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















