using System.Text.Json;

namespace UserRoleTest.Helpers
{
    /// <summary>
    /// Инкапсулирует свойства пагинации для коллекции
    /// </summary>
    public class PaginationOptions
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public int TotalElements;

        public int TotalPages;

        public bool HasPrevious;

        public bool HasNext;

        public PaginationOptions(){}
        public PaginationOptions(int pageNumber, int pageSize, int totalCount)
        {
            CurrentPage = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize < 1 ? 200 : pageSize;
            TotalElements = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            HasPrevious = CurrentPage > 1;
            HasNext = CurrentPage < PageSize;
        }

        public string GetSerializedMetadata()
        {
            var metadata = new
            {
                TotalElements,
                PageSize,
                CurrentPage,
                TotalPages,
                HasNext,
                HasPrevious
            };
            
            return JsonSerializer.Serialize(metadata);
        }
    }
}
