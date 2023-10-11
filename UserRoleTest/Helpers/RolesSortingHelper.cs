using System.Text.Json.Serialization;
using UserRoleTest.Models;

namespace UserRoleTest.Helpers
{
    /// <summary>
    /// Реализует сорировку коллекции типа User по строковым параметрам
    /// </summary>
    public class RolesSortingHelper
    {
        public string SortBy { get; set; } = ""; 

        public string Order { get; set; } = "";
        
        public RolesSortingHelper()
        {
            
        }

        /// <summary>
        /// Сортирует коллекцию заданного типа по значению поля "SortBy"
        /// </summary>
        /// <param name="list">Коллекция для сортировки</param>
        /// <returns>Отсортированное коллекция</returns>
        public IQueryable<Role> SortResponse(IQueryable<Role> list)
        {
            bool IsOrderAsc = (Order != "desc") && (Order != "descending") ? true : false;
            
            var result = SortBy.ToUpper() switch
            {
                "NAME" => IsOrderAsc ? list.OrderBy(x => x.Name) : list.OrderByDescending(x => x.Name),
                "ID" => IsOrderAsc ? list.OrderBy(x => x.Id) : list.OrderByDescending(x => x.Id),
                _ => IsOrderAsc ? list.OrderBy(x => x.Name) : list.OrderByDescending(x => x.Name)
            };
            return result;
        }
        
        

    }
}
