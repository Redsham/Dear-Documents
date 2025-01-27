using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

namespace Utility
{
    public static class EventWaiter
    {
        public static async UniTask Wait(
            Action<Action>    eventSubscriber,
            Action<Action>    eventUnsubscriber,
            CancellationToken cancellationToken = default)
        {
            UniTaskCompletionSource completionSource = new();

            void Handler()
            {
                eventUnsubscriber(Handler);
                completionSource.TrySetResult();
            }

            // Subscribe to the event
            eventSubscriber(Handler);

            try { await completionSource.Task.AttachExternalCancellation(cancellationToken); }
            finally { eventUnsubscriber(Handler); }
        }
        
        public static async UniTask WaitUnityEvent(
            UnityEvent @event,
            CancellationToken       cancellationToken = default)
        {
            UniTaskCompletionSource completionSource = new();

            void Handler()
            {
                @event.RemoveListener(Handler);
                completionSource.TrySetResult();
            }

            // Subscribe to the event
            @event.AddListener(Handler);

            try { await completionSource.Task.AttachExternalCancellation(cancellationToken); }
            finally { @event.RemoveListener(Handler); }
        }
    }
}