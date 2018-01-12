﻿using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Xml;
using System.Xml.Linq;

using TumblThree.Domain;

namespace TumblThree.Applications.Services
{
    /// <summary>
    /// </summary>
    [Export(typeof(IApplicationUpdateService))]
    public class ApplicationUpdateService : IApplicationUpdateService
    {
        private readonly IShellService shellService;
        private string downloadLink;
        private string version;

        [ImportingConstructor]
        public ApplicationUpdateService(IShellService shellService)
        {
            this.shellService = shellService;
        }

        private HttpWebRequest CreateWebReqeust(string url)
        {
            HttpWebRequest request =
                WebRequest.Create(url)
                    as HttpWebRequest;
            request.Method = "GET";
            request.ProtocolVersion = HttpVersion.Version11;
            request.ContentType = "application/json";
            request.ServicePoint.Expect100Continue = false;
            request.UnsafeAuthenticatedConnectionSharing = true;
            request.UserAgent = ApplicationInfo.ProductName;
            request.KeepAlive = true;
            request.Pipelined = true;
            if (!string.IsNullOrEmpty(shellService.Settings.ProxyHost))
            {
                request.Proxy = new WebProxy(shellService.Settings.ProxyHost, int.Parse(shellService.Settings.ProxyPort));
            }
            else
            {
                request.Proxy = null;
            }
            return request;
        }

        public async Task<string> GetLatestReleaseFromServer()
        {
            version = null;
            downloadLink = null;
            try
            {
                HttpWebRequest request = CreateWebReqeust(@"https://api.github.com/repos/johanneszab/tumblthree/releases/latest");
                string result;
                using (HttpWebResponse resp = await request.GetResponseAsync() as HttpWebResponse)
                {
                    StreamReader reader =
                        new StreamReader(resp.GetResponseStream());
                    result = reader.ReadToEnd();
                }
                XmlDictionaryReader jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(result),
                    new System.Xml.XmlDictionaryReaderQuotas());
                XElement root = XElement.Load(jsonReader);
                version = root.Element("tag_name").Value;
                downloadLink = root.Element("assets").Element("item").Element("browser_download_url").Value;
            }
            catch (Exception exception)
            {
                Logger.Error(exception.ToString());
                return exception.Message;
            }
            return null;
        }

        public bool IsNewVersionAvailable()
        {
            try
            {
                Version newVersion = new Version(version.Substring(1));
                if (newVersion > new Version(ApplicationInfo.Version))
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.ToString());
            }
            return false;
        }

        public string GetNewAvailableVersion()
        {
            return version;
        }

        public Uri GetDownloadUri()
        {
            if (downloadLink == null)
            {
                return null;
            }
            return new Uri(downloadLink);
        }
    }
}
