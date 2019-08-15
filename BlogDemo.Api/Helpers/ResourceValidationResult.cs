using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDemo.Api.Helpers
{
    public class ResourceValidationResult : Dictionary<string, IEnumerable<ResourceValidationError>>
    {
        /// <summary>
        /// Dictionary with modelState-Key as key and ResourceValidationError-object as value
        /// </summary>
        public ResourceValidationResult() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public ResourceValidationResult(ModelStateDictionary modelState) : this()
        {
            if (modelState == null)
            {
                throw new ArgumentException(nameof(modelState));
            }

            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    var errorToAdd = new List<ResourceValidationError>();
                    foreach(var error in errors)
                    {
                        var keyAndMessage = error.ErrorMessage.Split('|');

                        if (keyAndMessage.Length > 1)
                        {
                            errorToAdd.Add(new ResourceValidationError(keyAndMessage[1], keyAndMessage[0]));
                        }
                        else
                        {
                            errorToAdd.Add(new ResourceValidationError(keyAndMessage[0]));
                        }
                    }
                    Add(key, errorToAdd);
                }
            }
        }
    }
}
