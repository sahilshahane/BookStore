using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class BookListPaginationOptions
    {
        public int? CursorId { get; set; }

        public int? Limit { get; set; }

        public string Search { get; set; } = string.Empty;

        public BookListPaginationOptions() { }
    }

    public class BookListModel : BookListPaginationOptions
    {
        public BookListModel() : base() { }

        public IEnumerable<Book> Books { get; set; } = [];

        public int? NextCursorId { get; set; }

        public bool PreviousPageExists { get; set; }
    }

}
