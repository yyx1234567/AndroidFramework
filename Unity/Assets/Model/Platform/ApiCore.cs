using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 using System.Linq;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ETModel
{
    public static class ApiCore
    {
        public static string Url_Login = "http://192.168.50.131:44399/connect/token";
        public static string Url_Configuration = "http://192.168.50.131:30096/api/abp/application-configuration";

        public static object Api_get_useraids { get; set; }

        internal static string HttpRequestGetString(string v)
        {
            return "";
        }

        public static   string  HttpRequestGetAsyncUrlDym(string v, bool token,string returnData)
        {

            return returnData;
        }
    }
}
 
