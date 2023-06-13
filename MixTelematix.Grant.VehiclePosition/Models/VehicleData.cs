namespace MixTelematix.Grant.Assesment.VehiclePosition.Models
{
    internal class VehicleData
    {
        public int Id { get; set; }
        public string VehicleRegistration { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime TimeSinceEpoch { get; set; }
    }
}
