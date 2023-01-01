using System;
using System.Collections.Generic;
using System.Text;

namespace SkilledAlly.Common.Settings
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string AwsAccessKeyId { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string SerilogToken { get; set; }

    }
}
