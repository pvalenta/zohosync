using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace ZohoSync
{
    /// <summary>
    /// zoho data reader
    /// </summary>
    internal class ZohoReader
    {
        /// <summary>
        /// login API 
        /// </summary>
        const string LOGIN_API = "https://accounts.zoho.com/apiauthtoken/nb/create?SCOPE=ZohoCRM/crmapi&EMAIL_ID={0}&PASSWORD={1}";

        /// <summary>
        /// table API
        /// </summary>
        const string TABLE_API = "https://crm.zoho.com/crm/private/xml/{0}/getRecords?newFormat=1&authtoken={1}&scope=crmapi&fromIndex={2}&toIndex={3}";

        /// <summary>
        /// get data
        /// </summary>
        public void GetData()
        {
            // authenticate
            var webClient = new WebClient();
            string response = webClient.DownloadString(string.Format(LOGIN_API, ConfigReader.ZohoLogin, ConfigReader.ZohoPassword));

            // split by lines
            var lines = response.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // auth token
            var token = (from l in lines
                         where l.StartsWith("AUTHTOKEN=")
                         select l.Split('=')[1]).FirstOrDefault();

            // let's suck data
            var tables = ConfigReader.ZohoTables;
            foreach (var t in tables)
            {
                // main xml
                XElement root = new XElement("response");

                for (int i = 0; i < 1000; i = i + 200)
                {

                    // request
                    string content = webClient.DownloadString(string.Format(TABLE_API, t, token, i, i + 199));

                    // parse it
                    XElement partRoot = XElement.Parse(content);
                    if (partRoot.Elements("nodata").Any()) break;
                    else if (partRoot.Elements("error").Any())
                    {
                        var error = partRoot.Elements("error").First();
                        error.Remove();
                        root.Add(error);
                        break;
                    }
                    else if (partRoot.Descendants(t).Any())
                    {
                        var result = partRoot.Descendants(t).First().Elements("row").ToList();
                        foreach (var r in result)
                        {
                            r.Remove();
                            root.Add(r);
                        }
                    }

                }

                // build path
                string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), t + ".xml");

                // save it
                root.Save(file);
            }
        }
    }
}
