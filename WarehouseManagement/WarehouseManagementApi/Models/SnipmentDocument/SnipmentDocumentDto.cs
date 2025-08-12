namespace WarehouseManagementApi.Models.SnipmentDocument
{
    public class SnipmentDocumentDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateOnly Date {  get; set; }
        public int ClientId { get; set; }
        public bool IsSign { get; set; }
        public IEnumerable<SnipmentResourceDto>? Resources { get; set; }
    }
}
