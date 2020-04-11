using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBPushNotificationCore.Models
{
    public class APIRequest
    {  
        [Required(ErrorMessage = "Channel is required")]
        public string Channel { get; set; }

        [Required(ErrorMessage = "CustomerId is required")]
        public string CustomerId { get; set; }

    }

    public class PushNotifReq : APIRequest
    {
        public string Browser { get; set; }
        public string Device { get; set; }
        public string NotificationType { get; set; }
        public string Topic { get; set; }
        public string CustomerType { get; set; }

        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
        public string PrevToken { get; set; }
    }
    
    public class SendPushNotifReq : APIRequest
    {
        [Required(ErrorMessage = "MesssageTitle is required")]
        public string MesssageTitle { get; set; }

        [Required(ErrorMessage = "MesssageBody is required")]
        public string MesssageBody { get; set; }
        public string Image { get; set; }
        public object Data { get; set; }
        public string NotificationType { get; set; }
        public string Topic { get; set; }
        public string CustomerType { get; set; }

        [Required(ErrorMessage = "Token is required")]
        public string[] Tokens { get; set; }
    }
}
