namespace Q1
{
    public class OrderDTO
    {
        public int orderId { get; set; }
        public DateOnly? orderDate { get; set; }

        public virtual CustomerDTO customer { get; set; }
    }
}
