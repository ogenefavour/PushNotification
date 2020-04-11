using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBPushNotificationCore.Models
{
    public class ApiResponseModel
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string CustomerId { get; set; }
        
    }

    public class PushNotifResp : ApiResponseModel
    {
        public int TokenCount { get; set; }
        public int SentStatus { get; set; }
    }
  
    public class GetToken : ApiResponseModel
    {
        public string Token { get; set; }
    }
    public class GetUserTokensResp : ApiResponseModel
    {
        public string[] Tokens { get; set; }
    }

    public class GetAllTokensResp
    {
        public string[] Tokens { get; set; }

        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
    }
}
