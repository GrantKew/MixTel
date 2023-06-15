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

double farthestDistance = 0;

/*
Attempt 1.2
loop through coordinates and find farthest distance between coordinates
filter data to only include data for vehicles within the range
find closest vehicle.

filter however only limits data to 1.89m rows, so I'm going to take swing in the dark and say this isn't what you wanted from me.
I thought about looping through the table and collecting distances from there but the net result would still be 20m queries.
My best guess is that it would have something to do with a closeness threshold or grouping the cars in some way. 
Properly stumped...
 */

//get maximum distance between requested coordinates to filter list

foreach (var primaryCoordinate in coordinates)
{
    foreach (var compareCoordinate in coordinates)
    {
        if (primaryCoordinate == compareCoordinate)
        {
            //don't compare the coordinate to its self
            continue;
        }

        var distance = primaryCoordinate.GetDistanceTo(compareCoordinate);
        farthestDistance = distance > farthestDistance ? distance : farthestDistance;
    }
}

//find car closest to first point
var primaryVehicle = vehicleDataList
        .OrderBy(x => ((coordinates[0].Latitude - x.Latitude) * (coordinates[0].Latitude - x.Latitude)) +
                          ((coordinates[0].Longitude - x.Longitude) * (coordinates[0].Longitude - x.Longitude)))
        .First();

//list cars within farthest distance
var filteredList = new List<VehicleData>();

while (!filteredList.Any())
{
    filteredList = vehicleDataList
                    .Where(x => new GeoCoordinate(x.Latitude, x.Longitude)
                    .GetDistanceTo(new GeoCoordinate(primaryVehicle.Latitude, primaryVehicle.Longitude)) <= farthestDistance)
                    .ToList();
    //increase the distance by 1% each iteration
    farthestDistance = farthestDistance * 1.01;
}

var nearestVehicles = new List<NearestVehicle> {
    new NearestVehicle {
        Vehicle = primaryVehicle,
        RequestedCoordinate = coordinates[0]
    }
};

coordinates.Remove(coordinates[0]);

stopwatch.Restart();
foreach (var coordinate in coordinates)
{
    var nearestVehicle = filteredList
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

/*
 I'm fairly certain that this is not the solution that you are looking for and I realize that does not bode well for my prospects with you, 
 however you have genuinely intrigued me with this and I would love to know what the desired outcome is. Should this be the last you hear from me, 
 I would like you to know that this is one of few positions that I would be genuinely disappointed in not getting, purely for the growth potential that I can see at MixTel.
 I had a look at the rest of the GitHub repo and I like the code that I saw. 

 Something I found whilst looking for a solution to this magnificent problem that I thought might be of interest to you.
    https://www.codeproject.com/Questions/5340076/Find-the-nearest-vehicle-position-in-Csharp-NET-co.
 */