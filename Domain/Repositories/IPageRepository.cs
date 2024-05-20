using LayoutManager.Domain.Entities;
using LayoutManager.Dtos;

namespace LayoutManager.Domain.Repositories
{
    public interface IPageRepository
    {
        List<Folder> GetTranslatedPageType(string userId, 
            string itemType, 
            int languageId, 
            string storedProcedureName, 
            string? connectionString);
    }
}
