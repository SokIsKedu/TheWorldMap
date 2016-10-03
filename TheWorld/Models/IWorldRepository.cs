using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        Trip GetTripByName(string tripName, string username);
        void AddTrip(Trip trip);
        void RemoveTrip(string name, string username);
        void AddStop(string tripName, Stop newStop, string username);
        void RemoveStop(Stop newStop, string tripName, string username);

        Task<bool> SaveChangesAsync();
        IEnumerable<Trip> GetAllTripsWithStops();
        IEnumerable<Trip> GetUserTripsWithStops(string name);
    }
}