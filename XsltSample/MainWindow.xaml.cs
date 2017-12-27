using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml;
using WkHtmlToXSharp;
using XsltSample.Models;
using XsltSample.Services;
using XsltSample.Services.Pdf;

namespace XsltSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //public string TestHtml => @"N:\work\projects\GitHub\XsltSample\XsltSample\Resources\report.html";
        public string TestHtml => @"N:\work\projects\GitHub\XsltSample\XsltSample\Resources\reportTable.html";
        private int _itemsCount = 50;
        public int ItemsCount
        {
            get
            {
                return _itemsCount;
            }
            set
            {
                if (_itemsCount != value)
                {
                    _itemsCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = $"report_{DateTime.Now.ToString("hhmmss")}.pdf",
                Filter = "PDF file (*.pdf)|*.pdf",
                Title = "Save Report File",
                InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
            };

            if (dlg.ShowDialog() == true)
            {
                CreateReport(dlg.FileName);
            }
        }

        public void CreateReport(string pdfPath)
        {
            string xslPath = @"N:\work\projects\GitHub\XsltSample\XsltSample\Xslt\EncReport.xslt";
            ReportEnc re = GenerateReportData();
            ReportBuilder bld = new ReportBuilder(re, xslPath);

            string htmlPath = @"N:\work\projects\GitHub\XsltSample\XsltSample\Resources\report.html";
            File.WriteAllBytes(htmlPath, bld.GetHtml());

            bld.SavePdf(pdfPath);
            //string param = $"/select, \"{pdfPath}\"";
            //Process.Start("explorer.exe", param);
            Process.Start($"\"{pdfPath}\"");
        }

        private ReportEnc GenerateReportData()
        {
            List<Enc> list = new List<Enc>();
            for (int i = 1; i <= ItemsCount; ++i)
            {
                string description = string.Concat(Enumerable.Repeat("Enc Description ", (i % 4) + 1));
                list.Add(
                    new Enc()
                    {
                        Name = $"Enc Name {i}",
                        Description = $"{description}{i}",
                        ExpiryDate = DateTime.Now.AddDays(-i),
                        PublicationDate = DateTime.Now.AddDays(-10 - i),
                        Id = i,
                        Price = i * 10 + i % 3
                    });
            }

            ReportEnc re = new ReportEnc()
            {
                CreationDate = DateTime.Now,
                Title = "AVCS Cells Report",
                Encs = list
            };

            return re;
        }

        private void TestConversion()
        {
            var ignore = Environment.GetEnvironmentVariable("WKHTMLTOXSHARP_NOBUNDLES");
            WkHtmlToXLibrariesManager.Register(new Win32NativeBundle());
            using (var wk = new MultiplexingConverter())
            {
                wk.GlobalSettings.Margin.Top = "0cm";
                wk.GlobalSettings.Margin.Bottom = "0cm";
                wk.GlobalSettings.Margin.Left = "0cm";
                wk.GlobalSettings.Margin.Right = "0cm";
                //wk.GlobalSettings.Out = @"c:\temp\tmp.pdf";

                wk.ObjectSettings.Web.EnablePlugins = false;
                wk.ObjectSettings.Web.EnableJavascript = false;
                wk.ObjectSettings.Page = TestHtml;
                //wk.ObjectSettings.Page = "http://doc.trolltech.com/4.\6/qstring.html";
                wk.ObjectSettings.Load.Proxy = "none";

                var tmp = wk.Convert();
                var tempfname = Path.GetTempFileName();
                var tempPath = Path.Combine(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),
                    $"report_{DateTime.Now.ToString("hhmmss")}.pdf");
                File.WriteAllBytes(tempPath, tmp);
            }

        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            TestConversion();
        }
    }
}
