using ETModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace ETHotfix
{
     public class HttpRequestComponent : Component
    {
        /// 浏览器
        ///// </summary>
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";


        public async ETTask<string> HttpRequestGetString(string key, string param = null, bool token = false)
        {
            System.Net.HttpWebResponse response = null;
            if (token && ETModel.Global.LoginUser != null)
            {
                response =await Get(key, ETModel.Global.LoginUser.Token);
            }
            else
            {
                response = await Get(key);
            }

            if (response == null)
            {
                 return null;
            }

             if (response.ContentEncoding.ToLower().Contains("gzip"))
            {
                //deflate解压缩
                using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string _result = reader.ReadToEnd();
                        return _result;
                    }
                }
            }
            else
            {
                using (Stream _stream = response.GetResponseStream())
                {

                    using (StreamReader _responseStreamReader = new StreamReader(_stream, Encoding.UTF8))
                    {
                        string _result = _responseStreamReader.ReadToEnd();
                        return _result;
                    }
                }
            }
         }
        private  async ETTask<HttpWebResponse> Get(string url, string token = null)
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