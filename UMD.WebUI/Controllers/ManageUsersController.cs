using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMD.Data;
using UMD.Models;
using Microsoft.Extensions.Configuration;

namespace UMD.WebUI.Controllers;

public class ManageUsersController : CommonBaseClass
{
    private readonly UMDContext _context;

    public ManageUsersController(UMDContext context, IConfiguration configuration)
       : base(configuration)
    {
        _context = context;
    }

    public async Task<IActionResult> ManageUsers(int? id)
    {
        var users = await UsersData.GetList(_context); 

        User selectedUser = null;
        if (id.HasValue)
        {
            selectedUser = await UsersData.GetUser(id.Value, _context); 
        }

        ViewBag.Users = users; 
        return View(selectedUser); 
    }
    public IActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddUser(User user, string confirmPassword)
    {
        user.Password = ComputePasswordHash(user.Email, confirmPassword);

        if (!ModelState.IsValid)
        {
            return View(user);
        }

        try
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);
            TempData["SuccessMessage"] = "The username "+ user.Email + " has been successfully created";


            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already in use.");
                return View(user);
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
            return View(user);
        }

        return RedirectToAction("AddUser");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> UpdatePersonalSection(User user)
    {
        ModelState.Remove("Email");
        ModelState.Remove("Password");

        if (ModelState.IsValid)
        {
            try
            {
                await UsersData.UpdatePersonalSection(user, _context);
                TempData["SuccessMessage"] = "The Personal Section has been updated.";
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
                return (user.Id == 0) ? View() : View(user);
            }
        }
        return RedirectToAction("ManageUsers"); 

    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> UpdateSecuritySection(User user, string confirmPassword)
    {
        user.Password = ComputePasswordHash(user.Email, confirmPassword);

        ModelState.Remove("FirstName");
        ModelState.Remove("LastName");

        if (ModelState.IsValid)
        {
            try
            {
                await UsersData.UpdateSecuritySection(user, _context);
                TempData["SuccessMessage"] = "The Security Section has been updated.";
            }
            catch (Exception ex)
            {
                TempData["DangerMessage"] = ex.Message;
                return (user.Id == 0) ? View() : View(user);
            }
        }
        return RedirectToAction("ManageUsers");

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Remove(User user)
    {
        try
        {
            await UsersData.Remove(user, _context);
        }
        catch (Exception ex)
        {
            TempData["DangerMessage"] = ex.Message;
        }

        return RedirectToAction("ManageUsers");
    }
}
