using System;
using CloudFlareDdns.SharedLogic.Interfaces;

namespace CloudFlareDdns.Cli.Services
{
    public class ConsoleOutputService : IOutputService
    {
        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }

        public void WriteLine(Exception output)
        {
            Console.WriteLine(output.ToString());
        }
    }
}