using LayoutManager.Domain.Entities;
using LayoutManager.Domain.Repositories;
using LayoutManager.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LayoutManager.Infrastructure.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly LayoutContext _context;

        public PageRepository(LayoutContext context)
        {
            _context = context;
        }

        public List<Folder> GetTranslatedPageType(string userId, string itemType, int languageId, 
            string storedProcedureName, string? connectionString)
        {
            List<Folder> folders = new List<Folder>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlParameter itemTypeParameter = new SqlParameter("itemType", itemType);
                SqlParameter userIdParameter = new SqlParameter("ownerId", userId);
                SqlParameter languageIdParameter = new SqlParameter("languageId", languageId);
                //Create the SqlCommand object by passing the stored procedure name and connection object as parameters
                SqlCommand cmd = new SqlCommand(storedProcedureName, connection)
                {
                    //Specify the command type as Stored Procedure
                    CommandType = CommandType.StoredProcedure,
                    Parameters = { itemTypeParameter, userIdParameter, languageIdParameter }
                };

                //Open the Connection
                connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    folders.Add(new Folder
                    {
                        ItemType = Convert.ToString(sdr["itemType"]),
                        OwnerId = Convert.ToInt64(sdr["ownerId"]),
                        FolderId = Convert.ToInt64(sdr["folderId"]),
                        FolderName = Convert.ToString(sdr["folderName"]),
                        FolderDisplayOrder = Convert.ToInt32(sdr["folderDisplayOrder"])

                    });
                }

                sdr.Close();
            }

            return folders;

        }
    }
}
