using WarehouseManagementApi.Models.DocumentCommon;

namespace WarehouseManagementApi.Models.ReceiptDocument
{
    public class ReceiptDocumentViewDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateOnly Date { get; set; }
        public List<DocumentResource> DocumentResources { get; set; }
    }
}
