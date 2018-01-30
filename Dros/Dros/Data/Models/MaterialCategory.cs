namespace Dros.Data.Models
{
    public class MaterialCategory
    {
        public System.Guid MaterialId { get; set; }
        public System.Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Material Material { get; set; }
    }
}
