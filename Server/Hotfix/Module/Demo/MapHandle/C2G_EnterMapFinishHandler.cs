using ETModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_EnterMapFinishHandler : AMHandler<C2G_EnterMapFinish>
	{
        protected override async ETTask Run(Session session, C2G_EnterMapFinish message)
        {
             await ETTask.CompletedTask;
         }

    }
}