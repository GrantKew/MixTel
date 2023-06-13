using GeoCoordinatePortable;

namespace MixTelematix.Grant.Assesment.VehiclePosition.Models
{
    internal class NearestVehicle
    {
        public VehicleData Vehicle { get; set; }
        public GeoCoordinate RequestedCoordinate { get; set; }
    }
}
