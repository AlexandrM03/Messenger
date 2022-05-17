using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MessengerClient.Logic.Model
{
    public class LoginModel : IDataErrorInfo
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Login":
                        if (String.IsNullOrEmpty(Login))
                            error = "Login is required";
                        else if (!Regex.IsMatch(Login, @"^[A-ZА-Я][A-Za-zА-Яа-я0-9_-]+$"))
                            error = "Incorrect login";
                        break;
                }

                return error;
            }
        }
        public string Error => throw new NotImplementedException();
    }
}
