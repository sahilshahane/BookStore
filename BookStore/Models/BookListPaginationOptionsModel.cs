namespace BookStore.Models
{
    public class BookListPaginationOptionsModel
    {

        public int? CursorId { get; set; }

        public int? Limit { get; set; }

        public string Search { get; set; } = string.Empty;

        public BookListPaginationOptionsModel() { }
    }

}
