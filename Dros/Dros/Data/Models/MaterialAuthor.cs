namespace Dros.Data.Models
{
    public class MaterialAuthor
    {
        public System.Guid MaterialId { get; set; }
        public System.Guid AuthorId { get; set; }
        public Author Author { get; set; }
        public Material Material { get; set; }
    }
}
