using Microsoft.EntityFrameworkCore;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;

namespace SpeakingShorts.Service.Services.Users;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public async ValueTask<User> CreateAsync(User user)
    {
        var existUser = await unitOfWork.UserRepository.SelectAsync(u => u.Email == user.Email);
        if (existUser?.Email is not null)
        {
            throw new AlreadyExistException($"This user is already exist with this email | Email={user.Email}");
        }

        var roleWhichIsUser = await unitOfWork.UserRoleRepository.SelectAsync(u => u.Name.ToLower() == "user");

        user.UserRoleId = roleWhichIsUser.Id;
        user.PasswordHash = PasswordHasher.Hash(user.PasswordHash);
        var createdUser = await unitOfWork.UserRepository.InsertAsync(user);
        await unitOfWork.SaveAsync();
        return createdUser;
    }

    public async ValueTask<User> ModifyAsync(long id, User user)
    {
        var currentUserId = HttpContextHelper.GetUserId;

        // 1. Foydalanuvchi faqat o'zini tahrirlashi mumkin
        if (id != currentUserId)
            throw new ForbiddenException("You are not allowed to modify another user's data.");

        // 2. Mavjud foydalanuvchini olish
        var existUser = await unitOfWork.UserRepository.SelectAsync(u => u.Id == id)
            ?? throw new NotFoundException("User not found");

        // 3. Faqat Username ni yangilash
        existUser.Username = user.Username;
        existUser.UpdatedById = currentUserId;
        existUser.UpdatedAt = DateTime.UtcNow;

        // 4. Saqlash
        var updatedUser = await unitOfWork.UserRepository.UpdateAsync(existUser);
        await unitOfWork.SaveAsync();

        return updatedUser;
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var exsitUser = await unitOfWork.UserRepository.SelectAsync(user => user.Id == id)
            ?? throw new NotFoundException("This user is not found");

        exsitUser.DeletedById = HttpContextHelper.GetUserId;
        await unitOfWork.UserRepository.DeleteAsync(exsitUser);
        await unitOfWork.SaveAsync();
        return true;
    }

    public async ValueTask<User> GetAsync(long id)
    {
        return await unitOfWork.UserRepository
            .SelectAsync(expression: user => user.Id == id, includes: ["Role"])
            ?? throw new NotFoundException("This user is not found");
    }

    public async ValueTask<IEnumerable<User>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var users = unitOfWork.UserRepository.Select(isTracking: false, includes: ["Role"]);

        if (!string.IsNullOrWhiteSpace(search))
            users = users.Where(u =>
                u.Email.ToLower().Contains(search.ToLower()));

        return await users.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
    }

    public async ValueTask<User> ChangePasswordAsync(string oldPasword, string newPassword, string confirmPassword)
    {
        var existUser = await unitOfWork.UserRepository.SelectAsync(user => user.Id == HttpContextHelper.GetUserId)
            ?? throw new NotFoundException("User is not found");

        if (!PasswordHasher.Verify(oldPasword, existUser.PasswordHash))
            throw new ArgumentIsNotValidException("Old password is invalid");

        if (newPassword != confirmPassword)
            throw new ArgumentIsNotValidException("Password is not match");

        existUser.PasswordHash = PasswordHasher.Hash(newPassword);
        await unitOfWork.UserRepository.UpdateAsync(existUser);
        await unitOfWork.SaveAsync();

        return existUser;
    }

    public async ValueTask<User> ChangeRoleAsync(long userId, long roleId)
    {
        var existUser = await unitOfWork.UserRepository.SelectAsync(user => user.Id == userId)
            ?? throw new NotFoundException($"User is not found with this ID={userId}");

        var existRole = await unitOfWork.UserRoleRepository.SelectAsync(role => role.Id == roleId)
            ?? throw new NotFoundException($"Role is not found with this ID={roleId}");

        existUser.UserRoleId = existRole.Id;
        await unitOfWork.UserRepository.UpdateAsync(existUser);
        await unitOfWork.SaveAsync();

        return existUser;
    }

    public async ValueTask<IEnumerable<User>> GetAllAsync()
    {
        return await unitOfWork.UserRepository
            .Select(isTracking: false, includes: ["Role"])
            .ToListAsync();
    }
}