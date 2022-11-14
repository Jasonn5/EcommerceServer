namespace Authentication.Entities
{
    public class Email
    {
        public string Subject { get; set; }

        public string Message { get; set; }

        public string ToAddresses { get; set; }

        public string CcAddresses { get; set; }

        public string BccAddresses { get; set; }

        public bool IsBodyHtml { get; set; }
    }
}
