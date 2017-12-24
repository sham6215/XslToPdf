using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Xsl;
using XsltSample.Models;
using XsltSample.Services;

namespace XsltSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            

            List<Enc> list = new List<Enc>();
            for (int i = 1; i <= 10; ++i)
            {
                list.Add(
                    new Enc() {
                        Name = $"Enc Name {i}",
                        Description = $"Enc Description {i}",
                        ExpiryDate = DateTime.Now.AddDays(-i),
                        PublicationDate = DateTime.Now.AddDays(-10 - i),
                        Id = i,
                        Price = i * 10 + i%3 });
            }

            ReportEnc re = new ReportEnc() {
                CreationDate = DateTime.Now,
                Title = "AVCS Cells Report",
                Encs = list
            };

            ReportBuilder bld = new ReportBuilder(re, "");
            var xml = bld.GetXml();

            string xmlPath = @"N:\work\projects\GitHub\XsltSample\XsltSample\Resources\result.xml";
            string htmlPath = @"N:\work\projects\GitHub\XsltSample\XsltSample\Resources\result.html";
            string xslPath = @"N:\work\projects\GitHub\XsltSample\XsltSample\Xslt\EncReport.xslt";
            //File.WriteAllText(xmlPath, xml);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlToHtmlService svc = new XmlToHtmlService(doc, xslPath);
            var data = svc.GetHtml();
            File.WriteAllBytes(htmlPath, data);

            //var html = Encoding.ASCII.GetString(data);

            //File.WriteAllText(htmlPath, html);

            InitializeComponent();
        }
    }
}
