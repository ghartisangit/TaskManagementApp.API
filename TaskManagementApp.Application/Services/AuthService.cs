using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.Common.Exceptions;
using TaskManagementApp.Application.DTOs.Auth;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Application.Persistance;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Services
{
    public class AuthService: IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto)
        {
            
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new System.ComponentModel.DataAnnotations.ValidationException("Email already exists.");

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == registerDto.RoleName);
            if (role == null)
                throw new NotFoundException("Invalid role provided.");

            var user = new User
            {
                FullName = registerDto.FullName,
                PasswordHash = HashPassword(registerDto.Password),
                Email= registerDto.Email,
                RoleId = role.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email= user.Email,
                Role = role.Name,
                Token = token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto)
        {
            var user = await _context.Users.Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if(user == null)
                throw new UnauthorizedException("Invalid email address.");

            
            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid password.");

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email= user.Email,
                Role = user.Role.Name,
                Token = token
            };
        }
        public async Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new Exception("User not found.");

            
            if (!VerifyPassword(oldPassword, user.PasswordHash))
                throw new Exception("Old password is incorrect.");

            user.PasswordHash = HashPassword(newPassword);

            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var users= await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(u => u.UserId == id);
            if (users == null)
                return null;
            var userDto = new UserDto
            {
                UserId = users.UserId,
                FullName = users.FullName,
                Email = users.Email,
                Role = users.Role.Name
            };
            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users=  await _context.Users.Include(r => r.Role).ToListAsync();
            if (!users.Any())
                return Enumerable.Empty<UserDto>();
           var userDtos= users.Select(u=> new UserDto
           {
                UserId= u.UserId,
                FullName= u.FullName,
                Email= u.Email,
                Role= u.Role.Name
           });
            return userDtos;
        }

        
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
