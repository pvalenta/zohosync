
namespace ZohoSync
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml.Linq;

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
        /// <returns>Xml Element with all data</returns>
        public XElement GetData()
        {
            // write to console
            Console.Write("Zoho: authenticate - ");

            // authenticate
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string response = webClient.DownloadString(string.Format(LOGIN_API, ConfigReader.ZohoLogin, ConfigReader.ZohoPassword));

            // split by lines
            var lines = response.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // auth token
            var token = (from l in lines
                         where l.StartsWith("AUTHTOKEN=")
                         select l.Split('=')[1]).FirstOrDefault();

            // done
            Console.WriteLine(" done. [" + token + "]");

            // main xml
            XElement root = new XElement("response");

            // let's suck data
            var tables = ConfigReader.ZohoTables;
            foreach (var t in tables)
            {
                // request table
                Console.Write("Zoho: request table '" + t + "'");

                for (int i = 0; i < 1000; i = i + 200)
                {
                    Console.Write(".");

                    // request
                    webClient.Encoding = Encoding.UTF8;
                    string content = webClient.DownloadString(string.Format(TABLE_API, t, token, i, i + 199));

                    // parse it
                    XElement partRoot = XElement.Parse(content);
                    if (partRoot.Elements("nodata").Any())
                    {
                        Console.WriteLine(" done.");
                        break;
                    }
                    else if (partRoot.Elements("error").Any())
                    {
                        Console.WriteLine(" failed.");
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
                            // hodnoty
                            var row = new XElement("record");
                            var email = r.Elements("FL").Where(e => e.Attribute("val").Value == "Email").FirstOrDefault();
                            if (email != null) row.Add(new XElement("email", email.Value));
                            else row.Add(new XElement("email", string.Empty));
                            var fname = r.Elements("FL").Where(e => e.Attribute("val").Value == "First Name").FirstOrDefault();
                            if (fname != null) row.Add(new XElement("firstName", fname.Value));
                            else row.Add(new XElement("firstName", string.Empty));
                            var lname = r.Elements("FL").Where(e => e.Attribute("val").Value == "Last Name").FirstOrDefault();
                            if (lname != null) row.Add(new XElement("lastName", lname.Value));
                            else row.Add(new XElement("lastName", string.Empty));

                            //r.Remove();
                            root.Add(row);
                        }
                    }
                }
            }

            // save it
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), string.Join("-", tables.ToArray()) + ".xml");
            root.Save(file);
            Console.WriteLine("Zoho: export saved to '" + file + "' done.");

            return root;
        }
    }
}
