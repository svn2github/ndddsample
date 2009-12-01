namespace NDDDSample.Interfaces.HandlingService.Host
{
    #region Usings

    using System;
    using System.IO;
    using Application;
    using Castle.Windsor;
    using FolderScaner;
    using IoC;

    #endregion

    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Starting HandlingService.Host");

            IWindsorContainer container = ContainerBuilder.Build();
            using (container)
            {
                var applicationEvents = container.Resolve<IApplicationEvents>();

                // Scan every 5 sec
                var directoryScanner = new UploadDirectoryScanner(
                    5 * 1000,
                    applicationEvents,
                    new DirectoryInfo("C:\\NdddScanner"),
                    new DirectoryInfo("C:\\NdddScanner\\ParseFailure"));

                directoryScanner.Run();
                Console.WriteLine("HandlingService.Host Started, hit Enter to close");
                Console.ReadLine();
                directoryScanner.CancelTask();
            }
        }
    }
}