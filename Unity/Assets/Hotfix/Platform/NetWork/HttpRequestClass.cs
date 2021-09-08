using ETModel;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
namespace ETHotfix
{
    public class HttpRequestClass
    {    
        /// <summary>
        /// 浏览器
        ///// </summary>
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public static async ETTask<HttpWebResponse> GetHttpWebResponseMethodIsGetAsync(string url, string token = null)
        {
            //Url 为空
             if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            UnityEngine.Debug.LogError(url);
            //创建请求
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //请求方式
            request.Method = "GET";
            //user-agentHttp标头值
            request.UserAgent = DefaultUserAgent;

            //判断是否存在超时设置的值
           request.Timeout = 10000; //10秒

            if (!string.IsNullOrEmpty(token))
            {
                if (token.Contains("bearer"))
                {
                    request.Headers.Add("Authorization", token);
                }
                else
                {
                    request.Headers.Add("Authorization", "bearer " + token);
                }
            }
            return await request.GetResponseAsync() as HttpWebResponse;
        }
    }
}
