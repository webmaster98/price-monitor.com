use strict;
use warnings;
use Test::More;
use Test::Exception;

BEGIN {
  use_ok('PriceMonitor::API');
}
my $api = new_ok('PriceMonitor::API' => [api_key => 'foobar123']);

throws_ok { $api->get_price_updates() } qr/marketplace is a required parameter/,
  'throws ok';
throws_ok { $api->get_price_updates(marketplace => 'fooo', format => 'xxx') }
qr/Should be "json" or "csv"/, 'throws ok';

throws_ok {
  $api->import_products(
    marketplace => 'Foo',
    file        => '/does/not/exist/foo.csv'
    )
}
qr/foo.csv does not exist/, 'throws expected error';



done_testing();
