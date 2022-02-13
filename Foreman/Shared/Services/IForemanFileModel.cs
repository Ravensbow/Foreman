namespace Foreman.Shared.Models.Category
{
    public interface IForemanFileModel
    {
        public byte[] FileData { get; set; }
        public int ContextId { get; set; }
        public string Component { get; set; }
        public int? ItemId { get; set; }
        public int? UserId { get; set; }
        public string Filename { get; set; }
        public string MimeType { get; set; }
    }
}