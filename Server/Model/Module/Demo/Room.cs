using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETModel
{
    public class Room: Entity
    {
        public GameManager GameInstance;

        public List<Player> playersSeets = new List<Player>();

        public int RoomCaptain = 5;
        public int Count => playersSeets.Count;

        public string RoomName;

        public bool Add(Player player)
        {
            if (playersSeets.Count == 5)
            {
                return false;
            }
            playersSeets.Add(player);
            player.m_Room = this;
            return true;
        }

        public void Remove(Player player)
        {
            playersSeets.Remove(player);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

 
            for (int i = 0; i < playersSeets.Count; i++)
            {
                if (playersSeets[i] != null)
                {
                    playersSeets[i].Dispose();
                    playersSeets[i] = null;
                }
            }
            GameInstance.Dispose();
            playersSeets = null;
         }

        public void StartGame()
        {
            if (GameInstance == null)
            {
                GameInstance = new GameManager(this);
                GameInstance.StartGame();
            }
        }
     }
}
