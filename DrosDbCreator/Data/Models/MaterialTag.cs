using System;

namespace Dros.Data.Models
{
    public class MaterialTag
    {
        public string MaterialId { get; set; }
        public string TagId { get; set; }
        public Tag Tag { get; set; }
        public Material Material { get; set; }
    }
}
