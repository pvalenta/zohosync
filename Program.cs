using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZohoSync
{
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
            Console.WriteLine("Zoho Sync Tool");
            Console.WriteLine("");

            // get zoho
            var zoho = new ZohoReader();
            zoho.GetData();

            // the end
            Console.WriteLine("");
            Console.WriteLine("Please press key to close.");
            Console.ReadKey(true);
        }
    }
}
