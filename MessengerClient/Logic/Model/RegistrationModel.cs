using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MessengerClient.Logic.Model
{
    public class RegistrationModel : IDataErrorInfo
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Path { get; set; }
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
                    case "Name":
                        if (String.IsNullOrEmpty(Name))
                            error = "Name is required";
                        else if (!Regex.IsMatch(Name, @"^[A-ZА-Я][A-Za-zА-Яа-я]+$"))
                            error = "Incorrect name";
                        break;
                    case "Surname":
                        if (String.IsNullOrEmpty(Surname))
                            error = "Surname is required";
                        else if (!Regex.IsMatch(Surname, @"^[A-ZА-Я][A-Za-zА-Яа-я]+$"))
                            error = "Incorrect surname";
                        break;
                }

                return error;
            }
        }

        public string Error => throw new NotImplementedException();
    }
}
