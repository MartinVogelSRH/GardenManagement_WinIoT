using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenManagement.Data
{
    public interface IValidatable
    {
        Dictionary<string, List<IValidationMessage>> ValidationMessages { get; }

        void AddValidationMessage(IValidationMessage message, string property = "");
        void RemoveValidationMessage(string message, string property = "");
        void RemoveValidationMessages(string property = "");
        bool HasValidationMessageType<T>(string property = "");
        IValidationMessage ValidateProperty(
            Func<string,
            IValidationMessage> validationDelegate,
            string failureMessage,
            string propertyName = "");
    }
}

