namespace WarehouseManagementApi.Models.ReceiptDocument
{
    public class ReceiptFilterData
    {
        public IEnumerable<DropDownItem> Numbers { get; set; }
        public IEnumerable<DropDownItem> Resources { get; set; }
        public IEnumerable<DropDownItem> Units { get; set; }
    }
}
