using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Account
{
    public class AppSettings
    {
        // Properties for JWT Token Signature
        public string Site { get; set; }

        public string Audience { get; set; }
        public string ExpireTime { get; set; }
        public string Secret { get; set; }
        public string EmailSender { get; set; }
        public string Password { get; set; }
        public string UrlLogin { get; set; }
        public string UrlLoginMerchant { get; set; }
        public int EmailPort { get; set; }
        public string Host { get; set; }
        public string OrganisationType { get; set; }
        public string WebSURL { get; set; }
        public string OutDir { get; set; }




    }
}
