namespace KAShop.Model
{
     public enum Status
    {
        Active = 1,
        Inactive = 0,
    }
    public class BaseShared 
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Active;

    }
}
