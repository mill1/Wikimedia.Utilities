using System;
using Wikimedia.Utilities.Services;

namespace Wikimedia.Utilities.Console
{
    public class Program
    {
        static void Main()
        {
            new Program().Run();
        }

        public void Run()
        {
            try
            {
                System.Console.WriteLine("Enter Wikipedia article name (f.i.: Winston Churchill):");

                System.Console.ForegroundColor = ConsoleColor.Yellow;
                var article = System.Console.ReadLine();
                System.Console.ForegroundColor = ConsoleColor.White;

                if (string.IsNullOrEmpty(article))
                    throw new ArgumentNullException(article, "Wikipedia article name cannot be empty");

                var count = new WikipediaWebClient().GetWikimediaSearchDirectLinkCount(article);
                System.Console.WriteLine($"Number of links to article {article}: {count}");
            }
            catch (Exception e)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(e);
                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
