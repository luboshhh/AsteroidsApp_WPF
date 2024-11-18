using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidsAppWPF
{
    public class NearEarthObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPotentiallyHazardousAsteroid { get; set; }
        public EstimatedDiameter EstimatedDiameter { get; set; }
        public List<CloseApproachData> CloseApproachData { get; set; }
    }

    public class CloseApproachData
    {
        public string CloseApproachDate { get; set; }
        public RelativeVelocity RelativeVelocity { get; set; }
        public MissDistance MissDistance { get; set; }
    }

    public class RelativeVelocity
    {
        public string KilometersPerSecond { get; set; }
    }

    public class MissDistance
    {
        public string Kilometers { get; set; }
    }

    public class EstimatedDiameter
    {
        public Diameter Kilometers { get; set; }
    }

    public class Diameter
    {
        public double EstimatedDiameterMin { get; set; }
        public double EstimatedDiameterMax { get; set; }
    }

    public class NeoFeedResponse
    {
        public List<NearEarthObject> NearEarthObjects { get; set; }
    }
}
