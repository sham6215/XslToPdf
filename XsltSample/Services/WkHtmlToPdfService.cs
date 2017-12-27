using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WkHtmlToXSharp;

namespace XsltSample.Services
{
    /// <summary>
    /// The class utilize wkhtml wrapper.
    /// All not implemented methods and properties are declared fo interface purpose
    /// </summary>
    public class WkHtmlToPdfService
    {
        /// From WkHtmlToXSharp code: https://github.com/pruiz/WkHtmlToXSharp/blob/master/WkHtmlToXSharp/MultiplexingConverter.cs
        /// 
        /// XXX: We need to keep a converter instance alive during the whole application
        ///		lifetime due to some underlying's library bug by which re-initializing
        ///		the API after having deinitiaized it, causes all newlly rendered pdf
        ///		file to be corrupted. So we will keep this converter alive to avoid 
        ///		de-initialization until app's shutdown. (pruiz)
        ///		
        /// MultiplexingConverter is not used because the application can't exit after its using.
        private static WkHtmlToPdfConverter _instanse = null;


        #region Properties not implemented
        //
        // Summary:
        //     Custom WkHtmlToPdf global options
        public string CustomWkHtmlArgs { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Custom WkHtmlToPdf cover options (applied only if cover content is specified)
        public string CustomWkHtmlCoverArgs { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Custom WkHtmlToPdf page options
        public string CustomWkHtmlPageArgs { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Custom WkHtmlToPdf toc options (applied only if GenerateToc is true)
        public string CustomWkHtmlTocArgs { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Get or set maximum execution time for PDF generation process (by default is null
        //     that means no timeout)
        public TimeSpan? ExecutionTimeout { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Get or set TOC generation flag
        public bool GenerateToc { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Get or set option to generate grayscale PDF
        public bool Grayscale { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Get or set option to generate low quality PDF (shrink the result document space)
        public bool LowQuality { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Get or set PDF page height (in mm)
        public float? PageHeight { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Get or set PDF page width (in mm)
        public float? PageWidth { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Get or set path where WkHtmlToPdf tool is located
        //
        // Remarks:
        //     By default this property points to the folder where application assemblies are
        //     located. If WkHtmlToPdf tool files are not present PdfConverter expands them
        //     from DLL resources.
        public string PdfToolPath { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Suppress wkhtmltopdf debug/info log messages (by default is true)
        public bool Quiet { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        //
        // Summary:
        //     Get or set custom TOC header text (default: "Table of Contents")
        public string TocHeaderText { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        #endregion

        #region Properties
        private static long MarginLeftDefault => 20;
        private static long MarginTopDefault => 10;
        private static long MarginRightDefault => 10;
        private static long MarginBottomDefault => 10;

        /// <summary>
        /// Get or set zoom factor
        /// </summary>
        public double Zoom { get; set; } = 1.0;
        public int Dpi { get; set; } = 96;
        public float HeaderSpacing { get; set; } = 0;
        public float FooterSpacing { get; set; } = 0;

        /// <summary>
        /// Get or set custom page content HTML url or file path
        /// </summary>
        public string HtmlUrl { get; set; }
        
        /// <summary>
        /// Get or set custom page footer HTML url or file path
        /// </summary>
        public string FooterHtmlUrl { get; set; }
        
        /// <summary>
        /// Get or set custom page header HTML url or file path
        /// </summary>
        public string HeaderHtmlUrl { get; set; }
        
        /// <summary>
        /// Get or set PDF page margins (in mm)
        /// </summary>
        public long? MarginLeft { get; set; } = MarginLeftDefault;

        /// <summary>
        /// Get or set PDF page margins (in mm)
        /// </summary>
        public long? MarginTop { get; set; } = MarginTopDefault;

        /// <summary>
        /// Get or set PDF page margins (in mm)
        /// </summary>
        public long? MarginRight { get; set; } = MarginRightDefault;

        /// <summary>
        /// Get or set PDF page margins (in mm)
        /// </summary>
        public long? MarginBottom { get; set; } = MarginBottomDefault;

        /// <summary>
        /// Get or set PDF page orientation
        /// </summary>
        public PdfOrientation Orientation { get; set; } = PdfOrientation.Portrait;

        /// <summary>
        /// Get or set PDF page Size
        /// </summary>
        public PdfPageSize PageSize { get; set; } = PdfPageSize.A4;
        #endregion

        public WkHtmlToPdfService()
        {
            Init();
        }

        private void Init()
        {
            if (_instanse == null)
            {
                /// Copies wkhtmltox0.dll to the assembly directory
                WkHtmlToXLibrariesManager.Register(new Win32NativeBundle());
                _instanse = new WkHtmlToPdfConverter();
            }
        }

        /// <summary>
        /// Returned object must be disposed
        /// MultiplexingConverter should be used for multithreading 
        /// but and application doesn't exit after its using
        /// </summary>
        /// <returns></returns>
        private WkHtmlToPdfConverter GetConverter()
        {
            
            var conv = new WkHtmlToPdfConverter();
            conv.GlobalSettings.Dpi = Dpi;

            conv.GlobalSettings.Margin.Top = MarginTop?.ToString();
            conv.GlobalSettings.Margin.Bottom = MarginBottom?.ToString();
            conv.GlobalSettings.Margin.Left = MarginLeft?.ToString();
            conv.GlobalSettings.Margin.Left = MarginLeft?.ToString();
            conv.GlobalSettings.Orientation = Orientation;
            conv.GlobalSettings.Size.PageSize = PageSize;

            conv.ObjectSettings.Load.ZoomFactor = Zoom;
            conv.ObjectSettings.Header.Spacing = HeaderSpacing;
            conv.ObjectSettings.Header.HtmlUrl = HeaderHtmlUrl;
            conv.ObjectSettings.Page = HtmlUrl;

            conv.ObjectSettings.Footer.Spacing = FooterSpacing;
            conv.ObjectSettings.Footer.HtmlUrl = FooterHtmlUrl;
            conv.ObjectSettings.Web.EnableIntelligentShrinking = false;
            
            //conv.Begin += (s, e) => _Log.DebugFormat("Conversion begin, phase count: {0}", e.Value);
            //conv.Error += (s, e) => _Log.Error(e.Value);
            //conv.Warning += (s, e) => _Log.Warn(e.Value);
            //conv.PhaseChanged += (s, e) => _Log.InfoFormat("PhaseChanged: {0} - {1}", e.Value, e.Value2);
            //conv.ProgressChanged += (s, e) => _Log.InfoFormat("ProgressChanged: {0} - {1}", e.Value, e.Value2);
            //conv.Finished += (s, e) => _Log.InfoFormat("Finished: {0}", e.Value ? "success" : "failed!");

            return conv;
        }

        /// <summary>
        /// Generate PDF by specifed HTML content
        /// </summary>
        /// <param name="htmlContent">HTML content in UTF8 format</param>
        /// <returns></returns>
        public byte[] GeneratePdf(string htmlContent)
        {
            using (var converter = GetConverter())
            {
                return converter.Convert(htmlContent);
            }
        }

        /// <summary>
        /// Generates PDF from PageHtmlUrl source
        /// </summary>
        /// <returns>HTML content in UTF8 format</returns>
        public byte[] GeneratePdf()
        {
            using (var converter = GetConverter())
            {
                return converter.Convert();
            }
        }


        #region Methods Not Implemented
        //
        // Summary:
        //     Generate PDF by specfied HTML content and prepend cover page (useful with GenerateToc
        //     option)
        //
        // Parameters:
        //   htmlContent:
        //     HTML document
        //
        //   coverHtml:
        //     first page HTML
        //
        // Returns:
        //     PDF bytes
        public byte[] GeneratePdf(string htmlContent, string coverHtml)
        {
            throw new NotImplementedException();
        }

        //
        // Summary:
        //     Generate PDF by specfied HTML content and prepend cover page (useful with GenerateToc
        //     option)
        //
        // Parameters:
        //   htmlContent:
        //     HTML document
        //
        //   coverHtml:
        //     first page HTML
        //
        //   output:
        //     output stream for generated PDF
        public void GeneratePdf(string htmlContent, string coverHtml, Stream output)
        {
            throw new NotImplementedException();
        }

        //
        // Summary:
        //     Generate PDF by specfied HTML content and prepend cover page (useful with GenerateToc
        //     option)
        //
        // Parameters:
        //   htmlFilePath:
        //     path to HTML file or absolute URL
        //
        //   coverHtml:
        //     first page HTML (optional, can be null)
        //
        // Returns:
        //     PDF bytes
        public byte[] GeneratePdfFromFile(string htmlFilePath, string coverHtml)
        {
            throw new NotImplementedException();
        }

        //
        // Summary:
        //     Generate PDF by specfied HTML content and prepend cover page (useful with GenerateToc
        //     option)
        //
        // Parameters:
        //   htmlFilePath:
        //     path to HTML file or absolute URL
        //
        //   coverHtml:
        //     first page HTML (optional, can be null)
        //
        //   output:
        //     output stream for generated PDF
        public void GeneratePdfFromFile(string htmlFilePath, string coverHtml, Stream output)
        {
            throw new NotImplementedException();
        }

        //
        // Summary:
        //     Generate PDF by specfied HTML content and prepend cover page (useful with GenerateToc
        //     option)
        //
        // Parameters:
        //   htmlFilePath:
        //     path to HTML file or absolute URL
        //
        //   coverHtml:
        //     first page HTML (optional, can be null)
        //
        //   outputPdfFilePath:
        //     path to output PDF file (if file already exists it will be removed before PDF
        //     generation)
        public void GeneratePdfFromFile(string htmlFilePath, string coverHtml, string outputPdfFilePath)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
