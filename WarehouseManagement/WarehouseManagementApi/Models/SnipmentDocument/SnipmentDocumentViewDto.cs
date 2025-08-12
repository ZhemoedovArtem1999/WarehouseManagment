using WarehouseManagementApi.Models.DocumentCommon;

namespace WarehouseManagementApi.Models.SnipmentDocument
{
    public class SnipmentDocumentViewDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateOnly Date { get; set; }
        public string Client { get; set; }
        public bool Sign { get; set; }
        public List<DocumentResource> DocumentResources { get; set; }
    }
}
