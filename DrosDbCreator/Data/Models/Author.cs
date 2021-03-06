﻿using System;
using System.Collections.Generic;

namespace Dros.Data.Models
{
    public class Author : ITrackable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<MaterialAuthor> Materials { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
