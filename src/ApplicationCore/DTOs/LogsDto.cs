using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs
{
    public class LogsDto
    {
        public int? pkLogs { get; set; }
        public string? Fecha { get; set; }
        public string? IpLogs { get; set; }
        public string? NombreFuncion { get; set; }
        public string? StatusLogs { get; set; }
        public string? Datos { get; set; }
    }
}
