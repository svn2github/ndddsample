namespace NDDDSample.Interfaces.BookingRemoteService.Host
{
    #region Usings

    using System;
    using IoC;

    #endregion

    public static class Program
    {       
        public static void Main()
        {
            Console.WriteLine("Starting BookingRemoteService.Host");

            using (ContainerBuilder.Build())
            {
                Console.WriteLine("BookingRemoteService.Host Started, hit Enter to close");
                Console.ReadLine();
            }
        }     
    }    
}