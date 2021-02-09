namespace AppExample.Application.Feature.Order.Queries.GetOrderDetail
{
    public class OrderStructureDto
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public string Product { get; set; }

        public int OrderId { get; set; }

        public int UnitId { get; set; }
    }
}