using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integrations.Core.Extensions
{
    public static class UriExtensions
    {
        public static Uri Append(this Uri baseUri, params string[] paths)
        {
            return new Uri(paths.Aggregate(baseUri.AbsoluteUri, (current, path) => 
                string.Format("{0}/{1}", 
                current.TrimEnd('/'), 
                path.TrimStart('/'))));
        }
    }
}
