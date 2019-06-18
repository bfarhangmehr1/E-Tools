using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eToolsSystem.BLL
{
    [Serializable]
    public class BusinessRuleException : Exception
    {
        public List<string> RuleDetails { get; set; }

        public BusinessRuleException(string message, List<string> reasons) : base(message)
        {
            this.RuleDetails = reasons;
        }

        public BusinessRuleException(string message, string reason) : base(message)
        {
            this.RuleDetails = new List<string> { reason };
        }
    }
}
