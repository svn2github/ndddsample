namespace NDDDSample.Interfaces.HandlingService.WebService
{
    #region Usings

    using System.ServiceModel;

    #endregion

    [ServiceContract]
    public interface IHandlingReportService
    {
        [OperationContract, FaultContract(typeof (HandlingReportException))]
        void SubmitReport(HandlingReport handlingReport);
    }
}