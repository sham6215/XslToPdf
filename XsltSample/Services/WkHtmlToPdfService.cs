using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WkHtmlToXSharp;

namespace XsltSample.Services
{
    public class WkHtmlToPdfService
    {
        private MultiplexingConverter _GetConverter()
        {
            var obj = new MultiplexingConverter();
            //obj.Begin += (s, e) => _Log.DebugFormat("Conversion begin, phase count: {0}", e.Value);
            //obj.Error += (s, e) => _Log.Error(e.Value);
            //obj.Warning += (s, e) => _Log.Warn(e.Value);
            //obj.PhaseChanged += (s, e) => _Log.InfoFormat("PhaseChanged: {0} - {1}", e.Value, e.Value2);
            //obj.ProgressChanged += (s, e) => _Log.InfoFormat("ProgressChanged: {0} - {1}", e.Value, e.Value2);
            //obj.Finished += (s, e) => _Log.InfoFormat("Finished: {0}", e.Value ? "success" : "failed!");
            return obj;
        }
    }
}
