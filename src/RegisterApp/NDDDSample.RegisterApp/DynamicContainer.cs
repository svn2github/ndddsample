namespace NDDDSample.RegisterApp
{
    #region Usings

    using Castle.Windsor;
    using Castle.Windsor.Configuration.Interpreters;

    #endregion

    /// <summary>
    /// The dynamic container.
    /// </summary>
    public sealed class DynamicContainer
    {
        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="DynamicContainer"/> class from being created.
        /// </summary>
        private DynamicContainer() {}

        #endregion

        #region Properties

        /// <summary>
        /// Gets Instance.
        /// </summary>
        public static WindsorContainer Instance
        {
            get { return Nested.instance; }
        }

        #endregion

        /// <summary>
        /// The nested.
        /// </summary>
        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit

            #region Constants and Fields

            /// <summary>
            /// The instance.
            /// </summary>
            internal static readonly WindsorContainer instance =
                new WindsorContainer(new XmlInterpreter("Windsor.config"));

            #endregion
        }
    }
}