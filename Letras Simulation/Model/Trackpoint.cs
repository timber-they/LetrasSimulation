using System;
using System.Device.Location;


namespace Letras_Simulation.Model
{
    public class Trackpoint
    {
        public Trackpoint (double latitude, double longitude, double altitude) => Coordinate = new GeoCoordinate (latitude, longitude, altitude);

        public Trackpoint () => Coordinate = new GeoCoordinate ();

        public GeoCoordinate Coordinate { get; set; }

        public double GetDistance (Trackpoint b) => Coordinate.GetDistanceTo (b.Coordinate);

        public double GetClimb (Trackpoint b) => b.Coordinate.Altitude - Coordinate.Altitude;

        /**
         * Note that this is a rough estimate for high slopes
         */
        public double GetSlope (Trackpoint b)
            => GetClimb (b) / GetDistance (b);

        public void SetLatitude (double  latitude)  => Coordinate.Latitude = latitude;
        public void SetAltitude (double  altitude)  => Coordinate.Altitude = altitude;
        public void SetLongitude (double longitude) => Coordinate.Longitude = longitude;

        /// <inheritdoc />
        public override string ToString () => $"Lat: {Coordinate.Latitude}; Lon: {Coordinate.Longitude}; Alt: {Coordinate.Altitude}";
    }
}