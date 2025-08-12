namespace DataAccessLayer.Infrastructure.FilterModel
{
    public class BalanceFilter
    {
        public IEnumerable<int>? Resources { get; set; }
        public IEnumerable<int>? Units { get; set; }
    }
}
