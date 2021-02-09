namespace AppExample.Core.Entities
{
    /// <summary>
    /// Состав заказа
    /// </summary>
    public class OrderStructure
    {
        public int Id { get; set; }

        public int Count { get; set; }
        
        /// <summary>
        /// Товар, который надо транспортировать
        /// </summary>
        public string Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
        
        /// <summary>
        /// Единицы измерения
        /// </summary>
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}