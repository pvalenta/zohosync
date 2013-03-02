
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
    }
}
