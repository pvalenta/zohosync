
namespace ZohoSync
{
    using System;

    /// <summary>
    /// main entry class
    /// </summary>
    class Program
    {
        /// <summary>
        /// main entry
        /// </summary>
        /// <param name="args">args</param>
        static void Main(string[] args)
        {
            // write header
            Console.WriteLine("Zoho Sync Tool (1.0.0)");
            Console.WriteLine("© Pavel Valenta 2013");
            Console.WriteLine("");

            // get zoho
            var zoho = new ZohoReader();
            var response = zoho.GetData();

            // send to smart emailing
            var smart = new SmartEmailingWriter();
            smart.SendData(response);

            // the end
            Console.WriteLine("");
            Console.WriteLine("Please press key to close.");
            Console.ReadKey(true);
        }
    }
}
