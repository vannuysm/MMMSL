using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using mmmsl.Models;

namespace mmmsl.RouteConstraints
{
    public class DivisionRouteConstraint : IRouteConstraint
    {
        private readonly MmmslDatabase database;

        public DivisionRouteConstraint(MmmslDatabase database)
        {
            this.database = database;
        }
        
        public bool Match(HttpContext httpContext, IRouter route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (parameterName == null) {
                throw new ArgumentNullException(nameof(parameterName));
            }

            if (values == null) {
                throw new ArgumentNullException(nameof(values));
            }

            object value;
            if (values.TryGetValue(parameterName, out value) && value != null) {
                if (!(value is string)) {
                    return false;
                }

                var divisionId = Convert.ToString(value, CultureInfo.InvariantCulture);
                return database.Divisions.Any(division => division.Id == divisionId);
            }
            
            return false;
        }
    }
}