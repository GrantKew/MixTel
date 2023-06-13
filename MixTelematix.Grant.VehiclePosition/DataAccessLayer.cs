using MixTelematix.Grant.Assesment.VehiclePosition.Models;
using System.Text;

namespace MixTelematix.Grant.Assesment.VehiclePosition
{
    internal class DataAccessLayer
    {
        private readonly string filePath;

        public DataAccessLayer(string filePath)
        {
            this.filePath = filePath;
        }
        internal List<VehicleData> ReadDataFile()
        {
            var data = File.ReadAllBytes(filePath);
            var vehiclePositionList = new List<VehicleData>();
            var placeHolder = 0;
            while (placeHolder < data.Length)
            {
                vehiclePositionList.Add(getVehicleData(data, ref placeHolder));
            }
            return vehiclePositionList;
        }
        private VehicleData getVehicleData(byte[] bytes, ref int placeHolder)
        {
            var vehicleData = new VehicleData
            {
                Id = BitConverter.ToInt32(bytes, placeHolder)
            };

            placeHolder.OffsetPlaceHolder();
            var stringBuilder = new StringBuilder();
            while (bytes[placeHolder] != (byte)0)
            {
                stringBuilder.Append((char)bytes[placeHolder]);
                placeHolder.OffsetPlaceHolder(1);
            }
            vehicleData.VehicleRegistration = stringBuilder.ToString();
            placeHolder.OffsetPlaceHolder(1);

            vehicleData.Latitude = BitConverter.ToSingle(bytes, placeHolder);
            placeHolder.OffsetPlaceHolder();

            vehicleData.Longitude = BitConverter.ToSingle(bytes, placeHolder);
            placeHolder.OffsetPlaceHolder();

            vehicleData.TimeSinceEpoch = new DateTime(1970, 1, 1).AddSeconds(BitConverter.ToUInt64(bytes, placeHolder));
            placeHolder.OffsetPlaceHolder(8);

            return vehicleData;
        }
    }
}
