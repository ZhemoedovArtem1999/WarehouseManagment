namespace WarehouseManagementApi.Models.ReceiptDocument
{
    public class ReceiptResourceDto
    {
        public int? Id { get; set; }
        public int ResourceId { get; set; }
        public int UnitId { get; set; }
        public decimal Count { get; set; }
        public bool IsChange { get; set; } = false;
        public bool IsDelete { get; set; } = false;
    }
}
