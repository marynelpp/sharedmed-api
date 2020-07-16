using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiSharedMed.Models;
using ContextBD.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using ApiSharedMed.Services;

namespace ApiSharedMed.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : Controller
    {
        private IConfiguration _configuration;
        private devsharedmedContext _db;

        protected IConfiguration Configuration => _configuration;
        protected ContextBD.Models.devsharedmedContext Db => _db;
        public AuthController(devsharedmedContext eFContext, IConfiguration configuration)
        {
            _db = eFContext;
            _configuration = configuration;
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost("auth/login")]
        public IActionResult Login([FromBody] LoginCredentials data)
        {
            try
            {
                var prevUser = Db.Users.FirstOrDefault(t => t.Email == data.Username);

                if (prevUser == null)
                    return StatusCode(StatusCodes.Status401Unauthorized, "Usuario no existe. Verifique!");

                var saltCode = Convert.FromBase64String(prevUser.SaltCode);
                var formattedPasword = HashServices.HashPassword(data.Password, saltCode);

                if (formattedPasword != prevUser.Password)
                    return StatusCode(StatusCodes.Status401Unauthorized, "Clave incorrecta!");

                int? idSub = 0;

                if (prevUser.IdEspMad == 2)
                {
                    idSub = prevUser.IdSubEspMed;
                }
                if (prevUser.IdEspMad == 3)
                {
                    idSub = prevUser.IdSubEspQx;
                }
                if (prevUser.IdEspMad == 4)
                {
                    idSub = prevUser.IdSubEspGobs;
                }
                if (prevUser.IdEspMad == 5)
                {
                    idSub = prevUser.IdSubEspPed;
                }
                if (prevUser.IdEspMad == 7)
                {
                    idSub = prevUser.IdSubEspAnMedCr;
                }
                if (prevUser.IdEspMad == 8)
                {
                    idSub = prevUser.IdSubEspRad;
                }
                if (prevUser.IdEspMad == 9)
                {
                    idSub = prevUser.IdSubEspOdo;
                }
                if (prevUser.IdEspMad == 10)
                {
                    idSub = prevUser.IdSubEspTec;
                }

                var datos = new LoginResult()
                {
                    status = "Autorizado",
                    emailUser = prevUser.Email,
                    idUser = prevUser.IdUser,
                    nameUser = prevUser.Nombres + " " + prevUser.PApellido,
                    nameUserDr = prevUser.Sexo == "Femenino" ? "Dra. "+prevUser.Nombres+" "+prevUser.PApellido+"": "Dr. "+prevUser.Nombres+" "+prevUser.PApellido+"",
                    idEspMad = prevUser.IdEspMad,
                    idSubEsp = idSub,
                    idPais = prevUser.IdPais,
                    disponible = prevUser.Disponible

                };
                return Ok(datos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


      
    }
}
