using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using ApplicationCore.Wrappers;
using AutoMapper;
using Dapper;
using Infraestructure.Persistence;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Net.Sockets;
using System.Net;
using ApplicationCore.Commands;
using Org.BouncyCastle.Asn1.Ocsp;
using Newtonsoft.Json;

namespace Infraestructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardService(ApplicationDbContext dbContext, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<Response<object>> GetData()
        {
            object list = new object();
            list = await _dbContext.usuario.ToListAsync();

            return new Response<object>(list);
        }
        public async Task<Response<string>> GetClientIpAddress()
        {
            // Obtener el objeto IPHostEntry que contiene información sobre el host local.
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            // Seleccionar la primera dirección IP de la lista que sea de tipo InterNetwork (IPv4).
            IPAddress ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            // Convertir la dirección IP a una cadena de texto. Si la dirección IP es nula, asigna un mensaje de error.
            var ipAddressString = ipAddress?.ToString() ?? "No se pudo determinar la dirección IP del servidor";

            // Crear un nuevo objeto Response<string> para encapsular la dirección IP obtenida.
            // Response<string> es una clase ficticia que parece utilizarse para contener un valor de tipo string
            // junto con un mensaje asociado.
            return new Response<string>(ipAddressString);

        }

        
        public async Task<Response<int>> CreateUser(UserDto request)
        {
            try
            {
                var u = new CreateUsersCommand();
                u.Nombre = request.Nombre;
                u.Apellido = request.Apellido;
                u.Telefono = request.Telefono;
                u.Correo = request.Correo;
                u.Direccion = request.Direccion;

                var us = _mapper.Map<Domain.Entities.usuario>(u);
                await _dbContext.usuario.AddAsync(us);
                var req = await _dbContext.SaveChangesAsync();
                var res = new Response<int>(us.pkUsuario, "Registro creado");

                

                //logs
                var c = new LogsDto();
                c.Datos = JsonConvert.SerializeObject(request);
                c.Fecha = DateTime.Now.ToString();
                c.NombreFuncion = "Create";
                c.IpLogs = res.Message;
                c.StatusLogs = "200";

                await CreateLogs(c);
                return res;

            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                var errorLog = new LogsDto();
                errorLog.Datos = JsonConvert.SerializeObject(request);
                errorLog.Fecha = DateTime.Now.ToString();
                

                if (ex.InnerException != null)
                {
                    errorLog.NombreFuncion = $"Error desconocido al crear el registro. Mensaje interno: {ex.InnerException.Message}";
                }
                else
                {
                    errorLog.NombreFuncion = "Error desconocido al crear el registro";
                }

                errorLog.StatusLogs = "500";

                await CreateLogs(errorLog);
                throw;
            }


            //var ipAddress = await GetClientIpAddress();
            //var ip = ipAddress.Message;
        }

        public async Task<Response<int>> CreateLogs(LogsDto request)
        {

            var ipAddress = await GetClientIpAddress();
            var ip = ipAddress.Message;

            var c = new LogsDto();
            c.Fecha = request.Fecha;
            c.IpLogs = ip;
            c.NombreFuncion = request.NombreFuncion;
            c.StatusLogs = request.StatusLogs;
            c.Datos = JsonConvert.SerializeObject(request);

            var ca = _mapper.Map<Domain.Entities.logs>(c);
            await _dbContext.logs.AddAsync(ca);
            await _dbContext.SaveChangesAsync();
            return new Response<int>(ca.pkLogs, "Registro creado");
        }

        public async Task<Response<int>> DeleteUser(int id)
        {
            try { 

                if(id == null)
                {
                    throw new ArgumentException("El id no puede ser nulo");
                }

                var c = await _dbContext.usuario.FindAsync(id);
                _dbContext.usuario.Remove(c);

                await _dbContext.SaveChangesAsync();
                var res = new Response<int>(id, "Registro eliminado. ID: "+id);

                var logs = new LogsDto();
                logs.Datos = "id "+id+" eliminado";
                logs.Fecha = DateTime.Now.ToString();
                logs.NombreFuncion = "Delete";
                logs.IpLogs = res.Message;
                logs.StatusLogs = "200";

                await CreateLogs(logs);

                return res;
            
            }catch (Exception ex)
            {
                var errorLog = new LogsDto();
                errorLog.Datos = "id "+id+" eliminado";
                errorLog.Fecha = DateTime.Now.ToString();

                if (ex.InnerException != null)
                {
                    errorLog.NombreFuncion = $"Error desconocido al eliminar el registro. Mensaje interno: {ex.InnerException.Message}";
                }
                else
                {
                    errorLog.NombreFuncion = "Error desconocido al eliminar el registro";
                }

                errorLog.StatusLogs = "500";

                await CreateLogs(errorLog);
                throw;  
            }
        }


        public async Task<Response<int>> UpdateUser(int id, UserDto request)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentException("El id no puede ser nulo");
                }

                var c = await _dbContext.usuario.FindAsync(id);
                c.Nombre = request.Nombre;
                c.Apellido = request.Apellido;
                c.Telefono = request.Telefono;
                c.Correo = request.Correo;
                c.Direccion = request.Direccion;

                await _dbContext.SaveChangesAsync();
                var res = new Response<int>(id, "Registro actualizado. ID: " + id);

                var logs = new LogsDto();
                logs.Datos = JsonConvert.SerializeObject(request);
                logs.Fecha = DateTime.Now.ToString();
                logs.NombreFuncion = "Update";
                logs.IpLogs = res.Message;
                logs.StatusLogs = "200";

                await CreateLogs(logs);

                return res;

            }
            catch (Exception ex)
            {
                var errorLog = new LogsDto();
                errorLog.Datos = JsonConvert.SerializeObject(request);
                errorLog.Fecha = DateTime.Now.ToString();

                if (ex.InnerException != null)
                {
                    errorLog.NombreFuncion = $"Error desconocido al actualizar el registro. Mensaje interno: {ex.InnerException.Message}";
                }
                else
                {
                    errorLog.NombreFuncion = "Error desconocido al actualizar el registro";
                }

                errorLog.StatusLogs = "500";

                await CreateLogs(errorLog);
                throw;
            }   
        }

        public async Task<Response<object>> GetPaginated()
        {
            object list = new object();

            int pageNumber = 1;
            int pageSize = 10;

            list = await _dbContext.usuario.OrderBy(x => x.pkUsuario)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Response<object>(list);
        }

    }
}
