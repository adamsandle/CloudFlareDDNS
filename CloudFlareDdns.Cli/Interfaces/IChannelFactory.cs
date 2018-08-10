namespace CloudFlareDdns.Cli.Interfaces
{
    public interface IChannelFactory
    {
        T CreateChannelFactory<T>(NetworkOptions opts);
    }
}