using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class UserInfoAwakeSystem : AwakeSystem<UserInfo, string>
    {
        public override void Awake(UserInfo self, string name)
        {
            self.Awake(name);
        }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : Entity
    {
        public string UserName { get; set; }

         public int LastPlay { get; set; }

        //public List<Ca>
        public void Awake(string name)
        {
            UserName = name;
             LastPlay = 0;
        }

    }
}