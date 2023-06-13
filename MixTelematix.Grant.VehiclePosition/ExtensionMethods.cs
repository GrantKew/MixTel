using GeoCoordinatePortable;
using MixTelematix.Grant.Assesment.VehiclePosition.Models;
using System.Diagnostics;

namespace MixTelematix.Grant.Assesment.VehiclePosition
{
    internal static class ExtensionMethods
    {
        internal static void OffsetPlaceHolder(this ref int value, int offSetCount = 4)
        {
            value += offSetCount;
        }

        internal static void StopSopwatchAndLogTime(this Stopwatch stopwatch, string message)
        {
            stopwatch.Stop();
            Console.WriteLine($"{message}. Time elapsed: {stopwatch.ElapsedMilliseconds} milliseconds");
        }

        internal static void LogCompletedVehicleData(this NearestVehicle nearestVehicle)
        {
            Console.WriteLine($"Requested Coordinated: {nearestVehicle.RequestedCoordinate}");
            Console.WriteLine($"Found Coordinate:  {new GeoCoordinate(nearestVehicle.Vehicle.Latitude, nearestVehicle.Vehicle.Longitude)}");
            Console.WriteLine($"Vechicle Registration {nearestVehicle.Vehicle.VehicleRegistration}");
            Console.WriteLine(Environment.NewLine);
        }
    }
}
