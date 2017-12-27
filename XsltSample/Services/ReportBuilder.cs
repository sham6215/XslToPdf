using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace XsltSample.Services
{
    internal class ReportBuilder
    {
        public string FooterHtmlUrl { get; set; }
        public string HeaderHtmlUrl { get; set; }
        public float HeaderSpacing { get; set; }
        public float FooterSpacing { get; set; }
        private object _Data { get; set; }
        private string _XslPath { get; set; }
        public ReportBuilder(object data, string xslPath)
        {
            _Data = data;
            _XslPath = xslPath;
        }

        /// Returns UTF8 encoded XML
        public string GetXml()
        {
            XmlSerializer xsSubmit = new XmlSerializer(_Data.GetType());

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            settings.Encoding = Encoding.UTF8;

            using (var sww = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sww, settings))
            {
                xsSubmit.Serialize(writer, _Data);
                return sww.ToString();
            }
        }

        /// <summary>
        /// Returns byte[] representation of UTF8 encoded HTML
        /// </summary>
        /// <returns></returns>
        public byte[] GetHtml()
        {
            var xml = GetXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlToHtmlService svc = new XmlToHtmlService(doc, _XslPath);
            return svc.GetHtml();
        }

        public string GetHtmlString()
        {
            return Encoding.UTF8.GetString(GetHtml());
        }

        public void SavePdf(string pdfPath)
        {
            var html = GetHtmlString();
            var pdfSvc = GetConverterService();
            var pdfData = pdfSvc.GeneratePdf(html);
            File.WriteAllBytes(pdfPath, pdfData);
        }

        private WkHtmlToPdfService GetConverterService()
        {
            var svc = new WkHtmlToPdfService();
            if (!string.IsNullOrEmpty(FooterHtmlUrl))
            {
                svc.HeaderHtmlUrl = FooterHtmlUrl;
                svc.HeaderSpacing = HeaderSpacing;
                svc.FooterHtmlUrl = FooterHtmlUrl;
                svc.FooterSpacing = FooterSpacing;
            }
            svc.HeaderHtmlUrl = HeaderHtmlUrl;
            return svc;
        }
    }
}
