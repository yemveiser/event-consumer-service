using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMatcha.BackgroundServiceCore.Features.SMS.Models
{
    public class SMSLive247Options
    {
        public string OwnerEmail { get; set; } = string.Empty;
        public string SubAccount { get; set; } = string.Empty ;
        public string SubAccountPassword { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty ;
        public string ApiUrl { get; set;} = string.Empty ;
    }
}
