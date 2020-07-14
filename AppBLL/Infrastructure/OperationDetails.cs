using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBLL.Infrastructure
{
    /// <summary>
    /// This class will store information about the success of the operation.
    /// </summary>
    public class OperationDetails
    {
        public OperationDetails(bool succedeed, string message, string prop)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }
        public bool Succedeed { get; private set; }
        public string Message { get; private set; } // error message
        public string Property { get; private set; } // the property on which the error occurred
    }
}
