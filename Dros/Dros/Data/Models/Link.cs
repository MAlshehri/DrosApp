using System;

namespace Dros.Data.Models
{
    public class Link : ITrackable
    {
        public Guid Id { get; set; }
        public Guid MaterialId { get; set; }
        public int Order { get; set; }
        public long ContentLength { get; set; }
        public string Content { get; set; }
        public AudioType AudioType { get; set; }
        public Material Material { get; set; }
        
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
    }

    public enum AudioType
    {
        Mp3 = 1,
        Ogg,
        Wma,
        Rm
    }
}
