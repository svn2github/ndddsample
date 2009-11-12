namespace NDDDSample.Infrastructure.ExternalRouting
{
    #region Usings

    using System.Collections.Generic;
    using System.ServiceModel;
    using Domain.Model.Cargos;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;
    using Domain.Service;
    using Interfaces.PathfinderRemoteService.Common;
    using Log;

    #endregion

    /// <summary>
    /// Our end of the routing service. This is basically a data model
    /// translation layer between our domain model and the API put forward
    /// by the routing team, which operates in a different context from us.
    /// </summary>
    public class ExternalRoutingService : IRoutingService
    {
        private readonly IGraphTraversalService graphTraversalService;
        private readonly ILocationRepository locationRepository;
        private readonly IVoyageRepository voyageRepository;
        private static readonly ILog log = LogFactory.GetApplicationLayerLogger();

        public ExternalRoutingService(IGraphTraversalService graphTraversalService, ILocationRepository locationRepository, IVoyageRepository voyageRepository)
        {
            this.graphTraversalService = graphTraversalService;
            this.locationRepository = locationRepository;
            this.voyageRepository = voyageRepository;
        }

        /// <summary>
        /// The RouteSpecification is picked apart and adapted to the external API.
        /// </summary>
        /// <param name="routeSpecification">Route Specification</param>
        /// <returns></returns>
        public IList<Itinerary> FetchRoutesForSpecification(RouteSpecification routeSpecification)
        {
            Location origin = routeSpecification.Origin;
            Location destination = routeSpecification.Destination;

            IDictionary<string, string> limitations = new Dictionary<string, string>();
            limitations.Add("DEADLINE", routeSpecification.ArrivalDeadline.ToString());

            IList<TransitPath> transitPaths;
            try
            {
                transitPaths = graphTraversalService.FindShortestPath(
                    origin.UnLocode.IdString,
                    destination.UnLocode.IdString,
                    limitations);
            }
            catch (FaultException e)
            {
                log.Error(e, e);
                return new List<Itinerary>();
            }

            //The returned result is then translated back into our domain model.
            IList<Itinerary> itineraries = new List<Itinerary>();

            foreach (TransitPath transitPath in transitPaths)
            {
                Itinerary itinerary = ToItinerary(transitPath);
                // Use the specification to safe-guard against invalid itineraries
                if (routeSpecification.IsSatisfiedBy(itinerary))
                {
                    itineraries.Add(itinerary);
                }
                else
                {
                    log.Warn("Received itinerary that did not satisfy the route specification");
                }
            }

            return itineraries;
        }

        private Itinerary ToItinerary(TransitPath transitPath)
        {
            List<Leg> legs = new List<Leg>(transitPath.TransitEdges.Count);
            foreach (TransitEdge edge in transitPath.TransitEdges)
            {
                legs.Add(ToLeg(edge));
            }
            return new Itinerary(legs);
        }

        private Leg ToLeg(TransitEdge edge)
        {
            return new Leg(
                voyageRepository.Find(new VoyageNumber(edge.VoyageNumber)),
                locationRepository.Find(new UnLocode(edge.FromUnLocode)),
                locationRepository.Find(new UnLocode(edge.ToUnLocode)),
                edge.FromDate, edge.ToDate);
        }
    }
}