namespace NDDDSample.Interfaces.PathfinderRemoteService.Common
{
    #region Usings

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Part of the external graph traversal API exposed by the routing team
    /// and used by us (booking and tracking team).
    /// </summary>
    public interface IGraphTraversalService
    {
        IList<TransitPath> FindShortestPath(string originUnLocode,
                                            string destinationUnLocode,
                                            IDictionary<string, string> limitationProperties);
    }
}