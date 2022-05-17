using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Logic.Model.Validation
{
    public interface IDataErrorInfo
    {
        string Error { get; }
        string this[string columnName] { get; }
    }
}
