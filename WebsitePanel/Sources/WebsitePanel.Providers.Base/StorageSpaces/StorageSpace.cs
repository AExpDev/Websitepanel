using WebsitePanel.Providers.OS;

namespace WebsitePanel.Providers.StorageSpaces
{
    public class StorageSpace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ServiceId { get; set; }
        public int ServerId { get; set; }
        public int LevelId { get; set; }
        public string Path { get; set; }
        public QuotaType FsrmQuotaType { get; set; }
        public long FsrmQuotaSizeBytes { get; set; }
        public long UsedSizeBytes { get; set; }
    }
}