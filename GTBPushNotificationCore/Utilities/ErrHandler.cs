using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GTBPushNotificationCore.Utilities
{
    class ErrHandler
    {
        public static void Log(string msg)
        {
            try
            {
                string loc = System.Web.HttpContext.Current.Server.MapPath("~/Error/" + DateTime.Today.ToString("dd-MM-yy"));
                if (!Directory.Exists(loc))
                {
                    Directory.CreateDirectory(loc);
                }

                string path = loc + "/" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                
                using (StreamWriter w = File.AppendText((path)))
                {
                    w.WriteLine("\r\nLog Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    //w.WriteLine(ui.customername); 
                    string err = "Error in: " + HttpContext.Current.Request.Url.ToString() + ". Error Message:" + msg;
                    w.WriteLine(err);
                    w.WriteLine("_______________________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace);
            }

        }
    }
}
