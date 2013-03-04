
namespace ZohoSync
{
    using System;
    using System.Configuration;
    using ZohoSync.Config;

    /// <summary>
    /// read application configuration
    /// </summary>
    internal class ConfigReader
    {
        /// <summary>
        /// gets zoho login
        /// </summary>
        public static string ZohoLogin = ConfigurationManager.AppSettings["zohoLogin"];

        /// <summary>
        /// gets zoho password
        /// </summary>
        public static string ZohoPassword = ConfigurationManager.AppSettings["zohoPassword"];

        /// <summary>
        /// gets smart emailing login
        /// </summary>
        public static string SmartEmailLogin = ConfigurationManager.AppSettings["smartEmailLogin"];

        /// <summary>
        /// gets smart emailing token
        /// </summary>
        public static string SmartEmailToken = ConfigurationManager.AppSettings["smartEmailToken"];

        /// <summary>
        /// gets smart emailing reply to email
        /// </summary>
        public static string SmartEmailReplyTo = ConfigurationManager.AppSettings["smartEmailReplyTo"];

        /// <summary>
        /// gets smart emailing owner name
        /// </summary>
        public static string SmartEmailOwnerName = ConfigurationManager.AppSettings["smartEmailOwnerName"];

        /// <summary>
        /// gets smart emailing owner email
        /// </summary>
        public static string SmartEmailOwnerEmail = ConfigurationManager.AppSettings["smartEmailOwnerEmail"];

        /// <summary>
        /// gets current mapping setting
        /// </summary>
        public static MappingSetting MappingSetting = MappingSetting.CurrentSetting;
    }
}
