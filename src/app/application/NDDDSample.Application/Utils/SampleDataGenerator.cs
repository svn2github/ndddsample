namespace NDDDSample.Application.Utils
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Domain.Model.Cargos;
    using Domain.Model.Handlings;
    using Domain.Model.Locations;
    using Domain.Model.Voyages;
    using NHibernate;
    using Rhino.Commons;

    #endregion

    /// <summary>
    ///  Provides sample data.
    /// </summary>
    public static class SampleDataGenerator
    {
        private static DateTime baseTime;

        static SampleDataGenerator()
        {            
            baseTime = new DateTime(2008, 01, 01).AddDays(-100);
        }

        private static void LoadHandlingEventData(ISession session)
        {
            const string handlingEventSql =
                "insert into HandlingEvent (completionTime, registrationTime, type, location_id, voyage_id, cargo_id) " +
                "values (?, ?, ?, ?, ?, ?)";

            var handlingEventArgs = new[]
                                        {
                                            //XYZ (SESTO-FIHEL-DEHAM-CNHKG-JPTOK-AUMEL)
                                            new object[] {ts(0), ts((0)), "RECEIVE", 1, null, 1},
                                            new object[] {ts((4)), ts((5)), "LOAD", 1, 1, 1},
                                            new object[] {ts((14)), ts((14)), "UNLOAD", 5, 1, 1},
                                            new object[] {ts((15)), ts((15)), "LOAD", 5, 1, 1},
                                            new object[] {ts((30)), ts((30)), "UNLOAD", 6, 1, 1},
                                            new object[] {ts((33)), ts((33)), "LOAD", 6, 1, 1},
                                            new object[] {ts((34)), ts((34)), "UNLOAD", 3, 1, 1},
                                            new object[] {ts((60)), ts((60)), "LOAD", 3, 1, 1},
                                            new object[] {ts((70)), ts((71)), "UNLOAD", 4, 1, 1},
                                            new object[] {ts((75)), ts((75)), "LOAD", 4, 1, 1},
                                            new object[] {ts((88)), ts((88)), "UNLOAD", 2, 1, 1},
                                            new object[] {ts((100)), ts((102)), "CLAIM", 2, null, 1},
                                            //ZYX (AUMEL - USCHI - DEHAM -)
                                            new object[] {ts((200)), ts((201)), "RECEIVE", 2, null, 3},
                                            new object[] {ts((202)), ts((202)), "LOAD", 2, 2, 3},
                                            new object[] {ts((208)), ts((208)), "UNLOAD", 7, 2, 3},
                                            new object[] {ts((212)), ts((212)), "LOAD", 7, 2, 3},
                                            new object[] {ts((230)), ts((230)), "UNLOAD", 6, 2, 3},
                                            new object[] {ts((235)), ts((235)), "LOAD", 6, 2, 3},
                                            //ABC
                                            new object[] {ts((20)), ts((21)), "CLAIM", 2, null, 2},
                                            //CBA
                                            new object[] {ts((0)), ts((1)), "RECEIVE", 2, null, 4},
                                            new object[] {ts((10)), ts((11)), "LOAD", 2, 2, 4},
                                            new object[] {ts((20)), ts((21)), "UNLOAD", 7, 2, 4},
                                            //FGH
                                            new object[] {ts(100), ts(160), "RECEIVE", 3, null, 5},
                                            new object[] {ts(150), ts(110), "LOAD", 3, 3, 5},
                                            //// JKL
                                            new object[] {ts(200), ts(220), "RECEIVE", 6, null, 6},
                                            new object[] {ts(300), ts(330), "LOAD", 6, 3, 6},
                                            new object[] {ts(400), ts(440), "UNLOAD", 5, 3, 6} // Unexpected event
                                        };

            ExecuteUpdate(session, handlingEventSql, handlingEventArgs);
        }

        private static void LoadCarrierMovementData(ISession session)
        {
            const string voyageSql = "insert into Voyage (id, voyage_number) values (?, ?)";
            var voyageArgs = new[]
                                 {
                                     new object[] {1, "0101"},
                                     new object[] {2, "0202"},
                                     new object[] {3, "0303"}
                                 };
            ExecuteUpdate(session, voyageSql, voyageArgs);

            const string carrierMovementSql =
                "insert into CarrierMovement (id, voyage_id, departure_location_id, arrival_location_id, departure_time, arrival_time, cm_index) " +
                "values (?,?,?,?,?,?,?)";

            var carrierMovementArgs = new[]
                                          {
                                              // SESTO - FIHEL - DEHAM - CNHKG - JPTOK - AUMEL (voyage 0101)
                                              new object[] {1, 1, 1, 5, ts(1), ts(2), 0},
                                              new object[] {2, 1, 5, 6, ts(1), ts(2), 1},
                                              new object[] {3, 1, 6, 3, ts(1), ts(2), 2},
                                              new object[] {4, 1, 3, 4, ts(1), ts(2), 3},
                                              new object[] {5, 1, 4, 2, ts(1), ts(2), 4},
                                              // AUMEL - USCHI - DEHAM - SESTO - FIHEL (voyage 0202)
                                              new object[] {7, 2, 2, 7, ts(1), ts(2), 0},
                                              new object[] {8, 2, 7, 6, ts(1), ts(2), 1},
                                              new object[] {9, 2, 6, 1, ts(1), ts(2), 2},
                                              new object[] {6, 2, 1, 5, ts(1), ts(2), 3},
                                              // CNHKG - AUMEL - FIHEL - DEHAM - SESTO - USCHI - JPTKO (voyage 0303)
                                              new object[] {10, 3, 3, 2, ts(1), ts(2), 0},
                                              new object[] {11, 3, 2, 5, ts(1), ts(2), 1},
                                              new object[] {12, 3, 6, 1, ts(1), ts(2), 2},
                                              new object[] {13, 3, 1, 7, ts(1), ts(2), 3},
                                              new object[] {14, 3, 7, 4, ts(1), ts(2), 4}
                                          };
            ExecuteUpdate(session, carrierMovementSql, carrierMovementArgs);
        }

        private static void LoadCargoData(ISession session)
        {
            const string cargoSql =
                "insert into Cargo (id, tracking_id, origin_id, spec_origin_id, spec_destination_id, spec_arrival_deadline, transport_status, current_voyage_id, last_known_location_id, is_misdirected, routing_status, calculated_at, unloaded_at_dest) " +
                "values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

            var cargoArgs = new[]
                                {
                                    new object[]
                                        {
                                            1, "XYZ", 1, 1, 2, ts(10), "IN_PORT", null, 1, false, "ROUTED", ts(100),
                                            false
                                        },
                                    new object[]
                                        {
                                            2, "ABC", 1, 1, 5, ts(20), "IN_PORT", null, 1, false, "ROUTED", ts(100),
                                            false
                                        },
                                    new object[]
                                        {
                                            3, "ZYX", 2, 2, 1, ts(30), "IN_PORT", null, 1, false, "NOT_ROUTED", ts(100),
                                            false
                                        },
                                    new object[]
                                        {
                                            4, "CBA", 5, 5, 1, ts(40), "IN_PORT", null, 1, false, "MISROUTED", ts(100),
                                            false
                                        },
                                    new object[]
                                        {
                                            5, "FGH", 1, 3, 5, ts(50), "IN_PORT", null, 1, false, "ROUTED", ts(100),
                                            false
                                        }, // Cargo origin differs from spec origin
                                    new object[]
                                        {
                                            6, "JKL", 6, 6, 4, ts(60), "IN_PORT", null, 1, true, "ROUTED", ts(100),
                                            false
                                        }
                                };
            ExecuteUpdate(session, cargoSql, cargoArgs);
        }

        private static void LoadLocationData(ISession session)
        {
            const string locationSql = "insert into Location (id, unlocode, name) " +
                                       "values (?, ?, ?)";

            var locationArgs = new[]
                                   {
                                       new object[] {1, "SESTO", "Stockholm"},
                                       new object[] {2, "AUMEL", "Melbourne"},
                                       new object[] {3, "CNHKG", "Hongkong"},
                                       new object[] {4, "JPTOK", "Tokyo"},
                                       new object[] {5, "FIHEL", "Helsinki"},
                                       new object[] {6, "DEHAM", "Hamburg"},
                                       new object[] {7, "USCHI", "Chicago"}
                                   };
            ExecuteUpdate(session, locationSql, locationArgs);
        }

        private static void LoadItineraryData(ISession session)
        {
            const string legSql =
                "insert into Leg (id, cargo_id, voyage_id, load_location_id, unload_location_id, load_time, unload_time, leg_index) " +
                "values (?,?,?,?,?,?,?,?)";

            var legArgs = new[]
                              {
                                  // Cargo 5: Hongkong - Melbourne - Stockholm - Helsinki
                                  new object[] {1, 5, 1, 3, 2, ts(1), ts(2), 0},
                                  new object[] {2, 5, 1, 2, 1, ts(3), ts(4), 1},
                                  new object[] {3, 5, 1, 1, 5, ts(4), ts(5), 2},
                                  // Cargo 6: Hamburg - Stockholm - Chicago - Tokyo
                                  new object[] {4, 6, 2, 6, 1, ts(1), ts(2), 0},
                                  new object[] {5, 6, 2, 1, 7, ts(3), ts(4), 1},
                                  new object[] {6, 6, 2, 7, 4, ts(5), ts(6), 2}
                              };
            ExecuteUpdate(session, legSql, legArgs);
        }

        //TODO:atrosin Revise where and how is used the method
        public static void LoadHibernateData(ISessionFactory sf, HandlingEventFactory handlingEventFactory,
                                             IHandlingEventRepository handlingEventRepository)
        {
            Console.WriteLine("*** Loading Hibernate data ***");


            ISession session = sf.GetCurrentSession();

            foreach (Location location  in SampleLocations.GetAll())
            {
                session.Save(location);
            }

            session.Save(SampleVoyages.HONGKONG_TO_NEW_YORK);
            session.Save(SampleVoyages.NEW_YORK_TO_DALLAS);
            session.Save(SampleVoyages.DALLAS_TO_HELSINKI);
            session.Save(SampleVoyages.HELSINKI_TO_HONGKONG);
            session.Save(SampleVoyages.DALLAS_TO_HELSINKI_ALT);

            var routeSpecification = new RouteSpecification(SampleLocations.HONGKONG,
                                                                           SampleLocations.HELSINKI,
                                                                           DateTestUtil.toDate("2009-03-15"));
            var trackingId = new TrackingId("ABC123");
            var abc123 = new Cargo(trackingId, routeSpecification);

            var itinerary = new Itinerary(
                new List<Leg>
                    {
                        new Leg(SampleVoyages.HONGKONG_TO_NEW_YORK, SampleLocations.HONGKONG, SampleLocations.NEWYORK,
                                DateTestUtil.toDate("2009-03-02"), DateTestUtil.toDate("2009-03-05")),
                        new Leg(SampleVoyages.NEW_YORK_TO_DALLAS, SampleLocations.NEWYORK, SampleLocations.DALLAS,
                                DateTestUtil.toDate("2009-03-06"), DateTestUtil.toDate("2009-03-08")),
                        new Leg(SampleVoyages.DALLAS_TO_HELSINKI, SampleLocations.DALLAS, SampleLocations.HELSINKI,
                                DateTestUtil.toDate("2009-03-09"), DateTestUtil.toDate("2009-03-12"))
                    });
            abc123.AssignToRoute(itinerary);

            session.Save(abc123);


            HandlingEvent event1 = handlingEventFactory.CreateHandlingEvent(
                new DateTime(), DateTestUtil.toDate("2009-03-01"), trackingId, null, SampleLocations.HONGKONG.UnLocode,
                HandlingType.RECEIVE
                );
            session.Save(event1);

            HandlingEvent event2 = handlingEventFactory.CreateHandlingEvent(
                new DateTime(), DateTestUtil.toDate("2009-03-02"), trackingId,
                SampleVoyages.HONGKONG_TO_NEW_YORK.VoyageNumber, SampleLocations.HONGKONG.UnLocode,
                HandlingType.LOAD
                );
            session.Save(event2);

            HandlingEvent event3 = handlingEventFactory.CreateHandlingEvent(
                new DateTime(), DateTestUtil.toDate("2009-03-05"), trackingId,
                SampleVoyages.HONGKONG_TO_NEW_YORK.VoyageNumber, SampleLocations.NEWYORK.UnLocode,
                HandlingType.UNLOAD
                );
            session.Save(event3);


            HandlingHistory handlingHistory = handlingEventRepository.LookupHandlingHistoryOfCargo(trackingId);
            abc123.DeriveDeliveryProgress(handlingHistory);

            session.Update(abc123);

            // Cargo JKL567

            var routeSpecification1 = new RouteSpecification(SampleLocations.HANGZOU,
                                                             SampleLocations.STOCKHOLM,
                                                             DateTestUtil.toDate("2009-03-18"));
            var trackingId1 = new TrackingId("JKL567");
            var jkl567 = new Cargo(trackingId1, routeSpecification1);

            var itinerary1 = new Itinerary(new List<Leg>
                                               {
                                                   new Leg(SampleVoyages.HONGKONG_TO_NEW_YORK,
                                                           SampleLocations.HANGZOU, SampleLocations.NEWYORK,
                                                           DateTestUtil.toDate("2009-03-03"),
                                                           DateTestUtil.toDate("2009-03-05")),
                                                   new Leg(SampleVoyages.NEW_YORK_TO_DALLAS,
                                                           SampleLocations.NEWYORK, SampleLocations.DALLAS,
                                                           DateTestUtil.toDate("2009-03-06"),
                                                           DateTestUtil.toDate("2009-03-08")),
                                                   new Leg(SampleVoyages.DALLAS_TO_HELSINKI,
                                                           SampleLocations.DALLAS, SampleLocations.STOCKHOLM,
                                                           DateTestUtil.toDate("2009-03-09"),
                                                           DateTestUtil.toDate("2009-03-11"))
                                               });
            jkl567.AssignToRoute(itinerary1);

            session.Save(jkl567);


            HandlingEvent event21 = handlingEventFactory.CreateHandlingEvent(
                new DateTime(), DateTestUtil.toDate("2009-03-01"), trackingId1, null, SampleLocations.HANGZOU.UnLocode,
                HandlingType.RECEIVE);

            session.Save(event21);

            HandlingEvent event22 = handlingEventFactory.CreateHandlingEvent(
                new DateTime(), DateTestUtil.toDate("2009-03-03"), trackingId1,
                SampleVoyages.HONGKONG_TO_NEW_YORK.VoyageNumber, SampleLocations.HANGZOU.UnLocode,
                HandlingType.LOAD
                );
            session.Save(event22);

            HandlingEvent event23 = handlingEventFactory.CreateHandlingEvent(
                new DateTime(), DateTestUtil.toDate("2009-03-05"), trackingId1,
                SampleVoyages.HONGKONG_TO_NEW_YORK.VoyageNumber, SampleLocations.NEWYORK.UnLocode,
                HandlingType.UNLOAD
                );
            session.Save(event23);

            HandlingEvent event24 = handlingEventFactory.CreateHandlingEvent(
                new DateTime(), DateTestUtil.toDate("2009-03-06"), trackingId1,
                SampleVoyages.HONGKONG_TO_NEW_YORK.VoyageNumber, SampleLocations.NEWYORK.UnLocode,
                HandlingType.LOAD
                );
            session.Save(event24);


            HandlingHistory handlingHistory1 = handlingEventRepository.LookupHandlingHistoryOfCargo(trackingId1);
            jkl567.DeriveDeliveryProgress(handlingHistory1);

            session.Update(jkl567);
        }


        private static DateTime ts(int hours)
        {
            return baseTime.AddHours(hours);                
        }

        public static void LoadSampleData()
        {
            using (ITransaction transaction = UnitOfWork.CurrentSession.BeginTransaction())
            {
                ISession session = UnitOfWork.CurrentSession;
                LoadLocationData(session);
                LoadCarrierMovementData(session);
                LoadCargoData(session);
                LoadItineraryData(session);
                LoadHandlingEventData(session);

                transaction.Commit();
            }
        }

        private static void ExecuteUpdate(ISession session, string sql, object[][] dataTable)
        {
            for (int i = 0; i < dataTable.GetLength(0); i++)
            {
                IQuery query = session.CreateSQLQuery(sql);

                for (int j = 0; j < dataTable[i].GetLength(0); j++)
                {
                    object objValue = dataTable[i][j];

                    if (objValue == null)
                    {
                        //Workaround: cant set NULL value for int type 
                        query.SetBinary(j, null);
                    }
                    else
                    {
                        query.SetParameter(j, objValue);
                    }
                }

                query.ExecuteUpdate();
            }
        }

        public static DateTime Offset(int hours)
        {
            return ts(hours);
        }
    }
}