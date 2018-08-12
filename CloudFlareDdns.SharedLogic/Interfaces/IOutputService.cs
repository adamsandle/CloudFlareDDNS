using System;

namespace CloudFlareDdns.SharedLogic.Interfaces
{
    public interface IOutputService
    {
        void WriteLine(string output);
        void WriteLine(Exception e);
    }
}