using System;
using System.Globalization;
using System.IO;
using System.Xml;

using Letras_Simulation.Model;


namespace Letras_Simulation.Util
{
    public static class XmlUtil
    {
        public static Tour Parse (string raw)
        {
            var        tour              = new Tour ();
            Trackpoint currentTrackPoint = null;
            var        readingAltitude   = false;

            using (var rawReader = new StringReader (raw))
            using (var xmlReader = XmlReader.Create (rawReader))
                while (xmlReader.Read ())
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (xmlReader.Name)
                            {
                                case "trkpt":
                                    currentTrackPoint = new Trackpoint ();
                                    currentTrackPoint.SetLatitude(Convert.ToDouble (xmlReader.GetAttribute ("lat"), CultureInfo.InvariantCulture));
                                    currentTrackPoint.SetLongitude(Convert.ToDouble (xmlReader.GetAttribute ("lon"), CultureInfo.InvariantCulture));
                                    break;
                                case "ele":
                                    readingAltitude = true;
                                    break;
                            }

                            break;
                        case XmlNodeType.Text:
                            if (!readingAltitude)
                                break;
                            if (currentTrackPoint == null)
                                throw new Exception ("Malformed data");
                            currentTrackPoint.SetAltitude (Convert.ToDouble (xmlReader.Value, CultureInfo.InvariantCulture));
                            break;
                        case XmlNodeType.EndElement:
                            switch (xmlReader.Name)
                            {
                                case "trkpt":
                                    if (currentTrackPoint == null)
                                        throw new Exception ("Malformed data");
                                    tour.Add (currentTrackPoint);
                                    currentTrackPoint = null;
                                    break;
                                case "ele":
                                    if (!readingAltitude)
                                        throw new Exception ("Malformed data");
                                    readingAltitude = false;
                                    break;
                            }
                            break;
                    }

            return tour;
        }
    }
}