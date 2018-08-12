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























