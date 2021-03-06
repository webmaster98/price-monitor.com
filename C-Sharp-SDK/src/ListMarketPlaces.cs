﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorSDK
{
    public class ListMarketplaces
    {
        private static List<String> listMarkets;

        static ListMarketplaces()
        {
            listMarkets = new List<String>();

            String[] markets = new String[]
            {
                "123preis.com",
                "4insiders.de",
                "4phones.de",
                "abebooks.de",
                "amazon.ca",
                "amazon.cn",
                "amazon.co.uk",
                "amazon.com",
                "amazon.de",
                "amazon.fr",
                "amazon.it",
                "amazon.jp",
                "angebot-info.de",
                "antag.de",
                "apomio.de",
                "augencommunity.de",
                "auspreiser.de",
                "auvito.de",
                "auxion.de",
                "bebero.de",
                "billig-fotos-entwickeln.de",
                "billige-fotos.de",
                "billiger-reisen.de",
                "billiger-telefonieren.de",
                "billiger.de",
                "billigmed.de",
                "bookbutler.de",
                "buchpreis24.de",
                "buecher24.de",
                "buecher25.de",
                "buycentral.de",
                "cd-preis.de",
                "check24.de",
                "choozen.de",
                "ciao.de",
                "cooceo.de",
                "decido.de",
                "der-preis-fuchs.de",
                "der-wein-preisvergleich.de",
                "dooyoo.de",
                "dvd-25.de",
                "dvd-palace.de",
                "dvd-suche.de",
                "dvdeal.ch",
                "dvdiggle.de",
                "ebay.at",
                "ebay.ca",
                "ebay.ch",
                "ebay.co.uk",
                "ebay.com",
                "ebay.de",
                "ebay.es",
                "ebay.fr",
                "einfachbilliger.com",
                "einkaufswelt.t-online.de",
                "elektronischer-markt.de",
                "eurobuch.com",
                "evendi.de",
                "fashion.de",
                "filmundo.de",
                "findashop.de",
                "finder.ch",
                "findmybook.de",
                "foto-preise.de",
                "fotodienste.eu",
                "fotopreise.com",
                "fotoservice-vergleich.info",
                "froogle.de",
                "geizhals.at",
                "geizhals.de",
                "geizhals.eu",
                "geizkragen.de",
                "getprice.de",
                "gimahhot.de",
                "gimahot.de",
                "google.ch",
                "google.de",
                "gooster.de",
                "guenstig.de",
                "guenstiger.de",
                "hardwareschotte.de",
                "hartwarehunter.de",
                "heise.de/preisvergleich/",
                "hitmeister.de",
                "hood.de",
                "idealo.at",
                "idealo.co.uk",
                "idealo.de",
                "idealo.fr",
                "idealo.it",
                "ihrpreisvergleich.de",
                "insidepda.de",
                "auflux.de",
                "kelkoo.de",
                "kontaktlinsen-berater.de",
                "kontaktlinsen-preisvergleich.de",
                "ladenzeile.de",
                "letsbuyit.de",
                "livesuche.de",
                "mediensuchmaschine.de",
                "medikamentepreisvergleich.de",
                "medikompass.de",
                "medipreis.de",
                "medizinfuchs.de",
                "medpreis.de",
                "medvergleich.de",
                "meinpaket.de",
                "meinpreisvergleich.com",
                "mercateo.de",
                "meta-preisvergleich.de",
                "metabizz.de",
                "metakauf.de",
                "metapreis.de",
                "mibaby.de",
                "milando.de",
                "mistershoplister.de",
                "moebel.de",
                "motoso.de",
                "mysport.de",
                "nextag.de",
                "nice-prices.de",
                "outstore.de",
                "pikengo.de",
                "pixmania.de",
                "preis.de",
                "preisauskunft.de",
                "preisgalaxy.de",
                "preisomat.com",
                "preisroboter.de",
                "preissearch.de",
                "preissuchmaschine.at",
                "preissuchmaschine.ch",
                "preissuchmaschine.de",
                "preissuchmaschine.hu",
                "preissuchmaschine.pl",
                "preistester.de",
                "preistrend.de",
                "preisuma.de",
                "preisvergleich-city.de",
                "preisvergleich.at",
                "preisvergleich.ch",
                "preisvergleich.ch",
                "preisvergleich.de",
                "preisvergleich.eu",
                "preisvergleich.me",
                "preisvergleich.org",
                "preisvergleich.web.de",
                "preisvergleichsservice.de",
                "pricerunner.de",
                "produkt-detektiv.de",
                "rakuten.de",
                "reifensuchmaschine.de",
                "reitsport-preisvergleich.com",
                "ricardo.ch",
                "rockbottom.de",
                "sat-bay.de",
                "schnaeppchenjagd.de",
                "schottenland.de",
                "secoby.de",
                "shop-netz.de",
                "shop-vergleicher.de",
                "shop24.de",
                "shop4.eu",
                "shopmania.de",
                "shopping-profis.de",
                "shopping.at",
                "shopping.ch",
                "shopping.com",
                "shopping24.de",
                "shopwahl.de",
                "shopyoo.de",
                "shopzilla.de",
                "smartshopping.de",
                "smatch.com",
                "sparmedo.de",
                "stylight.de",
                "teltarif.de",
                "testberichte.de",
                "testsieger.de",
                "toppreise.ch",
                "toptarif.de",
                "traveljungle.com",
                "twenga.de",
                "vergleichbar24.de",
                "verivox.de",
                "webtradecenter.de",
                "wein-plus.de",
                "wein.cc",
                "weinevergleichen.de",
                "weinlupe.de",
                "wine-searcher.com",
                "wir-lieben-preise.de",
                "wowowo.de",
                "xdial.de",
                "yapii.de",
                "yatego.de",
                "yopi.de",
                "zdnet.de"
            };

            listMarkets.AddRange(markets);

        }


        public static List<String> GetListMarkets()
        {
            return listMarkets;
        }

        public static bool IsValidMarketplace(String marketplace)
        {

            if (IsEmptyMarketPlace(marketplace))
            {
                return false;
            }

            return listMarkets.Contains(marketplace);
        }

        private static bool IsEmptyMarketPlace(String marketplace)
        {
            return marketplace == null || marketplace.Length == 0;
        }
    }
}

