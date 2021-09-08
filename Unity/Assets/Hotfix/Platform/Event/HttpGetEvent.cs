using ETModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

namespace ETHotfix
{
    [Event(EventIdType.HttpGetEvent)]
    public class HttpGetEvent : AEvent<string,string>
    {
        public override void Run(string url,string token)
        {
             
        }
    }
}
