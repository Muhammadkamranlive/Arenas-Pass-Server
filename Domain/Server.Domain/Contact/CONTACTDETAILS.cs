namespace Server.Domain
{
    public class CONTACTDETAILS
    {
        public Guid   Id            { get; set; }
        public string userId        { get; set; }
        public string HomePhone     { get; set; }
        public string MobilePhone   { get; set; }
        public string Carrier       { get; set; }
        public string PersonalEmail { get; set; }
        public int   TenantId       { get; set; } = 1;
    }
}