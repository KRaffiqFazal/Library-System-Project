using System;
using System.Collections.Generic;

namespace Library_System
{
    public class Book
    {
        public String id { get; set; }
        public String title { get; set; }
        public String author { get; set; }
        public String year { get; set; }
        public String publisher { get; set; }
        public String edition { get; set; }
        public String isbn { get; set; }
        public String category { get; set; }
        public int availableCopies { get; set; }
        public String description { get; set; }
        public DateTime dueDate { get; set; }
        public decimal price { get; set; }
        public DateTime reserved { get; set; }
        public bool renewed { get; set; }

        public Book()
        {
            availableCopies = 0;
            reserved = DateTime.MinValue; //reserved is the date the book is reserved till, if min value it is not reserved
            renewed = false;
        }
    }
}
