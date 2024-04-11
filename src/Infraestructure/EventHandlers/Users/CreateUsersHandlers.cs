using ApplicationCore.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ApplicationCore.Commands;
using Infraestructure.Persistence;
using ApplicationCore.Interfaces;
using ApplicationCore.DTOs;


namespace Infraestructure.EventHandlers.Users
{
    public class CreateLogsHandlers : IRequestHandler<CreateUsersCommand, Response<int>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly CreateLogsCommand _createLogsCommand;
        private readonly IDashboardService _dashboardService;
        public CreateLogsHandlers(ApplicationDbContext context, IMapper mapper, IDashboardService dashboardService)
        {
            _context = context;
            _mapper = mapper;
            //_createLogsCommand = new CreateLogsCommand();
            _dashboardService = dashboardService;
        }

        public async Task<Response<int>> Handle(CreateUsersCommand request, CancellationToken cancellationToken)
        {

            
            var c = new CreateUsersCommand();
            c.Nombre = request.Nombre; 
            c.Apellido = request.Apellido;
            c.Telefono = request.Telefono;
            c.Correo = request.Correo;
            c.Direccion = request.Direccion;

            var ca = _mapper.Map<Domain.Entities.usuario>(c);

            await _context.usuario.AddAsync(ca);
            var req = await _context.SaveChangesAsync();
            

            if (req == 1) {

                var co = new Response<int>(ca.pkUsuario, "Registro creado");

                var logs = new LogsDto();
                logs.Fecha = DateTime.Now.ToString();
                logs.IpLogs = "IPs";
                logs.NombreFuncion = "Create";
                logs.StatusLogs = co.Message;
                logs.Datos = "Se ha creado un nuevo usuario";

                await _dashboardService.CreateLogs(logs);
                return co;
            }
            else
            {
                var co = new Response<int>("Error");
                return new Response<int>(0, "Error al crear el registro");
            }
            

        }
    }
}
        