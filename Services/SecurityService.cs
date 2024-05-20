using LayoutManager.Helpers;
using Microsoft.Extensions.Options;

namespace LayoutManager.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly Configuration _configuration;
        public SecurityService(IOptions<Configuration> options)
        {
            _configuration = options.Value;
        }

        public bool IsValid(string dataToDecrypt, bool isHexString)
        {
            return SecurityManager.IsValid(_configuration.SymmetricKey, dataToDecrypt, isHexString);
        }

        public string DecryptData(string dataToDecrypt, bool isHexString)
        {
            return SecurityManager.DecryptData(_configuration.SymmetricKey, dataToDecrypt, isHexString);
        }
    }
}
