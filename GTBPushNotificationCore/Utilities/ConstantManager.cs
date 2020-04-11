using GTBEncryptLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBPushNotificationCore.Utilities
{
    public static class ConstantManager
    {
        public static string EOneConnString()
        {
            try
            {
                return GTBEncryptLib.DecryptText(ConfigurationManager.ConnectionStrings["EOneConnString"].ConnectionString);
            }
            catch (Exception ex)
            {
                ErrHandler.Log("Unable to find EOneConnString: " + ex.Message);
                throw;
            }
        }
        
        public static string ServerKey()
        {
            try
            {
                return ConfigurationManager.AppSettings["ServerKey"];
            }
            catch (Exception ex)
            {
                ErrHandler.Log("Unable to find ServerKey: " + ex.Message);
                throw;
            }
        }
        
        public static string FireBasePushNotificationsURL()
        {
            try
            {
                return ConfigurationManager.AppSettings["FireBasePushNotificationsURL"];
            }
            catch (Exception ex)
            {
                ErrHandler.Log("Unable to find FireBasePushNotificationsURL: " + ex.Message);
                throw;
            }
        }

    }
}
