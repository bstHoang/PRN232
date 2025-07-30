using Project.DTOs.Roles;

namespace Project.Interfaces
{
    public interface IRoleService
    {
        Task<bool> UpdateUserRoleAsync(UpdateUserRoleDto dto);
    }
}
