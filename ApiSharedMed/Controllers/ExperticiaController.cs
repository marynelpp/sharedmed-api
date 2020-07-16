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
    public class ExperticiaController : Controller
    {
        private IConfiguration _configuration;
        private devsharedmedContext _db;

        protected IConfiguration Configuration => _configuration;
        protected ContextBD.Models.devsharedmedContext Db => _db;
        public ExperticiaController(devsharedmedContext eFContext, IConfiguration configuration)
        {
            _db = eFContext;
            _configuration = configuration;
        }




        [HttpGet("experticia/{iduser}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult GetExperticiaUser(int iduser)
        {
            try
            {
                var List = new List<ExperticiaUsers>();
                var query = Db.ExperticiaUser.Where(t => t.IdUser == iduser).ToList();

                foreach (var item in query)
                {
                    var findExp = Db.ExperticiasEspecialidades.FirstOrDefault(t => t.IdExp == item.IdExp);
                    var datos = new ExperticiaUsers()
                    {
                        Exp = findExp.Descripcion,
                        Result = item.Result

                    };
                    List.Add(datos);
                }


                return Ok(List);
            }
            catch (Exception ex)
            {
                // Logger.LogError(ex, "AuthController Register", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost("experticia/{iduser}")]
        public IActionResult PostExperticia([FromBody] ExperticiaUsersPost data, int iduser)
        {
            try
            {
                foreach (var item in data.data)
                {

                    var idExp = Db.ExperticiasEspecialidades.FirstOrDefault(t => t.Descripcion == item.Exp);
                    var exis = Db.ExperticiaUser.FirstOrDefault(t => t.IdUser == iduser && t.IdExp == idExp.IdExp);


                    if (exis != null)
                    {
                        var query = Db.ExperticiaUser.Where(t => t.IdExpUser == exis.IdExpUser).ToList();
                        foreach (var item2 in query)
                        {
                            item2.IdExp = idExp.IdExp;
                            item2.Result = item.Result;
                            Db.ExperticiaUser.Update(item2);
                        }
                        Db.SaveChanges(true);


                    }
                    else
                    {


                        ExperticiaUser datos = new ExperticiaUser()
                        {

                            IdExp = idExp.IdExp,
                            IdUser = iduser,
                            Result = item.Result


                        };
                        Db.ExperticiaUser.Add(datos);
                        Db.SaveChanges(true);


                    }

                }

                foreach(var itemBanco in data.dataBanco)
                {
                    var exis = Db.UserBanco.FirstOrDefault(t => t.IdUser == iduser);
                    if (exis != null)
                    {
                        var query = Db.UserBanco.Where(t => t.IdUserBanco == exis.IdUserBanco).ToList();
                        foreach (var item2 in query)
                        {
                            item2.IdBanco = itemBanco.IdBanco;
                            item2.NroCuenta = itemBanco.NroCuenta;
                            Db.UserBanco.Update(item2);
                        }
                        Db.SaveChanges(true);
                    }
                    else
                    {
                        var user = Db.Users.Where(t => t.IdUser == iduser).ToList();
                        var userFind = Db.Users.FirstOrDefault(t => t.IdUser == iduser);
                        var tpouser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == userFind.IdTipoUser);
                        if(tpouser.IdTipoUser == 1)
                        {
                            UserBanco datos = new UserBanco()
                            {
                                IdBanco = itemBanco.IdBanco,
                                IdPrecio = 1,
                                IdUser = iduser,
                                NroCuenta = itemBanco.NroCuenta,
                            };
                            Db.UserBanco.Add(datos);
                            Db.SaveChanges(true);
                            //bandera helper

                            foreach (var itemUser in user)
                            {
                                itemUser.Helper = 1;
                                Db.Users.Update(itemUser);
                            }
                            Db.SaveChanges(true);
                        }
                        if (tpouser.IdTipoUser == 2)
                        {
                            UserBanco datos = new UserBanco()
                            {
                                IdBanco = itemBanco.IdBanco,
                                IdPrecio = 2,
                                IdUser = iduser,
                                NroCuenta = itemBanco.NroCuenta,
                            };
                            Db.UserBanco.Add(datos);
                            Db.SaveChanges(true);
                            //bandera helper

                            foreach (var itemUser in user)
                            {
                                itemUser.Helper = 1;
                                Db.Users.Update(itemUser);
                            }
                            Db.SaveChanges(true);
                        }
                        if (tpouser.IdTipoUser == 3)
                        {
                            UserBanco datos = new UserBanco()
                            {
                                IdBanco = itemBanco.IdBanco,
                                IdPrecio = 3,
                                IdUser = iduser,
                                NroCuenta = itemBanco.NroCuenta,
                            };
                            Db.UserBanco.Add(datos);
                            Db.SaveChanges(true);
                            //bandera helper

                            foreach (var itemUser in user)
                            {
                                itemUser.Helper = 1;
                                Db.Users.Update(itemUser);
                            }
                            Db.SaveChanges(true);
                        }


                    }
                }

                return Ok("");
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "AuthController Login", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }



        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpDelete("experticia/{id}")]
        public IActionResult DeleteExperticia(int id)
        {
            try
            {
                var query = Db.ExperticiaUser.FirstOrDefault(t => t.IdExpUser == id);
                Db.Remove(query);
                Db.SaveChanges();
                return Ok("Eliminado Exitoso");


            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "AuthController Login", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("experticia/datosbancarios")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult GetBancosExp()
        {
            try
            {
                var query = from a in Db.UserBanco
                            select new UsersBancos
                            {
                               IdUserBanco = a.IdUserBanco,
                               IdBanco = a.IdBanco,
                               IdPrecio = a.IdPrecio,
                               IdUser = a.IdUser,
                               NroCuenta = a.NroCuenta
                               

                            };

                var datos = query.ToList();

                return Ok(datos);
            }
            catch (Exception ex)
            {
                // Logger.LogError(ex, "AuthController Register", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


        [HttpGet("experticia/datosbancarios/user/{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult GetBancosExpUser(int id)
        {
            try
            {
                var query = from a in Db.UserBanco
                            join s in Db.Bancos on a.IdBanco equals s.IdBanco
                            join q in Db.PrecioHoraAtencion on a.IdPrecio equals q.IdPrecio
                            select new UsersBancosGet
                            {
                                IdUserBanco = a.IdUserBanco,
                                Banco = s.Descripcion,
                                IdBanco = a.IdBanco,
                                IdPrecio = a.IdPrecio,
                                Precio = q.Valor,
                                NroCuenta = a.NroCuenta,
                                IdUser = a.IdUser


                            };

                var datos = query.Where(t=>t.IdUser == id).ToList();

                return Ok(datos);
            }
            catch (Exception ex)
            {
                // Logger.LogError(ex, "AuthController Register", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }




        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPut("experticia/datosbancarios/{id}")]
        public IActionResult Putdatban([FromBody] UsersBancosData data, int id)
        {
            try
            {
                var query = Db.UserBanco.Where(t => t.IdUserBanco == id).ToList();

                foreach (var item in query)
                {
                    item.IdBanco = data.IdBanco;
                    item.IdUser = data.IdUser;
                    item.NroCuenta = data.NroCuenta;
                }
                Db.SaveChanges(true);
                return Ok("Edición Exitosa");

            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "AuthController Login", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpDelete("experticia/datosbancarios/{id}")]
        public IActionResult Deletedatban(int id)
        {
            try
            {
                var query = Db.UserBanco.FirstOrDefault(t => t.IdUserBanco == id);
                Db.Remove(query);
                Db.SaveChanges();
                return Ok("Eliminado Exitoso");


            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "AuthController Login", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
