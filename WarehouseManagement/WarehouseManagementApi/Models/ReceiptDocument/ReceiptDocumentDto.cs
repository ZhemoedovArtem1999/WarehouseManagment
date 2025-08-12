namespace WarehouseManagementApi.Models.ReceiptDocument
{
    public class ReceiptDocumentDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateOnly Date {  get; set; }
        public IEnumerable<ReceiptResourceDto>? Resources { get; set; }
    }
}
