namespace Dros.Data.Models
{
    public class MaterialAuthor
    {
        public string MaterialId { get; set; }
        public string AuthorId { get; set; }
        public Author Author { get; set; }
        public Material Material { get; set; }
    }
}
