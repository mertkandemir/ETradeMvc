using System;
using System.Web.Mvc;
using System.Xml;

namespace ETradeMvcWebUI.Controllers
{
    public class KurController : Controller
    {
        private string usd;

        public KurController()
        {
            string exchangeRate = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(exchangeRate);
            usd = DateTime.Now.ToShortDateString() + " Tarihli Dolar Kuru: " + xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod ='USD']/BanknoteSelling").InnerXml;
        }

        // GET: Kur
        public ContentResult GetContent()
        {
            return Content("<b><i>" + usd + "</i></b>");
        }

        public JavaScriptResult GetJavaScript()
        {
            return JavaScript("alert('" + usd + "');");
        }

        public EmptyResult GetEmpty()
        {
            return new EmptyResult();
        }

        public RedirectResult GetRedirect()
        {
            return Redirect("http://www.tcmb.gov.tr/kurlar/today.xml");
        }
    }
}