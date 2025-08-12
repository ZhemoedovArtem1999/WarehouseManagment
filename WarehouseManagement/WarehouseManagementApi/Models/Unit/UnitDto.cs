namespace WarehouseManagementApi.Models.Unit
{
    public class UnitDto
    {
        public List<UnitItem> Items { get; set; }

    }

    public class UnitItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsArchive { get; set; }
    }
}
