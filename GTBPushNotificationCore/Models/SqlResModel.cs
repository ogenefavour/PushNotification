using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBPushNotificationCore.Models
{
    public class SqlResModel
    {
      
        public string ResponseCode { get; set; }
        public int ResultInt { get; set; }
        public DataTable ResultDataTable { get; set; }
        public bool ResultBool { get; set; }
    }
    
}
