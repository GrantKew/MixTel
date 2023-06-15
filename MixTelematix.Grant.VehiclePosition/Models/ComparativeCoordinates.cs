using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixTelematix.Grant.Assesment.VehiclePosition.Models
{
    internal class ComparativeCoordinates
    {
        public GeoCoordinate CoordinateA { get; set; }
        public GeoCoordinate CoordinateB { get; set; }
        public double DistanceBetweenPoints { get; set; } = 0;
    }
}
