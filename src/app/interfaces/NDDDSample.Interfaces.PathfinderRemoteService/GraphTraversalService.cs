namespace NDDDSample.Interfaces.PathfinderRemoteService
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Common;

    #endregion

    public class GraphTraversalService : IGraphTraversalService
    {
        private readonly GraphDAO dao;
        private readonly Random random;
        private const long OneMinMs = 1000 * 60;
        private const long OneDayMs = OneMinMs * 60 * 24;


        public GraphTraversalService(GraphDAO dao)
        {
            this.dao = dao;
            random = new Random();
        }

        public IList<TransitPath> FindShortestPath(string originUnLocode,
                                                   string destinationUnLocode,
                                                   IDictionary<string, string> limitationProperties)
        {
            DateTime date = NextDate(DateTime.Now);

            IList<string> allVertices = dao.ListLocations();
            allVertices.Remove(originUnLocode);
            allVertices.Remove(destinationUnLocode);

            int candidateCount = GetRandomNumberOfCandidates();
            IList<TransitPath> candidates = new List<TransitPath>(candidateCount);

            for (int i = 0; i < candidateCount; i++)
            {
                allVertices = GetRandomChunkOfLocations(allVertices);
                IList<TransitEdge> transitEdges = new List<TransitEdge>(allVertices.Count - 1);
                string firstLegTo = allVertices[0];

                DateTime fromDate = NextDate(date);
                DateTime toDate = NextDate(fromDate);
                date = NextDate(toDate);

                transitEdges.Add(new TransitEdge(
                                     dao.GetVoyageNumber(originUnLocode, firstLegTo),
                                     originUnLocode, firstLegTo, fromDate, toDate));

                for (int j = 0; j < allVertices.Count - 1; j++)
                {
                    string curr = allVertices[j];
                    string next = allVertices[j + 1];
                    fromDate = NextDate(date);
                    toDate = NextDate(fromDate);
                    date = NextDate(toDate);
                    transitEdges.Add(new TransitEdge(dao.GetVoyageNumber(curr, next), curr, next, fromDate, toDate));
                }

                string lastLegFrom = allVertices[allVertices.Count - 1];
                fromDate = NextDate(date);
                toDate = NextDate(fromDate);
                transitEdges.Add(new TransitEdge(
                                     dao.GetVoyageNumber(lastLegFrom, destinationUnLocode),
                                     lastLegFrom, destinationUnLocode, fromDate, toDate));

                candidates.Add(new TransitPath(transitEdges));
            }

            return candidates;
        }

        private DateTime NextDate(DateTime date)
        {
            return new DateTime(date.Millisecond + OneDayMs + (random.Next(1000) - 500) * OneMinMs);
        }

        private int GetRandomNumberOfCandidates()
        {
            return 3 + random.Next(3);
        }

        private List<string> GetRandomChunkOfLocations(IList<string> allLocationsPrm)
        {
            var allLocations = Shuffle(allLocationsPrm);
            int total = allLocations.Count;
            int chunk = total > 4 ? 1 + new Random().Next(5) : total;
            return allLocations.GetRange(0, chunk);
        }

        private List<T> Shuffle<T>(IList<T> list)
        {
            var shuffledList = new List<T>(list.Count);
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                int j = random.Next(list.Count);
                shuffledList.Add(list[j]);
                list.Remove(list[j]);
            }
            return shuffledList;
        }
    }
}