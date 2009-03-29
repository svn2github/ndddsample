namespace NDDDSample.Domain.Model.Cargos
{
    #region Usings

    using NDDDSample.Domain.Shared;

    #endregion

    public class Cargo : IEntity<Cargo>
    {


        #region Implementation of IEntity<Cargo>

        /// <summary>
        /// Entities compare by identity, not by attributes.
        /// </summary>
        /// <param name="other">The other entity.</param>
        /// <returns>true if the identities are the same, regardles of other attributes.</returns>
        public bool sameIdentityAs(Cargo other)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}