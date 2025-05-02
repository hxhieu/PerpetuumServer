using Perpetuum.Bootstrapper;

namespace Perpetuum.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var gameRoot = args[0];
            if (!Directory.Exists(gameRoot))
            {
                throw new Exception($"GameRoot folder was not found: {gameRoot}");
            }

            var bootstrapper = new PerpetuumBootstrapper();

            bootstrapper.Init(gameRoot);

            if (bootstrapper.TryInitUpnp(out bool upnpSuccess))
            {
                if (!upnpSuccess)
                {
                    //System Error Codes (500-999)
                    // signal upnp attempt error with custom errorcode
                    throw new Exception(upnpSuccess.ToString());
                }
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    Console.WriteLine("");
                    Console.WriteLine("STOPPING HOST IN 4 SECONDS");
                    Console.WriteLine("");

                    eventArgs.Cancel = true;
                    bootstrapper.Stop(TimeSpan.FromSeconds(4));
                };
                bootstrapper.Start();
                bootstrapper.WaitForStop();
            }
        }
    }
}
