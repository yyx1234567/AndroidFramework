using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.DB)]
	public class DBQueryJsonRequestHandler : AMRpcHandler<DBQueryJsonRequest, DBQueryJsonResponse>
	{
		protected override async ETTask RunAsync(Session session, DBQueryJsonRequest request, DBQueryJsonResponse response, Action reply)
		{
 			List<ComponentWithId> components = await Game.Scene.GetComponent<DBComponent>().GetJson(request.CollectionName, request.Json);
			response.Components = components;
  			reply();
			await ETTask.CompletedTask;
		}
	}
}