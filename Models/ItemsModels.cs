using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStore.Models
{
    public class ItemsModels
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public DateTime Entered { get; set; }
    }
}