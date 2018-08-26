package PriceMonitor::API;
use Moo;
use strictures 2;
use namespace::clean;
use feature 'state';

use LWP::UserAgent;
use constant BASE_URL => q{https://price-monitor.com/api/prm/login/};

use Params::ValidationCompiler qw(validation_for);
use Types::Standard qw( Int Str Bool ArrayRef Ids);
use PriceMonitor::Types qw(FileFormat PriceFormat
  Separator LineEnd Upload
  SortType Repricing EAN
);
use JSON qw(encode_json decode_json);
use HTTP::Request::Common qw(GET POST PUT DELETE);
use URI;


has api_key => (is => 'ro', required => 1);

has _user_agent => (
  is       => 'ro',
  lazy     => 1,
  init_arg => undef,
  default  => sub {
    LWP::UserAgent->new(ssl_opts => {verify_hostname => 0});
  },
);

sub _uri_for {
  my ($self, $path, $params) = @_;
  my $uri = URI->new(BASE_URL);
  $uri->path_segments(grep { !!$_ } $uri->path_segments, $self->api_key, $path);

  $uri->query_form($params) if $params;
  return $uri->as_string;
}

sub get_price_updates {
  my $self = shift;
  state $v = validation_for(
    params => {
      marketplace => {type => Str},
      format      => {type => FileFormat, optional => 1},
      exportall   => {type => Bool, optional => 1},
      id          => {type => Int, optional => 1},
      test        => {type => Bool, optional => 1},
      pformat_dec => {type => PriceFormat, optional => 1},
    }
  );
  my (%args) = $v->(@_);
  $self->_make_request(GET $self->_uri_for(get_price_updates => \%args));
}

sub import_products {
  my $self = shift;
  state $v = validation_for(
    params => {
      file        => {type => Upload},
      marketplace => {type => Str},
      separator   => {type => Separator, optional => 1},
      lineend     => {type => LineEnd, optional => 1},
      keepold     => {type => Bool, optional => 1},
      cleanold    => {type => Bool, optional => 1},
      test        => {type => Bool, optional => 1},
    }
  );
  my (%args) = $v->(@_);
  my $file = delete $args{file};

  $self->_make_request(
    POST $self->_uri_for(import_products => \%args),
    Content_Type => 'text/csv',
    Content      => do { local (*ARGV, $/) = [$file]; <> }
  );
}


sub export {
  my $self = shift;
  state $v = validation_for(
    params => {
      marketplace => {type => Str},
      format      => {type => FileFormat, optional => 1},
      exportall   => {type => Bool, optional => 1},
      sortby      => {type => SortType, optional => 1},
      offeridx    => {type => Int, optional => 1},
      ids         => {type => Ids, optional => 1},
      pformat_dec => {type => PriceFormat, optional => 1},
    }
  );
  my (%args) = $v->(@_);
  $self->_make_request(GET $self->_uri_for(export => \%args));
}

sub get_errors {
  my $self = shift;
  state $v = validation_for(
    params => {
      marketplace => {type => Str},
      format      => {type => FileFormat, optional => 1},
    }
  );
  my (%args) = $v->(@_);
  $self->_make_request(GET $self->_uri_for(get_errors => \%args));
}

sub delete_products {
  my $self = shift;
  state $v = validation_for(
    params => {marketplace => {type => Str}, ids => {type => Ids},});
  my (%args) = $v->(@_);
  $self->_make_request(GET $self->_uri_for(delete_products => \%args));
}

sub marketplace_settings {
  my $self = shift;
  state $v = validation_for(
    params => {
      marketplace => {type => Str},
      url         => {type => Str},
      repricing   => {type => Repricing},
      ean         => {type => Ean}
    }
  );
  my (%args) = $v->(@_);
  $self->_make_request(GET $self->_uri_for(marketplace_settings => \%args));
}

sub get_reprice_settings {
  my $self = shift;
  state $v = validation_for(
    params => {
      marketplace => {type => Str},
      format      => {type => FileFormat, optional => 1},
      ids         => {type => Ids, optional => 1},
      pformat_dec => {type => PriceFormat, optional => 1},
    }
  );
  my (%args) = $v->(@_);
  $self->_make_request(GET $self->_uri_for(reprice_settings => \%args));

}

sub set_reprice_settings {
  my $self = shift;
  state $v = validation_for(
    params => {
      marketplace => {type => Str},
      file        => {type => Upload},
      separator   => {type => Separator, optional => 1},
      lineend     => {type => LineEnd, optional => 1},
    }
  );
  my (%args) = $v->(@_);
  $self->_make_request(
    POST $self->_uri_for(reprice_settings => \%args),
    Content_Type => 'text/csv',
    Content      => do { local (*ARGV, $/) = [$file]; <> }
  );
}

sub _make_request {
  my ($self, $req) = @_;
  my $res = $self->_user_agent->request($req);
  if (!$res->is_success) {
    my $message = '(empty)';
    if (my $expected = eval { decode_json($res->content) }) {
      $message = $expected->{message};
    }
    my $code   = $res->code;
    my $method = $req->method;
    my $uri    = $req->uri;
    my $reason = $code ? "response code $code" : "unexpected response";
    die
      "$reason received while processing the request $method $uri:\n  $message";
  }

  return decode_json($res->decoded_content)
    if $res->content_type eq 'application/json';
  return $res->decoded_content;

}

1;

__END__

=pod

=encoding UTF-8

=head1 NAME

PriceMonitor::API -- a client library for price-monitor.com

=head1 SYNOPSIS

    my $api = PriceMonitor::API->new(api_key => 'acbd18db4cc2f85cedef654fccc4a4d8');
    # Returns a HTTP::Response
    my $response = $api->get_price_updates(marketplace => 'amazon.de')
  
=head1 METHODS

All methods described below return a L<HTTP::Response>.

=over

=item C<get_price_updates(marketplace =E<gt> Str [, format =E<gt> 'json|csv', exportall =E<gt> Bool, id =E<gt> Int, test =E<gt> Bool, pformat_dec =E<gt> 1|2 (1 for integer format, 2 for decimal) ])>

Response formats example:

=over

=item C<?format=json>

  [{"id":"44556677","new_price": 55884 ,"old_price": 55885}]

=item C<?format=csv>

  Id,new_price,old_price
  44556677,558.84,558.85

=back

=item C<import_products(marketplace =E<gt> Str,  file =E<gt> FilePath [, separator =E<gt> 'comma|semicolon|tab', lineend =E<gt> 'win|unix', keepold =E<gt> Bool, cleanod =E<gt> Bool, test =E<gt> Bool])>

Example of CSV

  Id,ean,id_on_marketplace,category,mpn,manufacturer,model,name_on_marketplace,min_price,max_price
  <Artikel-iD>,7611382551115,,Armbanduhr,XS.3051,Luminox,<NAME_ON_MPlace>,16710, 69800
  <Artikel-iD>,7611382551122,,Armbanduhr,XS.3059,Luminox,<NAME_ON_MPlace>,15120, 69800


=item C<export(marketplace =E<gt> Str [, format =E<gt> 'json|csv', exportall =E<gt> 1|0, sortby =E<gt> 'total_price|price|shipping_costs|ranking', offeridx =E<gt> Int, pformat_dec =E<gt> 1|2 (1 for integer format, 2 for decimal)])>

Example:

  $api->export(marketplace => 'idealo.de', exportall => 0, offeridx => 3, ids=8877

Response:

    [{
      "AVAILABILITY": "00-00",
      "BEST OFFERER": "crowdfox.com", // BEST OFFERER NAME
      "BEST PRICE": 133869, // BEST TOTAL  PRICE OF COMPETITOR
      "CATEGORY": "Deutsch>Waschen & Trocknen>Waschmaschinen>Frontlader",
      "EAN": "7332543382378",
      "ID": "<Artikel-iD>",
      "LAST UPDATE": "2017-10-20T21:45:08.684Z",
      "MANUFACTURER": "Electrolux Professional",
      "MODEL": "Electrolux MyPro WE170P Gewerbliche Waschvollautomat",
      "MPN": "",
      "NEW PRICE": 13386800, // -> OWN ARTICLE PRICE SUGGESTION
      "OLD PRICE": 133869, // -> COMPETITOR PRICE
      "PRICE CHANGE": "0.17 (0.01%)", // OWN PRICE CHANGES
      "PRODUCT NAME": "MyPro WE 170 P", // COMPETITOR NAME
      "RANKING": 3, //  COMPETITOR RANKING
      "SHIPPING COSTS": 0, // COMPETITOR SHIPPING COSTS
      "SHOP": "mediadeal.de", // COMPETITOR NAME
      "SHOP_URL": "mediadeal.de", // COMPETITOR URL
      "STATUS": "ON/OK", // OWN ARTICLE STATUS
      "TOTAL PRICE": 133869 // COMPETITOR TOTAl PRICE
      }]
  
=item C<get_errors( marketplace => Str [, format =E<gt> 'json|csv'])>

Response:

  {
    "Availability": "",
    "Best OFFERER": "",
    "BEST PRICE": null,
    "CATEGORY": "Deutsch>Haushalt Kleingeräte>Haushalt",
    "EAN": " 5KSM125PSEOB",
    "ID": "<Artikel-iD>",
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
    "STATUS": "ON/NO_RESULT",
    "TOTAL PRICE": null
  }

Notes:

The following error messages occurs in different cases:

=over 

=item  "ERROR_EAN", -- missing ean in product

=item  "ON/MISSING_MANDATORIES", -- missing shop or seller_url

=item  "ON/NO_RESULT", --  there are no competitors and productfound

=item  "ON/OWN_PRODUCT_NOT_FOUND" -- competitors exists buttarget product notfound

=item  "ON/UNKNOWN_ERROR" -- data transfer error (proxy error,timeout error, bad response from marketplace, etc .. )

=back

=item C<delete_products( marketplace =E<gt> Str, ids =E<gt> Str|ArrayRef[Int] (string of comma separated list of ids or an arrayref of ids))>

Example:

  $api->delete_products(marketplace => 'idealo.de', ids => 11)
  $api->delete_products(marketplace => 'idealo.de', ids => '11,22,33,44');
  $api->delete_products(marketplace => 'idealo.de', ids => [11,22,33,44]);

Response:

  {"deleted": 252}

=item C<marketplace_settings( marketplace =E<gt> Str, url =E<gt> Str, repricing =E<gt> 'on'|'off', ean =E<gt> 'on'|'off')>

Example:

  $api->marketplace_settings(marketplace => 'idealo.de', url => 'foo.de', repricing => 'off');
  
=item C<get_repricing_settings( marketplace =E<gt> Str [, format =E<gt> 'json|csv', ids =E<gt> Str|ArrayRef[Int] (string of comma separated list of ids or an arrayref of ids), pformat_dec =E<gt> 1|2 (1 for integer format, 2 for decimal)])>

=item C<set_repricing_settings(marketplace =E<gt> Str, file =E<gt> Str (readable file path) [ ,separator =E<gt> 'comma|semicolon|tab', lineend =E<gt> 'win|unix'])>


=item

=back

=cut
