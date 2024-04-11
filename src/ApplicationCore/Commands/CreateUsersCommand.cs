﻿using ApplicationCore.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.DTOs;

namespace ApplicationCore.Commands
{
    public class CreateUsersCommand : UserDto, IRequest<Response<int>>
    {

    }
}
