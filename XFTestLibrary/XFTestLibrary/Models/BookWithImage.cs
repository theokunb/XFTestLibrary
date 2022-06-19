using System;
using Xamarin.Forms;

namespace XFTestLibrary.Models
{
    public class BookWithImage
    {
        public BookWithImage(Book book,string imagePath)
        {
            Book = book;
            if (!string.IsNullOrEmpty(imagePath))
                Cover = ImageSource.FromUri(new Uri(imagePath));
        }

        public Book Book { get; set; }
        public ImageSource Cover { get; set; }
    }
}
