using System;
using Microsoft.AspNetCore.Mvc;
using PriceCalc.Models;
using System.Linq;
using System.Xml;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Text;

namespace PartyInvites.Controllers {

    public class HomeController : Controller {

        public ViewResult Index() {
            return View();
        }

        [HttpPost]
        public ViewResult PriceTable(MyModel mymodel) {
            if (ModelState.IsValid) {
                mymodel.Calc = mymodel.Price * 0.75;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1251");
                XmlTextReader reader = new XmlTextReader("http://www.cbr.ru/scripts/XML_daily.asp");
                string USDXml = "";
                string EuroXML = "";
                string GrivnaXML = "";
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "Valute")
                            {
                                if (reader.HasAttributes)
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name == "ID")
                                        {
                                            if (reader.Value == "R01235")
                                            {
                                                reader.MoveToElement();
                                                USDXml = reader.ReadOuterXml();
                                                XmlDocument usdXmlDocument = new XmlDocument();
                                                usdXmlDocument.LoadXml(USDXml);
                                                XmlNode xmlNode = usdXmlDocument.SelectSingleNode("Valute/Value");
                                                mymodel.Dollar = Math.Round(((mymodel.Price / Convert.ToDouble(xmlNode.InnerText.Replace(',', '.'))) * 0.75), 2);
                                            }
                                        }

                                        if (reader.Name == "ID")
                                        {
                                            if (reader.Value == "R01239")
                                            {
                                                reader.MoveToElement();
                                                EuroXML = reader.ReadOuterXml();
                                                XmlDocument euroXmlDocument = new XmlDocument();
                                                euroXmlDocument.LoadXml(EuroXML);
                                                XmlNode xmlNode = euroXmlDocument.SelectSingleNode("Valute/Value");
                                                mymodel.Euro = Math.Round(((mymodel.Price / Convert.ToDouble(xmlNode.InnerText.Replace(',', '.'))) * 0.75), 2);
                                            }
                                        }

                                        if (reader.Name == "ID")
                                        {
                                            if (reader.Value == "R01720")
                                            {
                                                reader.MoveToElement();
                                                GrivnaXML = reader.ReadOuterXml();
                                                XmlDocument grivnaXmlDocument = new XmlDocument();
                                                grivnaXmlDocument.LoadXml(GrivnaXML);
                                                XmlNode xmlNode = grivnaXmlDocument.SelectSingleNode("Valute/Value");
                                                mymodel.Grivna = Math.Round(((mymodel.Price / (Convert.ToDouble(xmlNode.InnerText.Replace(',', '.'))/10)) * 0.75), 2);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                return View(mymodel);
            } else {
                return View("Index");
            }
        }
    }
}
