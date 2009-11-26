namespace NDDDSample.Interfaces.HandlingService.Host
{
    #region Usings

    using System;
    using IoC;

    #endregion

    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Starting HandlingService.Host");

            using (ContainerBuilder.Build())
            {
                Console.WriteLine("HandlingService.Host Started, hit Enter to close");
                Console.ReadLine();
            }
        }
    }
}