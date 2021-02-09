namespace AppExample.Application.Feature.Order.Commands.Create
{
    public class OrderStructureDto
    {
        public int Count { get; set; }
        
        /// <summary>
        /// Товар, который надо транспортировать
        /// </summary>
        public string Product { get; set; }
        
        /// <summary>
        /// Единицы измерения
        /// </summary>
        public int UnitId { get; set; }
    }
}