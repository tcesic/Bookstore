namespace BookStore.DTOModels
{
    public class SearchParametersRequest
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;
    }
}
