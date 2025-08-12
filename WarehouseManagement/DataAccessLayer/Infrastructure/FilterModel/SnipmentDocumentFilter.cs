using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.FilterModel
{
    public class SnipmentDocumentFilter : ReceiptDocumentFilter
    {
        public IEnumerable<int>? Clients { get; set; }
    }
}
