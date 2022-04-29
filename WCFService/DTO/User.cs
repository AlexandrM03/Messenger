using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFService.DTO
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public int IdMedia { get; set; }
        public Media Media { get; set; }
        public int IdUserAuth { get; set; }
        public UserAuth UserAuth { get; set; }
    }
}
