using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenManagement.Data
{
    public abstract class ValidatableBase : IValidatable, INotifyPropertyChanged
    {
        private Dictionary<string, List<IValidationMessage>> validationMessages =
            new Dictionary<string, List<IValidationMessage>>();
        public event PropertyChangedEventHandler PropertyChanged;


        public Dictionary<string, List<IValidationMessage>> ValidationMessages
        {
            get
            {
                return this.validationMessages;
            }
            private set
            {
                this.validationMessages = value;
                this.OnPropertyChanged("ValidationMessages");
            }
        }
        public bool HasValidationMessageType<T>(string property = "")
        {
            if (string.IsNullOrEmpty(property))
            {
                bool result = this.validationMessages.Values.Any(collection =>
                    collection.Any(msg => msg is T));
                return result;
            }

            return this.validationMessages.ContainsKey(property);
        }
        public void AddValidationMessage(IValidationMessage message, string property = "")
        {
            if (string.IsNullOrEmpty(property))
            {
                return;
            }

            // If the key does not exist, then we create one.
            if (!this.validationMessages.ContainsKey(property))
            {
                this.validationMessages[property] = new List<IValidationMessage>();
            }

            if (this.validationMessages[property].Any(msg => msg.Message.Equals(message.Message) || msg == message))
            {
                return;
            }

            this.validationMessages[property].Add(message);
        }
        public void RemoveValidationMessage(string message, string property = "")
        {
            if (string.IsNullOrEmpty(property))
            {
                return;
            }

            if (!this.validationMessages.ContainsKey(property))
            {
                return;
            }

            if (this.validationMessages[property].Any(msg => msg.Message.Equals(message)))
            {
                // Remove the error from the key's collection.
                this.validationMessages[property].Remove(
                    this.validationMessages[property].FirstOrDefault(msg => msg.Message.Equals(message)));
            }
        }
        public void RemoveValidationMessages(string property = "")
        {
            if (string.IsNullOrEmpty(property))
            {
                return;
            }

            if (!this.validationMessages.ContainsKey(property))
            {
                return;
            }

            this.validationMessages[property].Clear();
            this.validationMessages.Remove(property);
        }
        public IValidationMessage ValidateProperty(
            Func<string,
            IValidationMessage> validationDelegate,
            string failureMessage,
            string propertyName = "")
        {
            IValidationMessage result = validationDelegate(failureMessage);
            if (result != null)
            {
                this.AddValidationMessage(result, propertyName);
            }
            else
            {
                this.RemoveValidationMessage(failureMessage, propertyName);
            }

            return result;
        }
        public abstract void Validate();

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
