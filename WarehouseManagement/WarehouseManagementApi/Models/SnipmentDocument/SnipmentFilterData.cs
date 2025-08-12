namespace WarehouseManagementApi.Models.SnipmentDocument
{
    public class SnipmentFilterData
    {
        public IEnumerable<DropDownItem> Numbers { get; set; }
        public IEnumerable<DropDownItem> Resources { get; set; }
        public IEnumerable<DropDownItem> Units { get; set; }
        public IEnumerable<DropDownItem> Clients { get; set; }
    }
}
