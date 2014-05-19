using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using metrics.Core;

namespace metrics.Serialization
{
    public class Serializer
    {
        public static string Serialize<T>(T entity)
        {
            if (entity is IDictionary<MetricName, IMetric>)
            {
                var collection = (IDictionary<MetricName, IMetric>)entity;
                List<MetricItem> container = new List<MetricItem>(collection.Count);
                container.AddRange(collection.Select(item => new MetricItem { Name = item.Key.Name, Metric = item.Value }));
                var serialized = Serialize(container);
                return serialized;
            }
            return JsonConvert.SerializeObject(entity, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }

    internal class MetricItem
    {
        public string Name { get; set; }
        public IMetric Metric { get; set; }
    }
}
