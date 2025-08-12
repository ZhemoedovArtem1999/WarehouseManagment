namespace WarehouseManagementApi.Models
{
    public class ClientDto
    {
        public List<ClientItem> Items { get; set; }
    }
    public class ClientItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsArchive { get; set; }
    }
}
