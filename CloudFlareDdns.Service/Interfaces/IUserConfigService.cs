using CloudFlareDdns.Service.Models;

namespace CloudFlareDdns.Service.Interfaces
{
    public interface IUserConfigService
    {
        void CreateFolder();
        UserConfig GetUserConfig();
        void SetUserConfig(UserConfig config);
    }
}