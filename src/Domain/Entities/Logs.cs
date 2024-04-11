using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    //internal class users
    //{
    //}
    [Table("logs")]
    public class logs
    {
        [Key]
        public int pkLogs { get; set; }
        public string Fecha { get; set; }
        public string IpLogs { get; set; }
        public string NombreFuncion { get; set; }
        public string StatusLogs { get; set; }
        public string Datos { get; set; }
    }
}
