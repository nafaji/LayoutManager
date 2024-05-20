namespace LayoutManager.Services
{
    public interface ISecurityService
    {
        bool IsValid(string dataToDecrypt, bool isHexString = true);

        string DecryptData(string dataToDecrypt, bool isHexString = true);
    }
}
