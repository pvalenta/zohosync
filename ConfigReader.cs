
namespace ZohoSync
{
    using System;
    using System.Configuration;

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
        /// gets zoho ta
        /// </summary>
        public static string[] ZohoTables = ConfigurationManager.AppSettings["zohoTables"].Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// gets smart emailing login
        /// </summary>
        public static string SmartEmailLogin = ConfigurationManager.AppSettings["smartEmailLogin"];

        /// <summary>
        /// gets smart emailing token
        /// </summary>
        public static string SmartEmailToken = ConfigurationManager.AppSettings["smartEmailToken"];

        /// <summary>
        /// gets smart emailing list
        /// </summary>
        public static string SmartEmailList = ConfigurationManager.AppSettings["smartEmailList"];

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
    }
}
