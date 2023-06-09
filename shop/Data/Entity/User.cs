namespace shop.Data.Entity
{
    public class User
    {
        public Guid      Id             { get; set; }
        public string    Login          { get; set; } = null!;
        public string    PasswordHash   { get; set; } = null!;
        public string?   RealName       { get; set; }
        public string    Email          { get; set; } = null!;
        public string?   Avatar         { get; set; }
        public DateTime  RegisteredDt   { get; set; }
    }
}
