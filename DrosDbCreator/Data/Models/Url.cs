using System;

namespace Dros.Data.Models
{
    public class Url : ITrackable
    {
        public string Id { get; set; }
        public string MaterialId { get; set; }
        public int Order { get; set; }
        public long ContentLength { get; set; }
        public string Link { get; set; }
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
        Rm,
        Iso,
        Unknown
    }
}
