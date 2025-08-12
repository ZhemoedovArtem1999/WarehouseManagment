using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.FilterModel
{
    public class ReceiptDocumentFilter
    {
        public DateOnly? PeriodS { get; set; }
        public DateOnly? PeriodPo { get; set; }
        public IEnumerable<string>? Numbers { get;set; }
        public IEnumerable<int>? Resources { get;set; }
        public IEnumerable<int>? Units { get;set; }
    }
}
