
namespace ZohoSync
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// smart emailing data writer
    /// </summary>
    internal class SmartEmailingWriter
    {
        /// <summary>
        /// api url
        /// </summary>
        private const string API_URL = "https://admin.smartemailing.cz/xml.php";

        /// <summary>
        /// send data
        /// </summary>
        /// <param name="data">data</param>
        public void SendData(XElement data)
        {
            Console.Write("SmartEmailing: building xml request - ");

            // build xml header
            var root = new XElement("xmlrequest",
                new XElement("username", ConfigReader.SmartEmailLogin),
                new XElement("usertoken", ConfigReader.SmartEmailToken),
                new XElement("requesttype", "sm_lists"),
                new XElement("requestmethod", "SynchronizeList"),
                new XElement("details",
                    new XElement("list_name", ConfigReader.SmartEmailList),
                    new XElement("replytoemail", ConfigReader.SmartEmailReplyTo),
                    new XElement("ownername", ConfigReader.SmartEmailOwnerName),
                    new XElement("owneremail", ConfigReader.SmartEmailOwnerEmail),
                    new XElement("notMatchingContacts", "setUnsubscribed"),
                    new XElement("contacts")));
            var contacts = root.Descendants("contacts").First();

            // let's build data in
            contacts.Add(data.Elements("record")
                .Where(e => e.Element("email").Value.Length > 0)
                .Select(e => new XElement("details",
                    new XElement("emailaddress", e.Element("email").Value),
                    new XElement("format", "html"),
                    new XElement("confirmed", "yes"),
                    new XElement("status", "active"),
                    new XElement("customfields",
                        new XElement("item",
                            new XElement("fieldid", "2"),
                            new XElement("value", e.Element("firstName").Value)),
                        new XElement("item",
                            new XElement("fieldid", "3"),
                            new XElement("value", e.Element("lastName").Value))))));

            // convert done
            Console.WriteLine("done.");

            // save it
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ConfigReader.SmartEmailList + ".xml");
            root.Save(file);
            Console.WriteLine("SmartEmailing: export saved to '" + file + "' done.");

            Console.Write("SmartEmailing: submit to server - ");

            // submit it
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var response = webClient.UploadString(API_URL, root.ToString());

            // parse
            root = XElement.Parse(response);
            if (root.Element("status").Value.Equals("success", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("done.");
            }
            else
            {
                Console.WriteLine("failed.");
            }
        }
    }
}
