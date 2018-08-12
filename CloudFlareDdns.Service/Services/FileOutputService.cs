using System;
using System.IO;
using CloudFlareDdns.SharedLogic.Interfaces;

namespace CloudFlareDdns.Service.Services
{
    public class FileOutputService : IOutputService
    {
        private readonly string _logFile;

        public FileOutputService(string folder)
        {
            _logFile = Path.Combine(folder, "log.txt");
        }

        public void WriteLine(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(_logFile, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " +
                             ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public void WriteLine(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(_logFile, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
    }
}
