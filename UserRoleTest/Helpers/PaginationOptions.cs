namespace UserRoleTest.Helpers
{
    public class PaginationOptions
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public PaginationOptions(){}
        public PaginationOptions(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 1 ? 20 : pageSize;
        }
    }
}
