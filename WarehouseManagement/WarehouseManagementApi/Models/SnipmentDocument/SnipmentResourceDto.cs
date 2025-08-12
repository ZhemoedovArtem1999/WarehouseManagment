namespace WarehouseManagementApi.Models.SnipmentDocument
{
    public class SnipmentResourceDto
    {
        public int? Id { get; set; }
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal Count { get; set; } = decimal.Zero;
        public bool IsChange { get; set; } = false;
        public decimal CountBalance { get; set; }
    }
}
