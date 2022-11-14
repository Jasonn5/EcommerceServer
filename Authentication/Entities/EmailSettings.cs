namespace Authentication.Entities
{
    public class EmailSettings
    {
        public string SmtpClientHost { get; set; }

        public int? SmtpClientPort { get; set; }

        public string SmtpClientUsername { get; set; }

        public string SmtpClientPassword { get; set; }

        public string FromAddress { get; set; }

        public string FromDisplayName { get; set; }
    }
}