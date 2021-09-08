using System;
 using DG.Tweening;
using System.Runtime.CompilerServices;
 
namespace ETModel
{
    public class PanelSelectTask<T>
    {
        public System.Action<T> onComplete;

        public bool IsComplete()
        {
             return false;
        }
        public T Reslut;

       
        public PanelSelectTaskAwaiter<T> GetAwaiter()
        {
            return new PanelSelectTaskAwaiter<T>(this);
        }
      }

    public class PanelSelectTaskAwaiter <T>: INotifyCompletion
    {
        public PanelSelectTask<T> Task;
        public PanelSelectTaskAwaiter(PanelSelectTask<T> tween)
        {
            Task = tween;
        }

        public bool IsCompleted => Task.IsComplete();

        public void GetResult()
        {
            
        }
        public  void OnCompleted(Action continuation)
        {
            if (IsCompleted)
            {
                continuation?.Invoke();
            }
            else
            {
                   Task.onComplete +=(x)=> {
                       Task.Reslut = x;
                      continuation?.Invoke(); };
             }
        }
     }
}