using GTBPushNotificationCore.Models;
using GTBPushNotificationCore.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GTBPushNotificationCore.Logic
{
    static public class PushNotificationLogic
    {
        private static Uri FireBasePushNotificationsURL = new Uri(ConstantManager.FireBasePushNotificationsURL());
        //private static string ServerKey = "AAAAvgqlqyM:APA91bGrOCHA2DYz8lq4iV7OhGAdV9qdLPril_PzMt9FAfI-ylb0oVtlG_Goa-vaElvRN8dK4Ux-PWo03OrXMKFtwcgdO3twYFFX_vz_jK6Lth3v-Cb_HaW3W6Ci_tUBL9JvqgMxYF5b";
        private static string ServerKey = ConstantManager.ServerKey();


        public static async Task<bool> SendPushNotification(string[] deviceTokens, string title, string body, string image)
        {
            bool sent = false;

            if (deviceTokens.Count() > 0)
            {
                //Object creation

                var messageInformation = new Message()
                {
                    notification = new Notification()
                    {
                        title = title,
                        body = body,
                        image = image
                    },
                    registration_ids = deviceTokens
                };

                //Object to JSON STRUCTURE => using Newtonsoft.Json;
                string jsonMessage = JsonConvert.SerializeObject(messageInformation);

                /*
                 ------ JSON STRUCTURE ------
                 {
                    notification: {
                                    title: "",
                                    text: ""
                                    },
                    data: {
                            action: "Play",
                            playerId: 5
                            },
                    registration_ids = ["id1", "id2"]
                 }
                 ------ JSON STRUCTURE ------
                 */

                //Create request to Firebase API
                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);

                request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                HttpResponseMessage result;
                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(request);
                    sent = result.IsSuccessStatusCode;
                }
            }

            return sent;
        }

    }
}
