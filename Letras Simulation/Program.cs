using System;
using System.Globalization;
using System.IO;

using Letras_Simulation.Model;
using Letras_Simulation.Util;


namespace Letras_Simulation
{
    internal class Program
    {
        private static string _data;
        private const  string RESOURCES = "Resources/";
        private const  string ENDING    = ".gpx";

        public static void Main (string [] args)
        {
            start:

            Console.WriteLine ("|\n| Which tour do you want to analyze?\n|");
            var input = Console.ReadLine ();
            if (input == null)
                goto start;
            _data = RESOURCES + input + ENDING;
            if (!File.Exists (_data))
                goto start;

            Console.WriteLine ("|\n| Do you want continuous power or time calculation?\n|");
            input = Console.ReadLine ();
            if (input == null)
                goto start;
            if (input.ToLower ().StartsWith ("p"))
                ContinuousPowerCalculation ();
            if (input.ToLower ().StartsWith ("t"))
                ContinuousTimeCalculation ();

            goto start;
        }


        #region time

        private static void ContinuousTimeCalculation ()
        {
            start:
            double power;
            do
                Console.WriteLine ("|\n| Please enter your power\n|");
            while (!double.TryParse (Console.ReadLine (), NumberStyles.Any, CultureInfo.InvariantCulture, out power));

            TimeCalculation (power);
            goto start;
        }

        private static int TimeCalculation (double power)
        {
            Console.WriteLine ("|\n| Calculating time...\n|");

            var elapsedTime = CalculateTime (power);

            var hours   = (int) (elapsedTime / 3600.0);
            var minutes = (int) (elapsedTime / 60.0 - hours * 60.0);
            var seconds = (int) (elapsedTime - hours * 3600.0 - minutes * 60.0);
            Console.WriteLine ($"|\n| This will take {elapsedTime} s, meaning about {hours} h, {minutes} m, {seconds} s\n|");

            return (int) elapsedTime;
        }

        private static double CalculateTime (double power, double precision = 0.01, Tour tour = null)
        {
            tour = tour ?? XmlUtil.Parse (File.ReadAllText (_data));
            // Me, my bike, my bottles and other stuff
            var state = new State (tour, 66.2 + 7.9 + 1.5 + 2.0);

            bool res;
            do
                res = state.Step (power, precision);
            while (!res);

            return state.ElapsedTime;
        }

        #endregion


        #region power

        private static void ContinuousPowerCalculation ()
        {
            start:
            int hours;
            do
                Console.WriteLine ("|\n| Please enter the hours\n|");
            while (!int.TryParse (Console.ReadLine (), NumberStyles.Any, CultureInfo.InvariantCulture, out hours));
            int minutes;
            do
                Console.WriteLine ("|\n| Please enter the minutes\n|");
            while (!int.TryParse (Console.ReadLine (), NumberStyles.Any, CultureInfo.InvariantCulture, out minutes));

            var power = PowerCalculation (hours * 3600 + minutes * 60);
            TimeCalculation (power);
            goto start;
        }

        private static int PowerCalculation (int seconds)
        {
            Console.WriteLine ("|\n| Calculating power...\n|");
            var power = CalculatePower (seconds);
            Console.WriteLine ($"|\n| This will need {power} W\n|");
            return power;
        }

        private static int CalculatePower (int seconds)
        {
            var tour = XmlUtil.Parse (File.ReadAllText (_data));

            var stepLength = 500.0;
            var current    = stepLength;
            while (stepLength > 0.5)
            {
                var time = CalculateTime (current, 0.1, tour);
                if ((int) time == seconds)
                    return (int) current;

                stepLength /= 2;

                if (time > seconds)
                    current += stepLength;
                else
                    current -= stepLength;
            }

            return (int) current;
        }

        #endregion
    }
}