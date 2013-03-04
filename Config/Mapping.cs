
namespace ZohoSync.Config
{
    using System.Configuration;

    /// <summary>
    /// mapping element
    /// </summary>
    public class Mapping : ConfigurationElement
    {
        /// <summary>
        /// zoho table
        /// </summary>
        [ConfigurationProperty("zoho", IsRequired = true)]
        public string ZohoTable
        {
            get
            {
                return this["zoho"] as string;
            }
        }

        /// <summary>
        /// smart emailing list
        /// </summary>
        [ConfigurationProperty("smartEmail", IsRequired = true)]
        public string SmartEmailList
        {
            get
            {
                return this["smartEmail"] as string;
            }
        }
    }
}
