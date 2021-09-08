using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public  class GameManagerComponent
    {
        public Dictionary<Room, GameManager> allGames = new Dictionary<Room, GameManager>();

         public void AddGameManger(GameManager game)
        {
            allGames.Add(game.room,game);
        }
    }
}
