using ApplicationCore.Commands;
using ApplicationCore.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Mappings
{
    public class LogsProfile: Profile
    {
        public LogsProfile()
        {
            CreateMap<LogsDto, logs>().ForMember(x => x.pkLogs, y => y.Ignore());
        }
    }
}
