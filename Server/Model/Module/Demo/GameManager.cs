using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ETModel
{
    public class GameManager : IDisposable
    {
        public int CurrentGameTime;

        public Dictionary<string, float> DataDic = new Dictionary<string, float>();
        
        public Room room;

        private CancellationTokenSource tokenSource;

        public GameManager(Room room)
        {
            this.room = room;
            DataDic.Add("数据1",100);
            DataDic.Add("数据2", 100);
            DataDic.Add("数据3", 100);
            DataDic.Add("数据4", 100);
            DataDic.Add("数据5", 100);
            tokenSource = new CancellationTokenSource();
         }

        public async void StartGame()
        {
            while (true)
            {
                 await TimerComponent.Instance.WaitAsync(1000, tokenSource.Token);
                 CurrentGameTime += 1;
            }
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            DataDic.Clear();
            CurrentGameTime = 0;
        }
    }
}
