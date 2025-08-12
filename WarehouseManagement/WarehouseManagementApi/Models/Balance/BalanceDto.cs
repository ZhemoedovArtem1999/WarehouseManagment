namespace WarehouseManagementApi.Models.Balance
{
    public class BalanceDto
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal Count { get; set; }
    }
}
