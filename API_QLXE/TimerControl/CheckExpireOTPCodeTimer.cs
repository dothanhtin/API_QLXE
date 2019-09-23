using API_QLXE.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace API_QLXE.TimerControl
{
    public class CheckExpireOTPCodeTimer
    {
        public readonly Timer checkExpireOTPCodeTimer;
        public int checkExpireInterval;
        public CheckExpireOTPCodeTimer()
        {
            checkExpireOTPCodeTimer = new Timer { AutoReset = true, Enabled = true, Interval = 86400000 };
        }
        public async Task Start()
        {
            await Task.Run(() =>
            {
                checkExpireInterval = int.Parse(ConfigurationManager.AppSettings["checkExpireInterval"]);
                checkExpireOTPCodeTimer.Interval = checkExpireInterval;
                checkExpireOTPCodeTimer.Start();
                checkExpireOTPCodeTimer.Elapsed += CheckExpireOTPFunc;
            });
        }

        public void CheckExpireOTPFunc(object o, ElapsedEventArgs e)
        {
            QLXEContext cn = new QLXEContext();
            try
            {
                var res =(from item1 in cn.Otps.AsQueryable().ToList()
                         where DateTime.Now.Subtract(item1.CREATEDTIME).TotalMinutes>5 
                         select item1).ToList();
                foreach(var item in res)
                {
                    cn.Otps.Remove(item);
                }
                cn.SaveChanges();
                if (res.Count > 0)
                    AppUtils.Log("CheckExpireOTPFunc deleted some items.");
                AppUtils.Log("CheckExpireOTPFunc deleted no items.");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}