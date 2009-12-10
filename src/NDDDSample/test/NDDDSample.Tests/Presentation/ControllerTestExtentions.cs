namespace NDDDSample.Tests.Presentation
{
    #region Usings

    using System.Web.Mvc;

    #endregion

    public static class ControllerTestExtentions
    {
        /// <summary>
        /// Extention method which return Model from Action Result.
        /// </summary>
        /// <typeparam name="T">Model Type</typeparam>
        /// <param name="actionResult">Action Result</param>
        /// <returns>Model Instance</returns>
        public static T GetModel<T>(this ActionResult actionResult) where T : class
        {
            var viewResult = actionResult as ViewResult;
            return viewResult == null ? null : viewResult.ViewData.Model as T;
        }
    }
}