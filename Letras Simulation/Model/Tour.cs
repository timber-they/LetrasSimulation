using System.Collections.Generic;
using System.Linq;


namespace Letras_Simulation.Model
{
    public class Tour
    {
        public Tour (List <Trackpoint> trackpoints) => Trackpoints = trackpoints;

        public Tour () => Trackpoints = new List <Trackpoint> ();

        public void Add (Trackpoint trackpoint) => Trackpoints.Add (trackpoint);

        public List <Trackpoint> Trackpoints { get; }

        /// <inheritdoc />
        public override string ToString () =>
            string.Join ("\n", Trackpoints.Select (trackpoint => trackpoint.ToString ()));

        public double GetSlope (int trackpointIndex)
        {
            if (trackpointIndex >= Trackpoints.Count - 1)
                return 0;

            return Trackpoints [trackpointIndex].GetSlope (Trackpoints [trackpointIndex + 1]);
        }

        public double GetClimb (int trackpointIndex)
        {
            if (trackpointIndex >= Trackpoints.Count - 1)
                return 0;

            return Trackpoints [trackpointIndex].GetClimb (Trackpoints [trackpointIndex + 1]);
        }

        public double GetDistance (int trackpointIndex)
        {
            if (trackpointIndex >= Trackpoints.Count - 1)
                return 0;

            return Trackpoints [trackpointIndex].GetDistance (Trackpoints [trackpointIndex + 1]);
        }
    }
}