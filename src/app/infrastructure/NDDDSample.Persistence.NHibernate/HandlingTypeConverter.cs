namespace NDDDSample.Persistence.NHibernate
{
    #region Usings

    using Domain.Model.Handlings;

    #endregion

    public class HandlingTypeConverter : EnumerationTypeConverter<HandlingEvent.HandlingType> {}
}