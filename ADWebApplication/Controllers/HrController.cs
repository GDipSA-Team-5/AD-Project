using System.ComponentModel.DataAnnotations;
using ADWebApplication.Data;
using ADWebApplication.Models;
using ADWebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ADWebApplication.Controllers
{
    [Authorize(Roles = "HR")]
    public class HrController : Controller
    {
        private readonly EmpDbContext _db;

        public HrController(EmpDbContext empDb)
        {
            _db = empDb;
        }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var employees = await _db.EmpAccounts
            .OrderBy(e => e.Username)
            .Select(e => new EmployeeRowViewModel
            {
                Id = e.Id,
                FullName = e.FullName,
                Username = e.Username,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                IsActive = e.IsActive,
                RoleName = _db.EmpRoles
                    .Where(er => er.EmpAccountId == e.Id)
                    .Select(er => er.Role.Name)
                    .FirstOrDefault() ?? "-"
            })
            .ToListAsync();

        return View(employees);
    }
       [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            ViewBag.Roles = await _db.Roles
                .Where(r => r.Name != "HR")
                .Select(r => r.Name)
                .ToListAsync();

            return View(new CreateEmployeeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            ViewBag.Roles = await _db.Roles
                .Where(r => r.Name != "HR")
                .Select(r => r.Name)
                .ToListAsync();

            if (!ModelState.IsValid) return View(model);

            bool exists = await _db.EmpAccounts.AnyAsync(u => u.Username == model.Username);
            if (exists)
            {
                ModelState.AddModelError(nameof(model.Username), "This Employee ID already exists.");
                return View(model);
            }

            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == model.RoleName);
            if (role == null)
            {
                ModelState.AddModelError(nameof(model.RoleName), "Invalid role.");
                return View(model);
            }

            var user = new EmpAccount
            {
                Username = model.Username,
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsActive = true,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)

            };

            _db.EmpAccounts.Add(user);
            await _db.SaveChangesAsync();

            _db.EmpRoles.Add(new EmpRole { EmpAccountId = user.Id, RoleId = role.Id });
            await _db.SaveChangesAsync();

            TempData["Message"] = "Employee created successfully.";
            return RedirectToAction(nameof(CreateEmployee));
        }

    // GET Edit page
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var emp = await _db.EmpAccounts.FirstOrDefaultAsync(e => e.Id == id);
        if (emp == null) return NotFound();

        var roleName = await _db.EmpRoles
            .Where(er => er.EmpAccountId == id)
            .Select(er => er.Role.Name)
            .FirstOrDefaultAsync() ?? "";

        var vm = new EditEmployeeViewModel
        {
            Id = emp.Id,
            FullName = emp.FullName,
            Username = emp.Username,
            Email = emp.Email,
            PhoneNumber = emp.PhoneNumber,
            IsActive = emp.IsActive,
            RoleName = roleName
        };

        ViewBag.Roles = await _db.Roles
            .Where(r => r.Name != "HR") // usually HR shouldn't assign HR
            .Select(r => r.Name)
            .ToListAsync();

        return View(vm);
    }

    // POST Save Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditEmployeeViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = await _db.Roles.Where(r => r.Name != "HR").Select(r => r.Name).ToListAsync();
            return View(vm);
        }

        var emp = await _db.EmpAccounts.FirstOrDefaultAsync(e => e.Id == vm.Id);
        if (emp == null) return NotFound();

        // Unique username check (if HR edits EmployeeId)
        bool usernameTaken = await _db.EmpAccounts.AnyAsync(e => e.Username == vm.Username && e.Id != vm.Id);
        if (usernameTaken)
        {
            ModelState.AddModelError("Username", "Employee ID already exists.");
            ViewBag.Roles = await _db.Roles.Where(r => r.Name != "HR").Select(r => r.Name).ToListAsync();
            return View(vm);
        }

        // Update fields
        emp.FullName = vm.FullName;
        emp.Username = vm.Username;
        emp.Email = vm.Email;
        emp.PhoneNumber = vm.PhoneNumber;
        emp.IsActive = vm.IsActive;

        // Update role
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == vm.RoleName);
        if (role == null)
        {
            ModelState.AddModelError("RoleName", "Invalid role.");
            ViewBag.Roles = await _db.Roles.Where(r => r.Name != "HR").Select(r => r.Name).ToListAsync();
            return View(vm);
        }

        var empRole = await _db.EmpRoles.FirstOrDefaultAsync(er => er.EmpAccountId == emp.Id);
        if (empRole == null)
        {
            _db.EmpRoles.Add(new EmpRole { EmpAccountId = emp.Id, RoleId = role.Id });
        }
        else
        {
            empRole.RoleId = role.Id;
        }

        // reset password
        if (!string.IsNullOrWhiteSpace(vm.NewPassword))
        {
            emp.PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.NewPassword);
        }

        await _db.SaveChangesAsync();
        TempData["Message"] = "Employee updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET Delete confirmation page
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var emp = await _db.EmpAccounts.FirstOrDefaultAsync(e => e.Id == id);
        if (emp == null) return NotFound();

        var roleName = await _db.EmpRoles
            .Where(er => er.EmpAccountId == id)
            .Select(er => er.Role.Name)
            .FirstOrDefaultAsync() ?? "-";

        ViewBag.RoleName = roleName;
        return View(emp);
    }

    // POST Delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var emp = await _db.EmpAccounts.FirstOrDefaultAsync(e => e.Id == id);
        if (emp == null) return NotFound();

        // Remove role rows first (FK safe)
        var roles = await _db.EmpRoles.Where(er => er.EmpAccountId == id).ToListAsync();
        _db.EmpRoles.RemoveRange(roles);

        _db.EmpAccounts.Remove(emp);

        await _db.SaveChangesAsync();
        TempData["Message"] = "Employee deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
}

