from pyvalid.validators import is_validator
import os
class Validator(object):
    @classmethod
    @is_validator
    def file_format(cls, val):
        return isinstance(val, str) and val in ('csv', 'json')

    @classmethod
    @is_validator
    def price_format(cls, val):
        return isinstance(val, int) and val in (1, 2)

    @classmethod
    @is_validator
    def separator(cls, val):
        return isinstance(val, str) and val in ('comma', 'semicolon', 'tab')

    @classmethod
    @is_validator
    def line_end(cls, val):
        return isinstance(val, str) and val in ('win', 'unix')

    @classmethod
    @is_validator
    def upload(cls, val):
        return isinstance(val, str) and os.path.isfile(val) and os.path.exists(val) \
            and os.access(file, os.R_OK)

    @classmethod
    @is_validator
    def sort_type(cls, val):
        return isinstance(val, str) and val in ('total_price', 'price',
                                                'shipping_costs', 'ranking')

    @classmethod
    @is_validator
    def repricing(cls, val):
        return isinstance(val, str) and val in ('on', 'off')
