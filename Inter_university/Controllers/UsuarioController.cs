using Inter.DAL.Dto.User;
using Inter.DAL.Enums;
using Inter.DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inter_university.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;

        public UsuarioController(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _rolManager = rolManager;
            _SignInManager = signInManager;
        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultAuth>> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser NuevoUsuario = new()
            {
                UserName = model.Correo,
                Email = model.Correo,
                FullName = $"{model.Nombres} {model.Apellidos}",
                PhoneNumber = model.Telefono,
            };

            IdentityResult register = await RegistrarUsuario(NuevoUsuario, Roles.Estudiante.ToString(), model.Clave);
            if (register.Succeeded)
            {
                return await ConstruirToken(new LoginDto { Email = NuevoUsuario.Email, AuthDate = DateTime.Now });
            }
            else
            {
                return BadRequest(register.Errors);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultAuth>> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Resultado = await _SignInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (Resultado.Succeeded)
            {
                return Ok(await ConstruirToken(model));
            }
            else
            {
                return Unauthorized(new { Code = (int)ErroresLogin.LoginIncorrecto });
            }

        }

        [HttpPost("informacionUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ResponseUser>> InformacionUsuario()
        {
            var Indentity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = Indentity.Claims.FirstOrDefault(c => c.Type == "ConnectionId")?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            ApplicationUser usuario = await _userManager.FindByIdAsync(userId);
            IList<string> roles = await _userManager.GetRolesAsync(usuario);
            IdentityRole rol = await _rolManager.FindByNameAsync(roles.FirstOrDefault());

            return Ok(new ResponseUser
            {
                User_Id = usuario.Id,
                Nombres = usuario.FullName,
                Correo = usuario.Email,
                Telefono = usuario.PhoneNumber,
                RolId = rol?.Id,
                NombreRol = rol?.Name,
            });
        }

        private async Task<ResultAuth> ConstruirToken(LoginDto Usuario)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(Usuario.Email);

            var Claims = new List<Claim>
            {
                new ("ConnectionId", user.Id),
                new ("Time", Usuario.AuthDate.ToString("yyyy-MM-dd HH:mm:ss")),
            };

            SymmetricSecurityKey llave = new(Encoding.UTF8.GetBytes(_configuration["llavejwt"]));
            SigningCredentials Credenciales = new(llave, SecurityAlgorithms.HmacSha256);
            DateTime Expiracion = DateTime.Now.AddDays(1);

            var SecurityToken = new JwtSecurityToken(issuer: null, audience: null, claims: Claims, expires: Expiracion, signingCredentials: Credenciales);

            return new ResultAuth
            {
                Token = new JwtSecurityTokenHandler().WriteToken(SecurityToken),
                Expiracion = Expiracion,
            };
        }

        private async Task<IdentityResult> RegistrarUsuario(ApplicationUser NuevoUsuario, string Rol, string Clave)
        {
            IdentityResult Result = await _userManager.CreateAsync(NuevoUsuario, Clave);

            if (Result.Succeeded)
            {
                ApplicationUser usuario = await _userManager.FindByEmailAsync(NuevoUsuario.Email);

                IdentityResult asignacion = await _userManager.AddToRoleAsync(usuario, Rol);

                if (!asignacion.Succeeded)
                {
                    Result = asignacion;
                }
            }

            return Result;
        }
    }
}
