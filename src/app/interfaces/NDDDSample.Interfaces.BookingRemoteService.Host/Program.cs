namespace NDDDSample.Interfaces.BookingRemoteService.Host
{
    #region Usings

    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using Castle.Facilities.WcfIntegration;
    using Castle.Windsor;
    using Castle.Windsor.Configuration.Interpreters;
    using Common;
    using Rhino.Commons;

    #endregion

    public class Program
    {
        internal static ServiceHost myServiceHost;
        private static WindsorContainer WindsorContainer;

        internal static void StartService()
        {            
            Uri baseAddress = new Uri("http://localhost:8080/BookingServiceFacade");

            //Instantiate new ServiceHost 
            myServiceHost = new ServiceHost(typeof (BookingServiceFacade), baseAddress);

            var smb = new ServiceMetadataBehavior {HttpGetEnabled = true};
            myServiceHost.Description.Behaviors.Add(smb);

            // Add MEX endpoint
            myServiceHost.AddServiceEndpoint(
                ServiceMetadataBehavior.MexContractName,
                MetadataExchangeBindings.CreateMexHttpBinding(),
                "mex"
                );

            // Create a BasicHttpBinding instance
            BasicHttpBinding binding = new BasicHttpBinding();

            // Add a service endpoint using the created binding
            myServiceHost.AddServiceEndpoint(typeof (IBookingServiceFacade), binding, "BookingServiceFacade");

            //Open myServiceHost
            myServiceHost.Open();
        }

        internal static void StopService()
        {
            //Call StopService from your shutdown logic (i.e. dispose method)
            if (myServiceHost.State != CommunicationState.Closed)
            {
                myServiceHost.Close();
            }
        }

        public static void Main()
        {            
            DependencyContainer();
            StartService();
            Console.WriteLine("Booking Remote Service has started");
            Console.ReadLine();
            StopService();
        }

        public static void DependencyContainer()
        {
            WindsorContainer = new WindsorContainer(
                new XmlInterpreter());

            // For Windsor WCF facility
            DefaultServiceHostFactory
                .RegisterContainer(WindsorContainer.Kernel);

            // For Rhino Commons
            IoC.Initialize(WindsorContainer);
        }
    }
}