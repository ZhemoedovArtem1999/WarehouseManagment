namespace WarehouseManagementApi.Exceptions
{
    public class InvalidUpdateOperationException : InvalidOperationException
    {
        public int? IdObject { get; set; }
        public InvalidUpdateOperationException()
        {
        }

        public InvalidUpdateOperationException(string message)
            : base(message)
        {
        }
        public InvalidUpdateOperationException(string message, int idObject)
          : base(message)
        {
            IdObject = idObject;
        }

        public InvalidUpdateOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        public InvalidUpdateOperationException(string message, Exception innerException, int idObject)
          : base(message, innerException)
        {
            IdObject = idObject;
        }
    }
}
