using System;
using System.IO;

namespace CloudFlareDDNS
{
    public static class Config
    {
        public static readonly string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CloudFlareDDNS/Logfile.txt");
    }
}