using System;
using System.Diagnostics;


namespace Letras_Simulation.Model
{
    public class State
    {
        public Tour Tour            { get; }
        public int  TrackpointIndex { get; private set; }

        /**
         * Note that speed is not a vector - we only consider the norm of the speed here.
         */
        public double Speed { get; private set; }

        public double ElapsedTime { get; private set; }
        public double Mass        { get; }

        private const double MIN_SPEED = 0.2;

        public State (Tour tour, double mass)
        {
            Tour            = tour;
            TrackpointIndex = 0;
            // Speeds of 0 can cause problems
            Speed       = MIN_SPEED;
            ElapsedTime = 0;
            Mass        = mass;
        }

        /**
         * Returns true if the final trackpoint is reached
         */
        public bool Step (double power)
        {
            var distance = Tour.GetDistance (TrackpointIndex);

            var (time, speed) = Numerical (power, distance);

            Speed = speed;
            TrackpointIndex++;
            ElapsedTime += time;

            Debug.WriteLine ($"Riding at {Speed} m/s, after {ElapsedTime} s, at trackpoint {TrackpointIndex}");

            return TrackpointIndex >= Tour.Trackpoints.Count - 1;
        }

        private (double Duration, double Speed) Numerical (double power, double distance)
        {
            var speed    = Speed;
            var duration = 0.0;

            // Using a step length of 1 m
            var s = 0.1;
            for (var i = 0.0; i < distance; i += s)
            {
                var accelerationPower = GetAccelerationPower (power, speed);
                var time              = s / speed;
                var acceleration      = accelerationPower / speed / Mass;
                speed    =  Math.Max (MIN_SPEED, speed + time * acceleration);
                duration += time;
            }

            return (duration, speed);
        }

        private double GetAccelerationPower (double power, double speed)
        {
            var slope = Tour.GetSlope (TrackpointIndex);

            // Reduction by 1% and additional 1-7% due to gearing
            var bikeReduced = power * 0.95;
            // Assuming a CDA of 0.35
            var airDragReduced = bikeReduced - 1.225 * speed * speed * speed * 0.35 / 2;
            // Assuming a g of 9.77653 (usual in Bogota, Columbia) and a Crr of 0.004
            var rollingResistanceReduced = airDragReduced - speed * Mass * 9.77653 * Math.Cos (Math.Atan (slope)) * 0.004;
            // Assuming a g of 9.77653
            var climbingReduced = rollingResistanceReduced - speed * Mass * 9.77653 * slope;

            return climbingReduced;
        }
    }
}