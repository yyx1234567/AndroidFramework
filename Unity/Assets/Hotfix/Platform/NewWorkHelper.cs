using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
namespace ETHotfix
{
    public static class NewWorkHelper
    {
        public static DateTime GetCurrentTime()
        {
            WebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=1");
            DateTime dateTime = default(DateTime);
            try
            {
                WebResponse response = myHttpWebRequest.GetResponse();
                string TimeString = response.Headers["date"];
                return DateTime.Parse(DateTime.Parse(TimeString).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.StackTrace);
                return DateTime.Now;
            }
        }
    }
}