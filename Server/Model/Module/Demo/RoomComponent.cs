using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class RoomAwakeComponent : AwakeSystem<RoomComponent>
    {
        public override void Awake(RoomComponent self)
        {
            self.Awake();
        }
    }

    public class RoomComponent : Component
    {
        private Dictionary<long, Room> roomdic = new Dictionary<long, Room>();

        private long lastId;

        public void Add(Room room)
        {
            lastId++;
            room.Id = lastId;
            roomdic.Add(lastId, room);
        }

        public List<Room> GetAllRoom()
        {
            return roomdic.Select(x=>x.Value).ToList();
        }

 

        public Room GetRoom(long id)
        {
            if (!roomdic.TryGetValue(id, out Room result))
            {
                Log.Info($"获取房间失败 找不到ID为{id}的房间");
            }
            return result;
        }
 

        public void RemoveRoom(long id)
        {
            if (!roomdic.Remove(id))
            {
                Log.Info($"移除失败 找不到ID为{id}的房间");
            }
        }

        internal void Awake()
        {
            roomdic = new Dictionary<long, Room>();
        }
    }
}
