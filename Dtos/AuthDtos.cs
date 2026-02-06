using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Dtos;

public record RegisterDto([Required] string Name, [Required][EmailAddress] string Email, [Required] string Password);
public record LoginDto([Required][EmailAddress] string Email, [Required] string Password);
public record AuthResponseDto(string Token, int UserId, string Name, string Email);
