namespace NDDDSample.Interfaces.PathfinderRemoteService.Host
{
    #region Usings

    using System;
    using IoC;

    #endregion

    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting PathfinderRemoteService.Host, hit Enter to close");

            using (ContainerBuilder.Build())
            {
                Console.ReadLine();
            }
        }
    }
}