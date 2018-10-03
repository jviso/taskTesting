using System;
using System.Threading.Tasks;
using System.Threading;

namespace taskTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var bad = new Bad();
            Console.WriteLine("Before bad.OuterTask in Main. We'll wait here for a while because of the awaited async method call in bad.OuterTask.");
            bad.OuterTask();
            Console.WriteLine("After bad.OuterTask in Main.");

            var good = new Good();
            Console.WriteLine("Before good.OuterTask in Main. We won't wait long because of the returned task in good.OuterTask.");
            good.OuterTask();
            Console.WriteLine("After good.OuterTask in Main. Notice that the good.LongRunningInnerProcess hasn't finished yet.");

            Console.ReadLine();
        }
    }

    class Bad
    {
        public async Task OuterTask()
        {
            await LongRunningInnerProcess();
            Console.WriteLine("After awaited call in bad.OuterTask.");
        }

        private async Task LongRunningInnerProcess()
        {
            Thread.Sleep(10000);
            await Task.Run(() => Console.WriteLine("Bad long running process done."));
        }
    }

    class Good
    {
        public async Task OuterTask()
        {
            await Task.Run(() => LongRunningInnerProcess());
            Console.WriteLine("After awaited call in good.OuterTask.");
        }

        private void LongRunningInnerProcess()
        {
            Thread.Sleep(10000);
            Console.WriteLine("Good long running process done.");
        }
    }
}
