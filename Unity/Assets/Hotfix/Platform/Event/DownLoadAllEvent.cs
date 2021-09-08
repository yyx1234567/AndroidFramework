using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 下载全部事件
    /// </summary>
    [Event(EventIdType.DownLoadAllEvent)]
    public sealed class DownLoadAllEvent : AEvent
    {
        public override async void Run()
        {
            
        }
    }
}