namespace NDDDSample.Interfaces.BookingRemoteService.Host
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Rhino.Commons;

    public class UnitOfWorkContext : ICallContextInitializer
    {
        private IUnitOfWork UnitOfWork;

        public object BeforeInvoke(InstanceContext instanceContext,
                                   IClientChannel channel,
                                   Message message)
        {
            UnitOfWork = Rhino.Commons.UnitOfWork.Start();
            return null;
        }

        public void AfterInvoke(object correlationState)
        {
            if (UnitOfWork != null)
            {
                UnitOfWork.Dispose();
                UnitOfWork = null;
            }
        }
    }

}
