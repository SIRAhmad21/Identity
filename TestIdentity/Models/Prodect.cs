using System.ComponentModel.DataAnnotations.Schema;

namespace TestIdentity.Models
{
    public class Prodect
    {
        public int ProdectId { get; set; }
        public string? ProductName { get; set; }
        public int Price { get; set;}
      //  [NotMapped]
    //    public IFormFile ProductImage { get; set; }
      //  public string ImagePath { get; set; }   
    }
}
