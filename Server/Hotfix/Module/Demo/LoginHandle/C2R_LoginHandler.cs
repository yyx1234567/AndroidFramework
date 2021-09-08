using System;
using System.Collections.Generic;
using System.Net;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2R_LoginHandler : AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask RunAsync(Session session, C2R_Login request, R2C_Login response, Action reply)
        {
            DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
            //验证提交来的的账号和密码
            List<ComponentWithId> result = await dbProxy.Query<AccountInfo>($"{{Account:'{request.Account}',Password:'{request.Password}'}}");

            if (result.Count != 1)
            {
                response.Error = ErrorCode.ERR_AccountOrPasswordError;
                reply();
                return;
            }

            AccountInfo account = (AccountInfo)result[0];

            int GateAppId;
            StartConfig config;
            //获取账号所在区服的AppId 索取登陆Key
            if (StartConfigComponent.Instance.GateConfigs.Count == 1)
            { 
                //只有一个Gate服务器时当作AllServer配置处理
                config = StartConfigComponent.Instance.StartConfig;
            }
            else
            {
                //有多个Gate服务器时当作分布式配置处理
                GateAppId = RealmHelper.GetGateAppIdFromUserId(account.Id);
                config = StartConfigComponent.Instance.GateConfigs[GateAppId - 1];
            }
            IPEndPoint innerAddress = config.GetComponent<InnerConfig>().IPEndPoint;
            Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(innerAddress);
            // 向gate请求一个key,客户端可以拿着这个key连接gate
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await gateSession.Call(new R2G_GetLoginKey() { Account = request.Account });

            string outerAddress = config.GetComponent<OuterConfig>().Address2;

            response.Address = outerAddress;
            response.Key = g2RGetLoginKey.Key;

            reply();
        }
    }
}