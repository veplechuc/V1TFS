using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integrations.Core.Extensions;
using NSpec;


namespace Integration.Core.Tests
{
    public class UriExtensionsSpecs : nspec
    {
        public void given_the_need_to_combine_strings_to_construct_a_uri()
        {

            const string baseUri = "http://www.google.com/";
            const string relativeUri = "mypath1";
            const string resourceName = "index.html";
            const string expectedResult = "http://www.google.com/mypath1/index.html";

            context["when a base uri and relative path are combined"] = () =>
                {
                    var actualResult = new Uri(baseUri).Append(relativeUri, resourceName);
                    it["then a valid uri results"] = () => actualResult.ToString().should_be(expectedResult);
                };
        }
    }
}
