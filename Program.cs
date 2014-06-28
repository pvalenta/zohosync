
namespace ZohoSync
{
    using System;
    using System.IO;
    using System.Linq;
    using ZohoSync.Config;

    /// <summary>
    /// main entry class
    /// </summary>
    class Program
    {
        /// <summary>
        /// output files
        /// </summary>
        public static bool WriteFiles { get; set; }

        /// <summary>
        /// wait for key
        /// </summary>
        public static bool WaitForKey { get; set; }

        /// <summary>
        /// skip log
        /// </summary>
        public static bool SkipLog { get; set; }

        /// <summary>
        /// test only
        /// </summary>
        public static bool TestOnly { get; set; }

        /// <summary>
        /// main entry
        /// </summary>
        /// <param name="args">args</param>
        static void Main(string[] args)
        {
            // write header
            Console.WriteLine("Zoho Sync Tool (1.0.1)");
            Console.WriteLine("© Pavel Valenta 2013");
            Console.WriteLine("");

            if (args.Length > 0)
            {
                if (args.Contains("writefiles")) WriteFiles = true;
                if (args.Contains("waitforkey")) WaitForKey = true;
                if (args.Contains("skiplog")) SkipLog = true;
                if (args.Contains("testonly")) TestOnly = true;
            }

            // header
            if (!SkipLog)
            {
                using (StreamWriter writer = File.AppendText(Path.Combine(Directory.GetCurrentDirectory(), "ZohoSync.log")))
                {
                    writer.WriteLine("");
                    writer.WriteLine("Sync Start On: " + DateTime.Now.ToString());
                    writer.Close();
                }
            }

            // get reader & writer
            var zoho = new ZohoReader();
            var smart = new SmartEmailingWriter();

            // get mapping
            var mapping = ConfigReader.MappingSetting;
            if (!zoho.Authenticate())
            {
                if (WaitForKey)
                {
                    Console.WriteLine("Please press key to close.");
                    Console.ReadKey(true);
                }
                return;
            }
            foreach (var m in mapping.Setting)
            {
                var map = m as Mapping;

                // get data
                var response = zoho.GetData(map.ZohoTable);

                // send to smart emailing
                smart.SendData(map.SmartEmailList, response);
            }

            // footer
            if (!SkipLog)
            {
                using (StreamWriter writer = File.AppendText(Path.Combine(Directory.GetCurrentDirectory(), "ZohoSync.log")))
                {
                    writer.WriteLine("Sync End On: " + DateTime.Now.ToString());
                    writer.WriteLine("");
                    writer.Close();
                }
            }

            // the end
            Console.WriteLine("");
            if (WaitForKey)
            {
                Console.WriteLine("Please press key to close.");
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// output write text
        /// </summary>
        /// <param name="text">text</param>
        public static void OutputWrite(string text)
        {
            Console.Write(text);
            if (!SkipLog)
            {
                using (StreamWriter writer = File.AppendText(Path.Combine(Directory.GetCurrentDirectory(), "ZohoSync.log")))
                {
                    writer.Write(text);
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// output write line text
        /// </summary>
        /// <param name="text">text</param>
        public static void OutputWriteLine(string text)
        {
            Console.WriteLine(text);
            if (!SkipLog)
            {
                using (StreamWriter writer = File.AppendText(Path.Combine(Directory.GetCurrentDirectory(), "ZohoSync.log")))
                {
                    writer.WriteLine(text);
                    writer.Close();
                }
            }
        }
    }
}
