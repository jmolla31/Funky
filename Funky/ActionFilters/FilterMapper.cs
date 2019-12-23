using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funky.Filters.ActionFilters
{
    public class FilterMapper
    {
        private readonly ICollection<KeyValuePair<string, Type>> FilterMappings = new List<KeyValuePair<string, Type>>();

        public void MapFilter<TClass, TFilter>() => this.FilterMappings.Add(new KeyValuePair<string, Type>(nameof(TClass), typeof(TFilter)));

        public void MapFilter<TFilter>(string actionName) => this.FilterMappings.Add(new KeyValuePair<string, Type>(actionName, typeof(TFilter)));

        public IEnumerable<Type> GetFilters(string actionName) => this.FilterMappings.Where(x => x.Key == actionName).Select(y => y.Value);

        public Type GetSingleFilter(string actionName) => this.FilterMappings.FirstOrDefault(x => x.Key == actionName).Value;
    }
}
