using ApplicationCore.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using AutoMapper;

namespace ApplicationCore.Mappings
{ 
    public class UsuarioProfile: Profile
    {
        public UsuarioProfile()
        {
            CreateMap<CreateUsersCommand, usuario>().ForMember(x => x.pkUsuario, y => y.Ignore());
        }
    }
}
