using System;
using System.Globalization;
using System.IO;

using Letras_Simulation.Model;
using Letras_Simulation.Util;


namespace Letras_Simulation
{
    internal class Program
    {
        public static void Main (string [] args)
        {
            start:
            double power;
            do
                Console.WriteLine ("Please enter your power");
            while (!double.TryParse (Console.ReadLine (), NumberStyles.Any, CultureInfo.InvariantCulture, out power));

            Console.WriteLine ("Calculating time...");

            var tour = XmlUtil.Parse (File.ReadAllText ("Resources/Data.gpx"));
            // Me, my bike, my bottles and other stuff
            var state = new State (tour, 66.2 + 7.9 + 1.5 + 2.0);

            bool res;
            do
                res = state.Step (power);
            while (!res);

            var hours = (int) (state.ElapsedTime / 3600.0);
            var minutes = (int) (state.ElapsedTime / 60.0 - hours * 60.0);
            var seconds = (int) (state.ElapsedTime - hours * 3600.0 - minutes * 60.0);
            Console.WriteLine ($"This will take {state.ElapsedTime} s, meaning about {hours} h, {minutes} m, {seconds} s");
            goto start;
        }
    }
}