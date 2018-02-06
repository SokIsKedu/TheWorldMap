using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using AutoMapper;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public void UpdateUser(WorldUser pim)
        {

            var usr = _context.Users.First(u => u.Id == pim.Id);
            if (usr != null)
            {
                //TODO fix
#pragma warning disable CS0618 // Type or member is obsolete
                Mapper.CreateMap(typeof(WorldUser), typeof(WorldUser));
#pragma warning restore CS0618 // Type or member is obsolete
                Mapper.Map(pim, usr);
                _context.SaveChanges();
            }
        }

        public void AddStop(string tripName, Stop newStop, string username)
        {
            var trip = GetTripByName(tripName, username);
            if (trip != null)
            {
                trip.Stops.Add(newStop);   // Saves only the foreign key. wtf...??
                _context.Stops.Add(newStop);  // Saves the actual object.
            }
        }

        public void RemoveStop(Stop newStop, string tripName, string username)
        {
            var trip = GetTripByName(tripName, username);
            if (trip != null)
            {
                foreach (Stop stop in trip.Stops.ToList())
                {
                    if (stop.Latitude == newStop.Latitude
                        && stop.Longitude == newStop.Longitude
                        && stop.Order == newStop.Order)
                    {
                        trip.Stops.Remove(stop);
                        _context.Stops.Remove(stop);
                    }
                }
            }
        }


        public void CalculateTrip(string tripName, string username)
        {
            var trip = GetTripByName(tripName, username);
            if (trip.Stops.Count > 1)
            {
                var R = 6371; // Radius of the earth in km
                var totalKM = 0.0;
                var prevStop = trip.Stops.First();
                var stopList = trip.Stops.Cast<Stop>().ToList();
                for (int i = 1; i < stopList.Count; i++)
                {
                    double dLat = deg2rad(stopList.ElementAt(i).Latitude - prevStop.Latitude);  // deg2rad below
                    double dLon = deg2rad(stopList.ElementAt(i).Longitude - prevStop.Longitude);


                    Console.WriteLine(dLat + "   " + dLon);
                    Console.WriteLine("======================================================");
                    var a =
                    Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(deg2rad(prevStop.Latitude)) * Math.Cos(stopList.ElementAt(i).Latitude) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                    Console.WriteLine(a);
                    Console.WriteLine("======================================================");
                    var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    Console.WriteLine(c + "  " + Math.Round(c, 15));
                    Console.WriteLine("======================================================");
                    var d = Math.Round(R * c, 15);
                    Console.WriteLine(d);
                    Console.WriteLine("======================================================");
                    totalKM += d;
                    Console.WriteLine(Math.Round(totalKM, 15));
                    Console.WriteLine("======================================================");
                    prevStop = stopList.ElementAt(i);
                }
                trip.TotalKM = totalKM;
            } else
            {
                trip.TotalKM = 0;
            }
        }


        public double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }


        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public void RemoveTrip(string name, string username)
        {
            Trip tr = GetTripByName(name, username);

            if (tr != null)
            {
                foreach (Stop stop in tr.Stops.ToList())
                {
                    tr.Stops.Remove(stop);
                    _context.Stops.Remove(stop);
                }
                _context.Trips.Remove(tr);
            }

        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting all Trips from the database");
            return _context.Trips.ToList();
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            return _context.Trips.Include(t => t.Stops).OrderBy(t => t.Name).ToList();
        }

        public Trip GetTripByName(string tripName, string username)
        {
            return _context.Trips.Include(t => t.Stops).
                Where(t => t.Name == tripName && t.UserName == username).FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        public IEnumerable<Trip> GetUserTripsWithStops(string name)
        {
            try {
                return _context.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .Where(t => t.UserName == name)
                    .ToList();
            } catch (Exception ex) {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }

        public IEnumerable<WorldUser> GetUsers()
        {
            try {
                return _context.Users.OrderBy(x => x.UserName)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get users from database", ex);
                return null;
            }
        }
        
        public void RemoveUser(string userName)
        {
            var user = _context.Users.Where(x => x.UserName == userName).First();
            if (user != null)
            {
                Console.WriteLine(user.UserName);
                _context.Remove(user);
                Console.WriteLine(user);
            }
        }

        public WorldUser GetUser(string userName)
        {
            var user = _context.Users.Where(x => x.UserName == userName).First();
            
            return user;

        }
    }
}
