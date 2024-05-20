namespace LayoutManager.Services
{
    public interface IPageService
    {
        string GetAllAsync(string userId, string itemType, int languageId);
    }
}
