namespace WarehouseManagementApi.Models
{
    public class ResourceDto
    {
        public List<ResourceItem> Items { get; set; }
    }

    public class ResourceItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsArchive { get; set; }
    }
}
