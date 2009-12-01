namespace NDDDSample.Interfaces.PathfinderRemoteService.Host.Wcf
{
    #region Usings

    using System;
    using System.Collections;
    using System.ServiceModel.Channels;
    using Castle.Facilities.WcfIntegration.Behaviors;

    #endregion

    public class LifestyleMessageAction : AbstractMessageAction
    {
        public LifestyleMessageAction()
            : base(MessageLifecycle.All) {}

        public override bool Perform(ref Message message, MessageLifecycle lifecycle, IDictionary state)
        {
            string action = "<Unknown Action>";

            if (message != null && message.Headers != null && message.Headers.Action != null)
            {
                action = message.Headers.Action;
            }

            if (lifecycle == MessageLifecycle.IncomingRequest || lifecycle == MessageLifecycle.OutgoingResponse)
            {
                Console.WriteLine("Perform called at lifecycle: {0} - {1}", lifecycle, action);
            }

            return true;
        }
    }
}