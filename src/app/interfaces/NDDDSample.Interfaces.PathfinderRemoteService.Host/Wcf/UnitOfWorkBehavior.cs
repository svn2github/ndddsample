namespace NDDDSample.Interfaces.PathfinderRemoteService.Host.Wcf
{
    #region Usings

    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using ServiceDescription=System.Web.Services.Description.ServiceDescription;

    #endregion

    public class UnitOfWorkBehavior : IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription,
                             ServiceHostBase serviceHostBase) {}

        public void Validate(System.ServiceModel.Description.ServiceDescription serviceDescription,
                             ServiceHostBase serviceHostBase) {}

        public void AddBindingParameters(System.ServiceModel.Description.ServiceDescription serviceDescription,
                                         ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters) {}

        public void ApplyDispatchBehavior(System.ServiceModel.Description.ServiceDescription serviceDescription,
                                          ServiceHostBase serviceHostBase) {}
    }
}