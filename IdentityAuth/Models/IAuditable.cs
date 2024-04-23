namespace IdentityAuth.Models
{
    public interface IAuditable
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get;set; }
        public DateTimeOffset DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
