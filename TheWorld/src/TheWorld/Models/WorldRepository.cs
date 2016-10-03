using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace TheWorld.Models
{
    public class WorldRepository: IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
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
                    if (stop.Order == newStop.Order)
                    {
                        trip.Stops.Remove(stop);                       
                        _context.Stops.Remove(stop); 
                    }
                } 
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public void RemoveTrip(string name,string username)
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
            } catch (Exception ex){
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }
    }
}
