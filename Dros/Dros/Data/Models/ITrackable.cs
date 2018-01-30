using System;

namespace Dros.Data.Models
{
    public interface ITrackable
    {
        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset UpdatedOn { get; set; }
    }
}