package monitorsdk;

import javax.net.ssl.*;

import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;

public class CertificateUtils
{


    static SSLSocketFactory makeSSLSocketFactoryTrustAll() throws NoSuchAlgorithmException, KeyManagementException
    {
        SSLContext context = SSLContext.getInstance("TLS");
        context.init(null, new TrustManager[]{trustManager()}, null);
        return context.getSocketFactory();

    }


    private static X509TrustManager trustManager()
    {
        return new X509TrustManager()
        {

            @Override
            public void checkClientTrusted(X509Certificate[] x509Certificates, String s) throws CertificateException
            {

            }

            @Override
            public void checkServerTrusted(X509Certificate[] x509Certificates, String s) throws CertificateException
            {

            }

            @Override
            public X509Certificate[] getAcceptedIssuers()
            {
                return new X509Certificate[0];
            }
        };
    }

    public static HostnameVerifier makeHostnameVerifier()
    {
        return new HostnameVerifier()
        {
            @Override
            public boolean verify(String s, SSLSession sslSession)
            {
                return true;
            }
        };
    }
}
