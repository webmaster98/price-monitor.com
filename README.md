# price-monitor.com

Welcome to price monitor the Reprice Robot’s API
Specification!
price monitor the Reprice Robot provides an Application Programming Interface (API) to enable seamless integration
of price monitor into any shop and/or warehouse computer system. This document describes the interface and provides
exemplary code snippets for how to use it. The API definition is in subject to future changes. For comments, bug
reports, change or feature requests, please refer to https://price-monitor.freshdesk.com/support/tickets/new
or for any samle code issue at https://github.com/webmaster98/price-monitor.com/issues


# General Information
The API is based on the paradigm of REST (Representational State Transfer) service models
REST Service Calls to BENY
Every call to a function of the  price monitor API is being done by GET and POST methods of the HTTP protocol. The
general template of a URL for service calls to  price monitor looks like the following:
<b>https://price-monitor.com/api/prm/login/<api-key>/<func>[?arg0=val0[&arg1=val1[&...]]]</b>
