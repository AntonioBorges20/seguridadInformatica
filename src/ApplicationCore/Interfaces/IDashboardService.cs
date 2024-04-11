using ApplicationCore.DTOs;
using ApplicationCore.Wrappers;

namespace ApplicationCore.Interfaces
{
    public interface IDashboardService
    {
        Task<Response<object>> GetData();
        Task<Response<string>> GetClientIpAddress();
        Task<Response<int>> CreateLogs(LogsDto logsDto);
        Task<Response<int>> CreateUser(UserDto userDto);
        Task<Response<int>> DeleteUser(int id);
        Task<Response<int>> UpdateUser(int id,UserDto userDto);
        Task<Response<object>> GetPaginated();
    }
}
