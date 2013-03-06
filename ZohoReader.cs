
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
        /// token
        /// </summary>
        private string token = string.Empty;

        /// <summary>
        /// authenticate
        /// </summary>
        public void Authenticate()
        {
            // write to console
            Program.OutputWrite("Zoho: authenticate - ");

            // authenticate
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string response = webClient.DownloadString(string.Format(LOGIN_API, ConfigReader.ZohoLogin, ConfigReader.ZohoPassword));

            // split by lines
            var lines = response.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // auth token
            this.token = (from l in lines
                          where l.StartsWith("AUTHTOKEN=")
                          select l.Split('=')[1]).FirstOrDefault();

            // done
            Program.OutputWriteLine(" done. [" + this.token + "]");
        }

        /// <summary>
        /// get data
        /// </summary>
        /// <param name="zohoTable">zoho table</param>
        /// <returns>Xml Element with all data</returns>
        public XElement GetData(string zohoTable)
        {

            // main xml
            XElement root = new XElement("response");

            // request table
            Program.OutputWrite("Zoho: request table '" + zohoTable + "'");
            var webClient = new WebClient();
            var counter = 0;
            for (int i = 0; i < 1000; i = i + 200)
            {
                Program.OutputWrite(".");

                // request
                webClient.Encoding = Encoding.UTF8;
                string content = webClient.DownloadString(string.Format(TABLE_API, zohoTable, token, i, i + 199));

                // parse it
                XElement partRoot = XElement.Parse(content);
                if (partRoot.Elements("nodata").Any())
                {
                    Program.OutputWriteLine(" done [" + counter + " records]");
                    break;
                }
                else if (partRoot.Elements("error").Any())
                {
                    Program.OutputWriteLine(" failed.");
                    var error = partRoot.Elements("error").First();
                    error.Remove();
                    root.Add(error);
                    break;
                }
                else if (partRoot.Descendants(zohoTable).Any())
                {
                    var result = partRoot.Descendants(zohoTable).First().Elements("row").ToList();
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

                        counter++;
                        root.Add(row);
                    }
                }
            }

            if (Program.WriteFiles)
            {
                // save it
                string file = Path.Combine(Directory.GetCurrentDirectory(), zohoTable + ".xml");
                root.Save(file);
                Program.OutputWriteLine("Zoho: export saved to '" + file + "' done.");
            }

            return root;
        }
    }
}
