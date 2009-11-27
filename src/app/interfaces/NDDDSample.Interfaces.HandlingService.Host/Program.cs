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
                var directoryScanner = new UploadDirectoryScanner(applicationEvents, new DirectoryInfo("C:\\temp\\DDD1"),
                                                                  new DirectoryInfo("C:\\temp\\DDD1\\ParseFailure"));

                directoryScanner.Run();
                Console.WriteLine("HandlingService.Host Started, hit Enter to close");
                Console.ReadLine();
                directoryScanner.CancelTask();
            }
        }
    }
}