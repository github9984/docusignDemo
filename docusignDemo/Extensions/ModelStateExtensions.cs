using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace docusignDemo.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetErrors(IEnumerable<ModelState> errors)
        {
            return errors.SelectMany(modelState => modelState.Errors).Aggregate("", (current, error) => current + (error.ErrorMessage + " "));
        }
    }
}