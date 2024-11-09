namespace Server.Models
{
    /// <summary>
    /// Certificate Enable Path
    /// </summary>
    public class Certificate_Settings_Model
    {
        public string AppleWWDRCACertificatePath  { get; set; }
        public string PassbookCertificatePath     { get; set; }
        public string PassbookCertificatePassword { get; set; }
    }
}
