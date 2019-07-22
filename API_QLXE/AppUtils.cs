using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_QLXE
{
    public class AppUtils
    {
        private const bool IsDebugMode = true;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Log(string s)
        {

            if (IsDebugMode)
            {
                //Dev log at local
                string client_ip = null;
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.IsLocal)
                    {
                        System.Diagnostics.Debug.Write(" [" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "] : " + s + '\n');
                    }

                    //Log to file

                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        client_ip = HttpContext.Current.Request.UserHostAddress;
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (string.IsNullOrEmpty(client_ip))
                        logger.Info(s);
                    else
                        logger.Info("[IP:" + client_ip + "]-" + s);
                }
            }
        }
    }
}