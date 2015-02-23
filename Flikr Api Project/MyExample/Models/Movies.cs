using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;

namespace MyExample.Models
{
    public class Movie
    {

        public int ID { get; set; }
        [Required, StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }
        [Display(Name = "Release Date"), DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Genre { get; set; }
        [Range(1, 100), DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [ StringLength(5)]
        public string Rating { get; set; }
      
    }



    //public class Photo
    //{     //[Required]
    //    public int UId { get; set; }
    //    public string id { get; set; }
    //    public string owner { get; set; }
    //    public string secret { get; set; }
    //    public string server { get; set; }
    //    public int farm { get; set; }
    //    public string title { get; set; }
    //    public int ispublic { get; set; }
    //    public int isfriend { get; set; }
    //    public int isfamily { get; set; }
    //}

    public class MovieDBContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MyExample.Controllers.Photo> Photos { get; set; }
        public MovieDBContext()
            : base("MovieDBContext")
        {
        }

        

    }
}