using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using XsltSample.Services.Pdf;

namespace XsltSample.Services
{
    class ReportBuilder
    {
        private object _Data { get; set; }
        private string _XslPath { get; set; }
        public ReportBuilder(object data, string xslPath)
        {
            _Data = data;
            _XslPath = xslPath;
        }

        public string GetXml()
        {
            XmlSerializer xsSubmit = new XmlSerializer(_Data.GetType());

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var sww = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sww, settings))
            {
                xsSubmit.Serialize(writer, _Data);
                return sww.ToString();
            }
        }

        public byte[] GetHtml()
        {
            var xml = GetXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlToHtmlService svc = new XmlToHtmlService(doc, _XslPath);
            return svc.GetHtml();
        }

        public void SavePdf(string pdfPath)
        {
            var html = GetHtml();
            HtmlToPdfService pdfSvc = new HtmlToPdfService();
            pdfSvc.HtmlToPdf(html, pdfPath);
        }
    }
}
