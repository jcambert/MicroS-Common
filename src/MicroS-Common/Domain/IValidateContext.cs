using System.Collections.Generic;
using WeCommon;

namespace MicroS_Common.Domain
{
    public interface IValidateContext
    {
        bool IsValid { get; }
        IEnumerable<string> GetMessages();
        void AddMessage(string message);
        void Clear();
    }
    public class ValidateContext : IValidateContext
    {
        public ValidateContext()
        {

        }
        WeStringBuilder sb = new WeStringBuilder();
        public bool IsValid => !sb.HasItems;

        public void AddMessage(string message)
        {
            sb.Add(message);
        }

        public IEnumerable<string> GetMessages()=>  sb.AsEnumerable;

        public override string ToString() => sb.Join("\n");

        public void Clear()
        {
            sb = new WeStringBuilder();
        }
    }

}
