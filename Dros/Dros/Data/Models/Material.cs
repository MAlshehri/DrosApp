using System;
using System.Collections.Generic;

namespace Dros.Data.Models
{
    public class Material : ITrackable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Language { get; set; }
        public virtual ICollection<MaterialAuthor> Authors { get; set; }
        public virtual ICollection<MaterialTag> Tags { get; set; }
        public virtual ICollection<MaterialCategory> Categories { get; set; }
        public virtual ICollection<Url> Urls { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
