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

        public double GetSlope (Trackpoint b) => GetClimb (b) / GetHorizontalDistance (b);

        public double GetHorizontalDistance (Trackpoint b)
        {
            var climb = GetClimb (b);
            var distance = GetDistance (b);

            if (Math.Abs (climb) < 0.0001)
                return distance;

            return climb / Math.Tan (Math.Asin (climb / distance));
        }

        public void SetLatitude (double  latitude)  => Coordinate.Latitude = latitude;
        public void SetAltitude (double  altitude)  => Coordinate.Altitude = altitude;
        public void SetLongitude (double longitude) => Coordinate.Longitude = longitude;

        /// <inheritdoc />
        public override string ToString () => $"Lat: {Coordinate.Latitude}; Lon: {Coordinate.Longitude}; Alt: {Coordinate.Altitude}";
    }
}