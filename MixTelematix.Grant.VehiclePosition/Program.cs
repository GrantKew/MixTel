using GeoCoordinatePortable;
using MixTelematix.Grant.Assesment.VehiclePosition;
using MixTelematix.Grant.Assesment.VehiclePosition.Models;
using System.Diagnostics;

Console.WriteLine("Please enter a file path for your .dat file");
var filePath = Console.ReadLine();
while (!File.Exists(filePath))
{
    Console.WriteLine($@"Bruh, please... ""Ctrl"" + ""C"" and then ""Ctrl"" + ""V"". {filePath} could not be found");
    filePath = Console.ReadLine();
}

var dataAccess = new DataAccessLayer(filePath);
var stopwatch = Stopwatch.StartNew();
var vehicleDataList = dataAccess.ReadDataFile();
stopwatch.StopSopwatchAndLogTime("Completed reading the data file");

var coordinates = new List<GeoCoordinate> {
                new GeoCoordinate(34.544909,-102.100843),
                new GeoCoordinate(32.345544,-99.123124),
                new GeoCoordinate(33.234235,-100.214124),
                new GeoCoordinate(35.195739,-95.348899),
                new GeoCoordinate(31.895839, -97.789573),
                new GeoCoordinate(32.895839,-101.789573),
                new GeoCoordinate(34.115839,-100.225732),
                new GeoCoordinate(32.335839,-99.992232),
                new GeoCoordinate(33.535339,-94.792232),
                new GeoCoordinate(32.234235,-100.222222)
            };

var nearestVehicles = new List<NearestVehicle>();

stopwatch.Restart();
foreach (var coordinate in coordinates)
{
    var nearestVehicle = vehicleDataList
        .OrderBy(x => ((coordinate.Latitude - x.Latitude) * (coordinate.Latitude - x.Latitude)) +
                          ((coordinate.Longitude - x.Longitude) * (coordinate.Longitude - x.Longitude)))
        .First();

    nearestVehicles.Add(new NearestVehicle
    {
        RequestedCoordinate = coordinate,
        Vehicle = nearestVehicle
    });


}
stopwatch.StopSopwatchAndLogTime("Completed vehicle location processing");
nearestVehicles.ForEach(vehicle => vehicle.LogCompletedVehicleData());
Console.ReadLine();