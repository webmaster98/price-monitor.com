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
        """Perform GET request to get price updates.

        Parameters
        ----------
        marketplace : str
            Marketplace name. ex: `idealo.de`, `google.de`
        format : { 'json', 'csv'} , optional
            Output file format
        exportall: bool, optional
            Get all products or product with a price update
        id : int, optional
            Specifying product ID
        test: bool, optional
            Get random prices
        pformat_dec: { 1, 2 }, optional
            Price format, integer (1) or decimal (2)
        
        Returns
        -------
        requests.models.Response
        """
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
        """Perform POST request to update products.

        Parameters
        ----------
        marketplace : str
            Marketplace name. ex: `idealo.de`, `google.de`
        file : str
            Path for upload file, must be readable
        separator : { 'comma', 'semicolon', 'tab' }, optional
            Get all products or product with a price update
        lineend : { 'win', 'unix'} , optional
            Line end format, windows \r\n (win) or unix \n (unix)
        keepold: bool, optional
            Deletes old products from database
        cleanold: bool, optional
            Deletes products not included in csv file from database
        test: bool, optional
            Data won't be saved to database
        
        Returns
        -------
        requests.models.Response
        """        
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
        """Perform GET request to get product offers.

        Parameters
        ----------
        marketplace : str
            Marketplace name. ex: `idealo.de`, `google.de`
        format : { 'json', 'csv'} , optional
            Output file format
        exportall: bool, optional
            Get all products or product with a price update
        sortby:  { 'total_price', 'price', 'shipping_costs', 'ranking'}, optional
            Sort by specified fields
        offeridx : int, optional
            Specifying offer ID
        ids: str or array, optional
            Comma separated list if ids or array of ids 
        pformat_dec: { 1, 2 }, optional
            Price format, integer (1) or decimal (2)
        
        Returns
        -------
        requests.models.Response
        """        
        
        assert kwargs.get('marketplace') is not None
        ids = kwargs.get('ids')
        if ids is not None and isinstance(ids, list):
            kwargs['ids'] =  ','.join(ids)
            
        return requests.get(
            self._uri_for('export'), params=kwargs, verify=False)

    @accepts(object, marketplace=str, format=Validator.file_format)
    def get_errors(self, **kwargs):
        """Perform GET request to get products with an error status.

        Parameters
        ----------
        marketplace : str
            Marketplace name. ex: `idealo.de`, `google.de`
        format : {'json', 'csv'}, optional
            Output file format
        
        Returns
        -------
        requests.models.Response
        """        

        assert kwargs.get('marketplace') is not None
        return requests.get(
            self._uri_for('get_errors'), params=kwargs, verify=False)

    @accepts(object, marketplace=str, ids=Validator.ids,)
    def delete_products(self, **kwargs):
        """Perform GET request to delete products.

        Parameters
        ----------
        marketplace : str
            Marketplace name. ex: `idealo.de`, `google.de`
        ids: str or array
            String with comma separated list if ids or array of ids 
        
        Returns
        -------
        requests.models.Response
        """        

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
        ean=Validator.ean)
    def marketplace_settings(self, **kwargs):
        """Perform GET request to delete products.

        Parameters
        ----------
        marketplace : str
            Marketplace name. ex: `idealo.de`, `google.de`
        url: str
            Sets default URL for marketplace
        repricing: {'on', 'off'}
            Enable/Disable repricing. Default `on`
        ean: {'on', 'off'}
            Default is `on`
        
        Returns
        -------
        requests.models.Response
        """        
        
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
        """Perform GET request to get reprice settings.

        Parameters
        ----------
        marketplace : str
            Marketplace name. ex: `idealo.de`, `google.de`
        format : {'json', 'csv'}, optional
            Output file format
        ids: str or array
            String with comma separated list if ids or array of ids 
        pformat_dec: { 1, 2 }, optional
            Price format, integer (1) or decimal (2)
        
        
        Returns
        -------
        requests.models.Response
        """        
        assert kwargs.get('marketplace') is not None
        ids = kwargs.get('ids')
        if ids is not None and isinstance(ids, list):
            kwargs['ids'] =  ','.join(ids)
        
        return requests.get(
            self._uri_for('reprice_settings'), params=kwargs, verify=False)

    @accepts(
        object,
        marketplace=str,
        file=Validator.upload,
        separator=Validator.separator,
        lineend=Validator.line_end)
    def set_reprice_settings(self, **kwargs):
        """Perform POST request to set reprice settings.

        Parameters
        ----------
        marketplace : str
            Marketplace name. ex: `idealo.de`, `google.de`
        file : str
            Path for CSV filename containing reprice settings
        separator : { 'comma', 'semicolon', 'tab' }, optional
            Get all products or product with a price update
        lineend : { 'win', 'unix'} , optional
            Line end format, windows \r\n (win) or unix \n (unix)
        
        
        Returns
        -------
        requests.models.Response
        """        

        assert kwargs.get('marketplace') is not None
        assert kwargs.get('file') is not None        
        return requests.post(
            self._uri_for('reprice_settings'), params=kwargs, verify=False)
