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
            Console.WriteLine("Starting BookingRemoteService.Host, hit Enter to close");

            using (ContainerBuilder.Build())
            {
                Console.ReadLine();
            }
        }     
    }    
}