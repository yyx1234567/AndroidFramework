using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace ETModel
{
    public class WebRequestController : MonoBehaviour
    {
        public static WebRequestController Instance;

        private void Awake()
        {
            Instance = this;
        }
        public void Post(string url, WWWForm form, Dictionary<string, string> header, Action<string> action = null)
        {
            StartCoroutine(PostCoroutine(url, form, header, action));
        }

        public void Put(string url, string json, Dictionary<string, string> header, Action<string> action = null)
        {
            StartCoroutine(PostCoroutine(url, json, header, action));
        }


        public void Post(string url, string form, Dictionary<string, string> header, Action<string> action = null)
        {
            StartCoroutine(PostCoroutine(url, form, header, action));
        }

        public void Get(string url, string token, Action<string> action = null)
        {
            StartCoroutine(GetCoroutine(url, token, action));
        }

        private IEnumerator GetCoroutine(string url, string token, Action<string> action)
        {
            var webrequest = UnityWebRequest.Get(url);
            webrequest.SetRequestHeader("Content-Type", "application/json");
            if (token.Contains("bearer"))
            {
                webrequest.SetRequestHeader("Authorization", token);
            }
            else
            {
                webrequest.SetRequestHeader("Authorization", "bearer " + token);
            }
            yield return webrequest.SendWebRequest();
            var data = webrequest.downloadHandler.text;
            action?.Invoke(data);
        }

        private IEnumerator PostCoroutine(string url, WWWForm form, Dictionary<string, string> header, Action<string> action = null)
        {
            var webrequest = UnityWebRequest.Post(url, form);
            foreach (var item in header)
            {
                webrequest.SetRequestHeader(item.Key, item.Value);
            }
            yield return webrequest.SendWebRequest();
            var data = webrequest.downloadHandler.text;
            action?.Invoke(data);
        }

        private IEnumerator PostCoroutine(string url, string form, Dictionary<string, string> header, Action<string> action = null)
        {
            var webrequest = UnityWebRequest.Put(url, form);

            if (header != null)
            {
                foreach (var item in header)
                {
                    webrequest.SetRequestHeader(item.Key, item.Value);
                }
            }
            webrequest.SetRequestHeader("Content-Type", "application/json");
            if (Global.LoginUser.Token != null)
            {
                if (Global.LoginUser.Token.Contains("bearer"))
                {
                    webrequest.SetRequestHeader("Authorization", Global.LoginUser.Token);
                }
                else
                {
                    webrequest.SetRequestHeader("Authorization", "bearer " + Global.LoginUser.Token);
                }
            }
            yield return webrequest.SendWebRequest();
            var data = webrequest.downloadHandler.text;
            action?.Invoke(data);
        }
    }
}