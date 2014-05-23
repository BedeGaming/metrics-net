using System;

namespace metrics.Core
{
    /// <summary>
    /// A hash key for storing metrics associated by the resource name and metric name pair
    /// </summary>
    public struct MetricName : IComparable<MetricName>
    {
        public string ResourceName { get; private set; }

        public string Name { get; private set; }

        public MetricName(string resourceName, string name) : this()
        {
            ResourceName = resourceName;
            Name = name;
        }

        public bool Equals(MetricName other)
        {
            return Equals(other.Name, Name) && Equals(other.ResourceName, ResourceName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MetricName && Equals((MetricName) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (ResourceName != null ? ResourceName.GetHashCode() : 0);
            }
        }

        public static bool operator == (MetricName left, MetricName right)
        {
            return left.Equals(right);
        }

        public static bool operator != (MetricName left, MetricName right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(MetricName other)
        {
            return string.Concat(ResourceName, ".", Name).CompareTo(string.Concat(other.ResourceName, ".", other.Name));
        }

        public override string ToString()
        {
            return string.Concat(ResourceName, ".", Name);
        }
    }
}



