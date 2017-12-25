using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XsltSample.Services.Pdf
{
    public class HtmlToPdfService
    {
        public void HtmlToPdf(string html, string outPath, 
            bool defaultFoterLeft = true, string footerLeft = null, string headerRight = null)
        {
            string pdfFooterLeft = defaultFoterLeft
                ? $"Reproduced from Admiralty digital Notices to Mariners by permission of the Controller of Her Majesty’s Stationery Office and the UK \nHydrographic Office \nHO2448/150610/01.\n© British Crown Copyright {DateTime.Today.Year}"
                : (footerLeft ?? string.Empty);

            HtmlToPdfConverter htmlToPdf = new HtmlToPdfConverter();
            //htmlToPdf.PageFooterHtml = pdfFooter;

            htmlToPdf.Margins = new PageMargins()
            {
                Bottom = 10,
                Top = 10,
                Left = 5,
                Right = 10
            };

            //htmlToPdf.PageHeaderHtml = File.ReadAllText("Resources/header.html");
            //htmlToPdf.PageFooterHtml = File.ReadAllText("Resources/footer.html");

            htmlToPdf.CustomWkHtmlArgs =
                $"--dpi 96 " +
                $"--encoding utf-8 --disable-smart-shrinking ";
            /*
             * $"--header-right \"[page] of [toPage]\" " +
                $"--header-left \"{headerRight ?? string.Empty}\" " +
                $"--footer-left \"{pdfFooterLeft}\" " +
                $"--footer-spacing 15 " +
                $"--header-line " +
                $"--footer-font-name \"Arial Unicode MS\" --footer-font-size 8 " +
                $"--header-font-name \"Arial Unicode MS\" --footer-font-size 8";
             */

            var pdfBytes = htmlToPdf.GeneratePdf(html);
            File.WriteAllBytes(outPath, pdfBytes);
        }

        public void HtmlToPdf(byte[] html, string outPath, 
            bool defaultFoter = true, string footer = null, string headerRight = null)
        {
            using (MemoryStream ms = new MemoryStream())
            using (StreamReader sr = new StreamReader(ms))
            {
                HtmlToPdf(sr.CurrentEncoding.GetString(html), outPath, defaultFoter, footer, headerRight);
            }
        }
    }
}
