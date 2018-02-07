namespace Dros.Data.Models
{
    public class MaterialCategory
    {
        public string MaterialId { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public Material Material { get; set; }
    }
}
