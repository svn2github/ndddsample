namespace NDDDSample.Interfaces.HandlingService.Host.Wcf
{
    #region Usings

    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Rhino.Commons;

    #endregion

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
            //TODO:revise UoW logic
            if (UnitOfWork != null)
            {
                UnitOfWork.Dispose();
                UnitOfWork = null;
            }
        }
    }
}