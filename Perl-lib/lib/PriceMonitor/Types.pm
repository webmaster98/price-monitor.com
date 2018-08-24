package PriceMonitor::Types;

use Type::Library -base;
use Type::Utils -all;

BEGIN { extends "Types::Standard" }

declare FileFormat, as Enum [qw(json csv)],
  message {qq{Should be "json" or "csv" not "$_"}};

declare PriceFormat, as Enum [qw(1 2)],
  message {'Price format should be 1 (integer) or 2 (decimal)'};

declare Separator, as Enum [qw(comma semicolon tab)],
  message {'Separator should be one of: comma, semicolon or tab'};

declare LineEnd, as Enum [qw(win unix)],
  message {'Separator should be: win (windows \r\n) or unix (unix \n)'};

declare Upload, as Str, where { -f && -r },
  message {"$_ does not exist or it's not readable"};

declare SortType, as Enum [qw(total_price price shipping_costs ranking)],
  message {
  "Sort type should be one of: total_price or price or shipping_costs or ranking"
  };

declare Repricing, as Enum [qw(on off)],
  message {'Repricing should be either "on" or "off"'};

declare Ids, as Str, where {/^\d+(?:,\d+)*$/};

coerce Ids, from ArrayRef [Int], via { join(q{,}, @$_) };

1;
