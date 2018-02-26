using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenManagement.Data
{
    public class ValidationErrorMessage : IValidationMessage
    {
        public ValidationErrorMessage() : this(string.Empty)
        { }

        public ValidationErrorMessage(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }
    }
}
