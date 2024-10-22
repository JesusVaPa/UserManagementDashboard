using Microsoft.EntityFrameworkCore;
using UMD.Models;

namespace UMD.Data;

public static class UsersData
{
    public static async Task Insert(User user, UMDContext context)
    {
        var entity = await context.Users.FindAsync(user.Id);

        if (entity != null)
        {
            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;

            await context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("User not found.");
        }
    }

    public static async Task UpdatePersonalSection(User user, UMDContext context)
    {
        var entity = await context.Users.FindAsync(user.Id);

        if (entity != null)
        {
            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;

            await context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("User not found.");
        }
    }

    public static async Task UpdateSecuritySection(User user, UMDContext context)
    {
        var existingUser = await context.Users
            .Where(u => u.Email == user.Email && u.Id != user.Id)
            .FirstOrDefaultAsync();

        if (existingUser != null)
            throw new Exception("This email address has already been used.");

        var entity = await context.Users.FindAsync(user.Id);
        if (entity == null)
            throw new Exception("User not found.");

        entity.Email = user.Email;
        entity.Password = user.Password;

        await context.SaveChangesAsync();
    }

    public static async Task<User?> GetUser(int userId, UMDContext context)
    {
        return await context.Users.FindAsync(userId);
    }

    public static async Task<List<User>> GetList(UMDContext context)
    {
        return await context.Users.ToListAsync();
    }

    public static async Task Remove(User user, UMDContext context)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
}
