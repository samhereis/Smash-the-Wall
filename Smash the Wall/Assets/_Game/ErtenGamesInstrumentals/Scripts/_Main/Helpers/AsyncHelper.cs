using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Helpers
{
    public static class AsyncHelper
    {
#if UNITY_2023_2_OR_NEWER
        public static async Awaitable Skip()
        {
            await Awaitable.WaitForSecondsAsync(0);
        }

        public static async Awaitable NextFrame()
        {
            await Awaitable.NextFrameAsync();
        }

        public static async Awaitable DelayFloat(float delay)
        {
            await Awaitable.WaitForSecondsAsync(delay);
        }

        public static async Awaitable DelayInt(int delay)
        {
            int duration = (int)Mathf.Max(delay * 1000, 0);

            await Awaitable.WaitForSecondsAsync(duration, cancellationToken);
        }

        public static async Awaitable FromAsyncOperation(AsyncOperation asyncOperation)
        {
            await Awaitable.FromAsyncOperation(asyncOperation));
        }
#else
        public static async Task Skip()
        {
            await Task.Yield();
        }

        public static async Task NextFrame()
        {
            await Task.Delay(40);
        }

        public static async Task DelayFloat(float delay)
        {
            int duration = (int)Mathf.Max(delay * 1000, 0);

            await Task.Delay(duration);
        }

        public static async Task DelayInt(int delay)
        {
            await Task.Delay(delay);
        }

        public static async Task FromAsyncOperation(AsyncOperation asyncOperation)
        {
            while (asyncOperation != null && asyncOperation.isDone == false)
            {
                await Skip();
            }
        }
#endif
    }
}