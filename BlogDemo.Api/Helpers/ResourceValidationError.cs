using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDemo.Api.Helpers
{
    public class ResourceValidationError
    {
        /// <summary>
        /// Stored the error message and the validation key
        /// </summary>
        public string Validationkey { get; private set; }
        public string Message { get; private set; }

        public ResourceValidationError(string message, string validationKey = "")
        {
            Validationkey = validationKey;
            Message = message;
        }
    }
}
