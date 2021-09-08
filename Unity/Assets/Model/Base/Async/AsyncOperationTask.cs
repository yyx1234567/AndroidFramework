using System;
using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ETModel
{
    public class AsyncOperationTask
    {
        public AsyncOperation Handle;

        public AsyncOperationTask(AsyncOperation tween)
        {
            Handle = tween;
        }
        public AsyncOperationAwaiter GetAwaiter()
        {
            return new AsyncOperationAwaiter(this);
        }

    }

    public class AsyncOperationAwaiter : INotifyCompletion
    {
        public AsyncOperationTask Task;
        public AsyncOperationAwaiter(AsyncOperationTask tween)
        {
            Task = tween;
        }

        public bool IsCompleted => Task.Handle.isDone;

        public void GetResult()
        {

        }
        public void OnCompleted(Action continuation)
        {
            if (IsCompleted)
            {
                continuation?.Invoke();
            }
            else
            {
                Task.Handle.completed += (x) => { continuation?.Invoke(); };
            }
        }
    }
}