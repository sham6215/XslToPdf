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

namespace XsltSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string ApplicationDirectory => Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        public string XslPath => Path.Combine(ApplicationDirectory, @"Resources\EncReport.xslt");
        public string HeaderPath => Path.Combine(ApplicationDirectory, @"Resources\header.html");
        public string FooterPath => Path.Combine(ApplicationDirectory, @"Resources\footer.html");
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
                InitialDirectory = ApplicationDirectory
            };

            if (dlg.ShowDialog() == true)
            {
                CreateReport(dlg.FileName);
            }
        }

        public void CreateReport(string pdfPath)
        {
            string xslPath = XslPath;
            ReportEnc re = GenerateReportData();
            ReportBuilder bld = new ReportBuilder(re, xslPath);
            bld.HeaderHtmlUrl = HeaderPath;
            bld.HeaderSpacing = 1;
            bld.FooterHtmlUrl = FooterPath;
            bld.FooterSpacing = 1;
            bld.SavePdf(pdfPath);
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
    }
}
