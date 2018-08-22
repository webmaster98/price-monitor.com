package PriceMonitor::API;
use Moo;
use strictures 2;
use namespace::clean;
use feature 'state';

use LWP::UserAgent;
use constant BASE_URL => q{https://price-monitor.com/api/prm/login/};

use Params::ValidationCompiler qw(validation_for);
use Types::Standard qw( Int Str Bool ArrayRef);
use PriceMonitor::Types qw(FileFormat PriceFormat
  Separator LineEnd Upload
  SortType Repricing
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
      ids         => {type => ArrayRef [Int], optional => 1},
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
    params => {marketplace => {type => Str}, ids => {type => ArrayRef [Int]},});
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
      ean         => {type => Str}
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
      ids         => {type => ArrayRef [Int], optional => 1},
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
      separator   => {type => Separator, optional => 1},
      lineend     => {type => LineEnd, optional => 1},
    }
  );
  my (%args) = $v->(@_);
  $self->_make_request(POST $self->_uri_for(reprice_settings => \%args));
  
}

sub _make_request {
  my ($self, $req) = @_;
  warn $req->as_string;
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
