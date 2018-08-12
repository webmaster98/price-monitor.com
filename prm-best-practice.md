## Intro

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

### 1.1​ ​You​ ​want​ ​to​ ​get​ ​the​ ​Beny​ ​price​ ​suggestion​ ​from​ ​a​ ​ ​dedicated​ ​product
REQUEST
GET
get_price_updates?marketplace=idealo.de&id=<Artikel-ID>"
RESPONSE
[{"id":"<Artikel-ID>","new_price":55884​,"old_price":55885​}]
### 1.2​ ​You​ ​want​ ​to​ ​get​ ​the​ ​Beny​ ​price​ ​suggestion​ ​from​ ​a​ ​ ​dedicated​ ​product​ ​in​ ​decimal​ ​format
REQUEST
GET
get_price_updates?marketplace=idealo.de&id=<Artikel-ID>&pformat_dec=2"
RESPONSE
[{"id":"<Artikel-ID>","new_price":558.84​,"old_price":558.85​}]
### 1.3​ ​You​ ​want​ ​to​ ​get​ ​the​ ​Beny​ ​price​ ​suggestion​ ​from​ ​a​ ​ ​dedicated​ ​product​ ​in​ ​decimal​ ​format​ ​and​ ​as​ ​CSV​ ​output​ ​format
REQUEST
GET
get_price_updates?marketplace=idealo.de&id=<Artikel-ID>&pformat_dec=2&format=csv"
4
RESPONSE
Id,new_price,old_price
<Artikel-ID>,558.84,558.85
### 1.4​ ​You​ ​want​ ​to​ ​get​ ​the​ ​Beny​ ​price​ ​suggestion​ ​from​ ​all​ ​products​ ​in​ ​decimal​ ​format​ ​and​ ​as​ ​CSV​ ​output​ ​format​ ​only​ ​if​ ​Beny
calculate​ ​the​ ​price​ ​suggestion
REQUEST
GET
get_price_updates?marketplace=idealo.de&id=<Artikel-ID>&pformat_dec=2&format=csv&exportall=false"
RESPONSE
id,new_price,old_price
<Artikel-ID>,200.98,296.98
<Artikel-ID>,451.08,451.01
<Artikel-ID>,436.77,403.95
<Artikel-ID>,298.98,298.98