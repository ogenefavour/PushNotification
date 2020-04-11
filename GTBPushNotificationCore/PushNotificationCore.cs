using GTBPushNotificationCore.Logic;
using GTBPushNotificationCore.Models;
using GTBPushNotificationCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBPushNotificationCore
{
    public class PushNotificationCore
    {
        public PushNotifResp PushNotification(PushNotifReq pushNotifReq)
        {
            PushNotifResp resp = new PushNotifResp();
            try
            {
                if (string.IsNullOrEmpty(pushNotifReq.PrevToken))
                {
                    //Insert
                    SqlParameter[] sqlParams = new SqlParameter[]
                    {
                        new SqlParameter("@CustomerId", pushNotifReq.CustomerId),
                        new SqlParameter("@Browser", pushNotifReq.Browser),
                        new SqlParameter("@Channel", pushNotifReq.Channel),
                        new SqlParameter("@Device", pushNotifReq.Device),
                        new SqlParameter("@NotificationType", pushNotifReq.NotificationType),
                        new SqlParameter("@Topic", pushNotifReq.Topic),
                        new SqlParameter("@CustomerType", pushNotifReq.CustomerType),
                        new SqlParameter("@Token", pushNotifReq.Token),
                    };


                    SqlResModel insertToken = SqlDbHelper.InsertWithParam(ConstantManager.EOneConnString(), "proc_InsertToken", CommandType.StoredProcedure, sqlParams);

                    if (insertToken.ResponseCode == "00" && insertToken.ResultInt > 0)
                    {
                        resp.ResponseCode = "00";
                        resp.TokenCount = insertToken.ResultInt;
                        resp.ResponseDescription = "successful";
                    }
                    else
                    {
                        resp.ResponseCode = "01";
                        resp.TokenCount = insertToken.ResultInt;
                        resp.ResponseDescription = "failed";
                    }
                }
                else
                {
                    //select
                    SqlParameter[] sqlParam = new SqlParameter[]
                    {
                        new SqlParameter("@PrevToken", pushNotifReq.PrevToken),
                    };

                    SqlResModel selectToken = SqlDbHelper.SelectWithParam(ConstantManager.EOneConnString(), "proc_GetPrevToken", CommandType.StoredProcedure, sqlParam);
                    //if record exist - update
                    if (selectToken.ResponseCode == "00" && selectToken.ResultDataTable.Rows.Count > 0)
                    {
                        SqlParameter[] sqlParam1 = new SqlParameter[]
                        {
                            new SqlParameter("@Token", pushNotifReq.Token),
                            new SqlParameter("@PrevToken", pushNotifReq.PrevToken),
                        };

                        SqlResModel updateToken = SqlDbHelper.SelectWithParam(ConstantManager.EOneConnString(), "proc_UpdateToken", CommandType.StoredProcedure, sqlParam1);

                        if (updateToken.ResultInt > 0 && updateToken.ResponseCode == "00")
                        {
                            resp.ResponseCode = "00";
                            resp.TokenCount = updateToken.ResultInt;
                            resp.ResponseDescription = "successful";
                        }
                        else
                        {
                            resp.ResponseCode = "01";
                            resp.TokenCount = updateToken.ResultInt;
                            resp.ResponseDescription = "failed";
                        }
                    }
                    else
                    {
                        SqlParameter[] sqlParams = new SqlParameter[]
                        {
                            new SqlParameter("@CustomerId", pushNotifReq.CustomerId),
                            new SqlParameter("@Browser", pushNotifReq.Browser),
                            new SqlParameter("@Channel", pushNotifReq.Browser),
                            new SqlParameter("@Device", pushNotifReq.Device),
                            new SqlParameter("@NotificationType", pushNotifReq.NotificationType),
                            new SqlParameter("@Topic", pushNotifReq.Topic),
                            new SqlParameter("@CustomerType", pushNotifReq.CustomerType),
                            new SqlParameter("@Token", pushNotifReq.Token),
                        };

                        SqlResModel insertToken = SqlDbHelper.InsertWithParam(ConstantManager.EOneConnString(), "proc_InsertToken", CommandType.StoredProcedure, sqlParams);

                        if (insertToken.ResponseCode == "00" && insertToken.ResultInt > 0)
                        {
                            resp.ResponseCode = "00";
                            resp.TokenCount = insertToken.ResultInt;
                            resp.ResponseDescription = "successful";
                        }
                        else
                        {
                            resp.ResponseCode = "01";
                            resp.TokenCount = insertToken.ResultInt;
                            resp.ResponseDescription = "failed";
                        }

                    }
                    
                }

            }
            catch (Exception ex)
            {
                ErrHandler.Log(pushNotifReq.CustomerId + " Error encounterd: " + ex.Message + " " + ex.StackTrace);
                resp.ResponseCode = "99";
                resp.ResponseDescription = "Exception thrown";
            }
            
            resp.CustomerId = pushNotifReq.CustomerId;
            return resp;
        }

        public PushNotifResp SendPushNotification(SendPushNotifReq req)
        {
            PushNotifResp res = new PushNotifResp();

            //Send Notif here
            var tokens = req.Tokens;

            var pushSent =  PushNotificationLogic.SendPushNotification(tokens, req.MesssageTitle, req.MesssageBody, req.Image).Result;

            if (pushSent)
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@Title", req.MesssageTitle),
                    new SqlParameter("@Body", req.MesssageBody),
                    new SqlParameter("@Image", req.Image),
                    new SqlParameter("@SentStatus", 1),
                };

                SqlResModel insertToken = SqlDbHelper.InsertWithParam(ConstantManager.EOneConnString(), "proc_InsertPushMessage", CommandType.StoredProcedure, sqlParams);

                if (insertToken.ResponseCode == "00" && insertToken.ResultInt > 0)
                {
                    ErrHandler.Log("Push Notification Sent and logged successfully on PUshMessages DB for target token Ids");
                }
                else
                {
                    ErrHandler.Log("Push Notification successfully Sent to target token Ids but an error occured while trying to log the Message on PushMessages DB");
                }
                
                res.ResponseCode = "00";
                res.ResponseDescription = "successful";
                res.TokenCount = tokens.Length;
                res.CustomerId = req.CustomerId;
                res.SentStatus = 1;
            }
            else
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@Title", req.MesssageTitle),
                    new SqlParameter("@Body", req.MesssageBody),
                    new SqlParameter("@Image", req.Image),
                    new SqlParameter("@SentStatus", 0),
                };

                SqlResModel insertToken = SqlDbHelper.InsertWithParam(ConstantManager.EOneConnString(), "proc_InsertPushMessage", CommandType.StoredProcedure, sqlParams);

                if (insertToken.ResponseCode == "00" && insertToken.ResultInt > 0)
                {
                    ErrHandler.Log("Failed to send Push Notification for target token Ids. Messages details has been logged to the PushMessages DB");
                }
                else
                {
                    ErrHandler.Log("Failed in logging Message details for the failed Push Notification");
                }

                res.ResponseCode = "01";
                res.ResponseDescription = "failed";
                res.TokenCount = 0;
                res.CustomerId = req.CustomerId;
                res.SentStatus = 0;
            }

            return res;
        }

        public GetUserTokensResp GetUserTokens(string CustomerId)
        {
            GetUserTokensResp res = new GetUserTokensResp();
            string[] tokens = null;

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("CustomerId", CustomerId),
                           
            };

            SqlResModel selectTokens = SqlDbHelper.SelectWithParam(ConstantManager.EOneConnString(), "proc_GetUserToken", CommandType.StoredProcedure, param);

            if (selectTokens.ResponseCode == "00" && selectTokens.ResultDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < selectTokens.ResultDataTable.Rows.Count; i++)
                {
                    tokens[i] = Convert.ToString(selectTokens.ResultDataTable.Rows[i]["TokenId"]);
                }

                res.ResponseCode = "00";
                res.ResponseDescription = "successful";
            }
            else
            {
                res.ResponseCode = "01";
                res.ResponseDescription = "failed";
            }

            res.Tokens = tokens;
            return res;
        }

        public GetAllTokensResp GetAllTokens()
        {
            GetAllTokensResp res = new GetAllTokensResp();
            string[] tokens = null;

            SqlResModel selectTokens = SqlDbHelper.SelectWithParam(ConstantManager.EOneConnString(), "proc_GetAllTokens", CommandType.StoredProcedure, null);

            if (selectTokens.ResponseCode == "00" && selectTokens.ResultDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < selectTokens.ResultDataTable.Rows.Count; i++)
                {
                    tokens[i] = Convert.ToString(selectTokens.ResultDataTable.Rows[i]["TokenId"]);
                }

                res.ResponseCode = "00";
                res.ResponseDescription = "successful";
            }
            else
            {
                res.ResponseCode = "01";
                res.ResponseDescription = "failed";
            }

            res.Tokens = tokens;
            return res;
        }

    }

}
