using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class BookListModel : BookListPaginationOptionsModel
    {

        // indicates current cursor id 
        //public int? CursorId { get; set; }

        //public int? Limit { get; set; }

        //public string Search { get; set; } = string.Empty;

        public BookListModel() : base() { }

        public IEnumerable<Book> Books { get; set; } = [];

        public int? NextCursorId { get; set; }

        internal int? PreviousCursorId { get; set; }
    }

}
