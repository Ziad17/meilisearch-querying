namespace FluentSearchEngine.Paging
{
    public class PageCriteria
    {


        public PageCriteria(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

    }
}
