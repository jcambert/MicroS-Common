using System.Collections.Generic;
using WeCommon;

namespace MicroS_Common.Domain
{
    public interface IValidateContext
    {
        bool IsValid { get; }
        IEnumerable<string> GetMessages();
        void AddMessage(string message);
    }
    public class ValidateContext : IValidateContext
    {
        WeStringBuilder sb = new WeStringBuilder();
        bool _isValid = false;
        public bool IsValid => !sb.HasItems;

        public void AddMessage(string message)
        {
            sb.Add(message);
        }

        public IEnumerable<string> GetMessages()=>  sb.AsEnumerable;

        public override string ToString() => sb.Join("\n");
    }

}
