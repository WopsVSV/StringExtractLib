using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringExtract.Dummy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string key = "SECRET KEY";

            Console.WriteLine("Type in your key:");
            var input = Console.ReadLine();

            if (key == input)
                Console.WriteLine("Correct, you can pass.");
            else
                Console.WriteLine("Incorrect, you shall not pass.");

            Console.ReadKey(true);
        }
    }
}
