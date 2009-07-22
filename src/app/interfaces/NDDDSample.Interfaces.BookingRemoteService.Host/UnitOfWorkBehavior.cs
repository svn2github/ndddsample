namespace NDDDSample.Interfaces.BookingRemoteService.Host
{
    #region Usings

    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    #endregion

    public class UnitOfWorkBehavior : IServiceBehavior
    {
        public void ApplyDispatchBehavior(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {
            foreach (var cdb in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = cdb as ChannelDispatcher;
                if (null != channelDispatcher)
                {
                    foreach (var endpointDispatcher in
                        channelDispatcher.Endpoints)
                    {
                        foreach (var dispatchOperation in
                            endpointDispatcher.DispatchRuntime.Operations)
                        {
                            dispatchOperation.CallContextInitializers
                                .Add(new UnitOfWorkContext());
                        }
                    }
                }
            }
        }

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters) {}

        public void Validate(ServiceDescription serviceDescription,
                             ServiceHostBase serviceHostBase) {}
    }
}