using AviMerch.Application.DTO;

public interface IAuthService
{
Task<List<AdminUserResponse>> GetAllUsersAsync();

}