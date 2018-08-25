import sys
import os
import requests

try:  #py3
    from urllib.parse import urlparse, urljoin
except ImportError:
    from urlparse import urlparse, urljoin
from pricemonitor.types import Validator
from pyvalid import accepts


class Api():
    BASE_URL = 'https://price-monitor.com/api/prm/login/'

    def __init__(self, api_key=None):
        if not api_key:
            raise Exception('Missing api_key')
        self.api_key = api_key

    def _uri_for(self, path):
        return urljoin(urljoin(Api.BASE_URL, self.api_key + '/'), path)

    @accepts(
        object,
        marketplace=str,
        format=Validator.file_format,
        exportall=bool,
        id=int,
        test=bool,
        pformat_dec=Validator.price_format)
    def get_price_updates(self, **kwargs):
        assert kwargs.get('marketplace') is not None

        return requests.get(
            self._uri_for('get_price_updates'), params=kwargs, verify=False)

    @accepts(
        object,
        marketplace=str,
        file=Validator.upload,
        separator=Validator.separator,
        lineend=Validator.line_end,
        keepold=bool,
        cleanold=bool,
        test=bool)
    def import_products(self, **kwargs):
        assert kwargs.get('marketplace') is not None
        assert kwargs.get('file') is not None
        with open(kwargs.get('file'), 'rb') as fh:
            return requests.post(
                self._uri_for('import_products'),
                headers={'Content-type': 'text/csv'},
                params=kwargs,
                data=fh,
                verify=False)

    @accepts(
        object,
        marketplace=str,
        format=Validator.file_format,
        exportall=bool,
        sortby=Validator.sort_type,
        offeridx=int,
        ids=Validator.ids,
        pformat_dec=Validator.price_format)
    def export(self, **kwargs):
        assert kwargs.get('marketplace') is not None
        ids = kwargs.get('ids')
        if ids is not None and isinstance(ids, list):
            kwargs['ids'] =  ','.join(ids)
            
        return requests.get(
            self._uri_for('export'), params=kwargs, verify=False)

    @accepts(object, marketplace=str, format=Validator.file_format)
    def get_errors(self, **kwargs):
        assert kwargs.get('marketplace') is not None
        return requests.get(
            self._uri_for('get_errors'), params=kwargs, verify=False)

    @accepts(object, marketplace=str, ids=Validator.ids,)
    def delete_products(self, **kwargs):
        assert kwargs.get('marketplace') is not None
        assert kwargs.get('ids') is not None
        ids = kwargs.get('ids')
        if isinstance(ids, list):
            kwargs['ids'] =  ','.join(ids)

        return requests.get(
            self._uri_for('get_errors'), params=kwargs, verify=False)

    @accepts(
        object,
        marketplace=str,
        url=str,
        repricing=Validator.repricing,
        ean=str)
    def marketplace_settings(self, **kwargs):
        assert kwargs.get('marketplace') is not None
        assert kwargs.get('url') is not None
        assert kwargs.get('repricing') is not None
        assert kwargs.get('ean') is not None
        return requests.get(
            self._uri_for('marketplace_settings'), params=kwargs, verify=False)

    @accepts(
        object,
        marketplace=str,
        format=Validator.file_format,
        ids=Validator.ids,
        pformat_dec=Validator.price_format)
    def get_reprice_settings(self, **kwargs):
        assert kwargs.get('marketplace') is not None
        ids = kwargs.get('ids')
        if ids is not None and isinstance(ids, list):
            kwargs['ids'] =  ','.join(ids)

        
        return requests.get(
            self._uri_for('reprice_settings'), params=kwargs, verify=False)

    @accepts(
        object,
        marketplace=str,
        separator=Validator.separator,
        lineend=Validator.line_end)
    def set_reprice_settings(self, **kwargs):
        assert kwargs.get('marketplace') is not None
        return requests.post(
            self._uri_for('reprice_settings'), params=kwargs, verify=False)
