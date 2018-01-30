using System;
using System.Collections.Generic;

namespace  Dros.Data.Models
{
    public class Category : ITrackable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<MaterialCategory> Materials { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
