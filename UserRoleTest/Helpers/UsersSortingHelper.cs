using System.Reflection;
using System.Text.Json.Serialization;
using UserRoleTest.Models;

namespace UserRoleTest.Helpers
{
    /// <summary>
    /// Реализует сорировку коллекции типа User по строковым параметрам
    /// </summary>
    public class UsersSortingHelper
    {
        public string SortBy { get; set; } = ""; 

        public string Order { get; set; } = "";
        
        public UsersSortingHelper()
        {
            
        }

        /// <summary>
        /// Сортирует коллекцию заданного типа по значению поля "SortBy"
        /// </summary>
        /// <param name="list">Коллекция для сортировки</param>
        /// <returns>Отсортированное коллекция</returns>
        public IQueryable<User> SortResponse(IQueryable<User> list)
        {
            bool IsOrderAsc = (Order != "desc") && (Order != "descending") ? true : false;
            
            var result = SortBy.ToUpper() switch
            {
                "NAME" => IsOrderAsc ? list.OrderBy(x => x.Name) : list.OrderByDescending(x => x.Name),
                "ID" => IsOrderAsc ? list.OrderBy(x => x.Id) : list.OrderByDescending(x => x.Id),
                "AGE" => IsOrderAsc ? list.OrderBy(x => x.Age) : list.OrderByDescending(x => x.Age),
                "EMAIL" => IsOrderAsc ? list.OrderBy(x => x.Email) : list.OrderByDescending(x => x.Email),
                _ => IsOrderAsc ? list.OrderBy(x => x.Name) : list.OrderByDescending(x => x.Name)
            };
            return result;
        }
        
        

    }
}
