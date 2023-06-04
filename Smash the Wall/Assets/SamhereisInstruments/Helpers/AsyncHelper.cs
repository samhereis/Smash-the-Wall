using System.Threading;
using System.Threading.Tasks;

namespace Helpers
{
    public static class AsyncHelper
    {
        public static async Task Delay()
        {
            await Task.Yield();
        }

        public static async Task Delay(float delay)
        {
            await Task.Delay((int)delay * 1000);
        }

        public static async Task Delay(float delay, CancellationToken cancellationToken)
        {
            await Task.Delay((int)delay * 1000, cancellationToken);
        }

        public static async Task Delay(int delay)
        {
            await Task.Delay(delay);
        }

        public static async Task Delay(int delay, CancellationToken cancellationToken)
        {
            await Task.Delay(delay, cancellationToken);
        }
    }
}
