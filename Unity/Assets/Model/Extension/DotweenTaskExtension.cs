using System;
 using DG.Tweening;
using System.Runtime.CompilerServices;
 
namespace ETModel
{
    public static class DotweenTaskExtension
    {
        public static DotweenAwaiter GetAwaiter(this Tween tween)
        {
            return new DotweenAwaiter(tween);
        }
     }

    public class DotweenAwaiter : INotifyCompletion
    {
        public Tween Task;
        public DotweenAwaiter(Tween tween)
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
                Task.onComplete +=()=> {continuation?.Invoke(); };
            }
        }
     }
}