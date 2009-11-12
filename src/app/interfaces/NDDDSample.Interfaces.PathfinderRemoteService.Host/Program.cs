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
            Console.WriteLine("Starting PathfinderRemoteService.Host");

            using (ContainerBuilder.Build())
            {
                Console.WriteLine("PathfinderRemoteService.Host Started, hit Enter to close");
                Console.ReadLine();
            }
        }
    }
}