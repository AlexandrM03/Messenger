using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFService.DTO
{
    public class ChatUser
    {
        [Key, Column(Order = 0)]
        public int IdChat { get; set; }
        public Chat Chat { get; set; }
        [Key, Column(Order = 1)]
        public int IdUser { get; set; }
        public User User { get; set; }
    }
}
