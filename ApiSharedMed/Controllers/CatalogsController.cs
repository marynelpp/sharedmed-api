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
using BancosModels = ApiSharedMed.Models.BancosModels;

namespace ApiSharedMed.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CatalogsController : Controller
    {
        private IConfiguration _configuration;
        private devsharedmedContext _db;

        protected IConfiguration Configuration => _configuration;
        protected ContextBD.Models.devsharedmedContext Db => _db;
        public CatalogsController(devsharedmedContext eFContext, IConfiguration configuration)
        {
            _db = eFContext;
            _configuration = configuration;
        }

        [HttpGet("tipouser")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult TipoUser()
        {
            try
            {
                var query = from a in Db.TipoUser
                            select new TipoUsers
                            {
                                IdTipoUser = a.IdTipoUser,
                                Descripcion = a.Descripcion,
                                DescripcionInt = a.DescripcionInterna
                                

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

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost("tipouser")]
        public IActionResult PostTipoUser([FromBody] TipoUsersData data)
        {
            try
            {
                TipoUser datos = new TipoUser()
                {

                    Descripcion = data.Descripcion,
                    DescripcionInterna = data.DescripcionInt
                };
                Db.TipoUser.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("tipouser/{id}")]
        public IActionResult PutTipoUser([FromBody] TipoUsersData data, int id)
        {
            try
            {
                var query = Db.TipoUser.Where(t => t.IdTipoUser == id).ToList();

                foreach (var item in query)
                {
                    item.Descripcion = data.Descripcion;
                    item.DescripcionInterna = data.DescripcionInt;
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
        [HttpDelete("tipouser/{id}")]
        public IActionResult DeleteTipoUsero(int id)
        {
            try
            {
                var query = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == id);
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

        [HttpGet("clinicas")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Clinicas()
        {
            try
            {
                var query = from a in Db.Clinicas
                            select new ClinicasModel
                            {
                                IdClinica = a.IdClinica,
                                IdPais = a.IdPais,
                                Descripcion = a.Descripcion,


                            };

                var datos = query.ToList().OrderBy(t => t.Descripcion);

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
        [HttpPost("clinicas")]
        public IActionResult PostClinicas([FromBody] ClinicasModelData data)
        {
            try
            {
                Clinicas datos = new Clinicas()
                {

                    IdPais = data.IdPais,
                    Descripcion = data.Descripcion,
                };
                Db.Clinicas.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("clinicas/{id}")]
        public IActionResult PutClinicas([FromBody] ClinicasModelData data, int id)
        {
            try
            {
                var query = Db.Clinicas.Where(t => t.IdClinica == id).ToList();

                foreach (var item in query)
                {
                    item.IdPais = data.IdPais;
                    item.Descripcion = data.Descripcion;
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
        [HttpDelete("clinicas/{id}")]
        public IActionResult DeleteClinicas(int id)
        {
            try
            {
                var query = Db.Clinicas.FirstOrDefault(t => t.IdClinica == id);
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

        [HttpGet("especialidadmadre")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult EspecialidadMadre()
        {
            try
            {
                var query = from a in Db.EspecialidadMadre
                            select new EspMadre
                            {
                                IdEspMad = a.IdEspMad,
                                Descripcion = a.Descripcion


                            };

                var datos = query.ToList().OrderBy(t=>t.Descripcion);
                return Ok(datos);
            }
            catch (Exception ex)
            {
                // Logger.LogError(ex, "AuthController Register", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        [HttpGet("especialidadmadre/services")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult EspecialidadMadreServices()
        {
            try
            {
                var query = from a in Db.EspecialidadMadre
                            where !(a.IdEspMad == 11)
                            select new EspMadre
                            {
                                IdEspMad = a.IdEspMad,
                                Descripcion = a.Descripcion


                            };

                var datos = query.ToList().OrderBy(t => t.Descripcion);
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
        [HttpPost("especialidadmadre")]
        public IActionResult PostEspecialidadMadre([FromBody] EspMadreData data)
        {
            try
            {
                EspecialidadMadre datos = new EspecialidadMadre()
                {

                    Descripcion = data.Descripcion,
                };
                Db.EspecialidadMadre.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("especialidadmadre/{id}")]
        public IActionResult PutEspecialidadMadre([FromBody] EspMadreData data, int id)
        {
            try
            {
                var query = Db.EspecialidadMadre.Where(t => t.IdEspMad == id).ToList();

                foreach (var item in query)
                {
                    item.Descripcion = data.Descripcion;
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
        [HttpDelete("especialidadmadre/{id}")]
        public IActionResult DeleteEspecialidadMadre(int id)
        {
            try
            {
                var query = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == id);
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

        [HttpGet("subespqx")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult SubEspQx()
        {
            try
            {
                var query = from a in Db.SubEspQx
                            select new SespecialidadQx
                            {
                                IdSubEspQx = a.IdSubEspQx,
                                Descripcion = a.Descripcion


                            };

                var datos = query.ToList().OrderBy(t=>t.Descripcion);
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
        [HttpPost("subespqx")]
        public IActionResult PostSubEspQx([FromBody] SespecialidadQxData data)
        {
            try
            {
                SubEspQx datos = new SubEspQx()
                {

                    Descripcion = data.Descripcion,
                };
                Db.SubEspQx.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("subespqx/{id}")]
        public IActionResult PutSubEspQx([FromBody] SespecialidadQxData data, int id)
        {
            try
            {
                var query = Db.SubEspQx.Where(t => t.IdSubEspQx == id).ToList();

                foreach (var item in query)
                {
                    item.Descripcion = data.Descripcion;
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
        [HttpDelete("subespqx/{id}")]
        public IActionResult DeleteSubEspQx(int id)
        {
            try
            {
                var query = Db.SubEspQx.FirstOrDefault(t => t.IdSubEspQx == id);
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


        [HttpGet("subespodo")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult SubEspOdo()
        {
            try
            {
                var query = from a in Db.SubEspOdo
                            select new SespecialidadOdo
                            {
                                IdSubEspOdo = a.IdSubEspOdo,
                                Descripcion = a.Descrpcion


                            };

                var datos = query.ToList().OrderBy(t => t.Descripcion);
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
        [HttpPost("subespodo")]
        public IActionResult PostSubEspOdo([FromBody] SespecialidadOdoData data)
        {
            try
            {
                SubEspOdo datos = new SubEspOdo()
                {

                    Descrpcion = data.Descripcion,
                };
                Db.SubEspOdo.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("subespodo/{id}")]
        public IActionResult PutSubEspOdo([FromBody] SespecialidadOdoData data, int id)
        {
            try
            {
                var query = Db.SubEspOdo.Where(t => t.IdSubEspOdo == id).ToList();

                foreach (var item in query)
                {
                    item.Descrpcion = data.Descripcion;
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
        [HttpDelete("subespodo/{id}")]
        public IActionResult DeleteSubEspOdo(int id)
        {
            try
            {
                var query = Db.SubEspOdo.FirstOrDefault(t => t.IdSubEspOdo == id);
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

        [HttpGet("subesptec")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult SubEsptec()
        {
            try
            {
                var query = from a in Db.SubEspTec
                            select new SespecialidadTec
                            {
                                IdSubEspTec = a.IdSubEspTec,
                                Descripcion = a.Descripcion


                            };

                var datos = query.ToList().OrderBy(t => t.Descripcion);
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
        [HttpPost("subesptec")]
        public IActionResult PostSubEspTec([FromBody] SespecialidadTecData data)
        {
            try
            {
                SubEspTec datos = new SubEspTec()
                {

                    Descripcion = data.Descripcion,
                };
                Db.SubEspTec.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("subesptec/{id}")]
        public IActionResult PutSubEspTec([FromBody] SespecialidadTecData data, int id)
        {
            try
            {
                var query = Db.SubEspTec.Where(t => t.IdSubEspTec == id).ToList();

                foreach (var item in query)
                {
                    item.Descripcion = data.Descripcion;
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
        [HttpDelete("subesptec/{id}")]
        public IActionResult DeleteSubEspTec(int id)
        {
            try
            {
                var query = Db.SubEspTec.FirstOrDefault(t => t.IdSubEspTec == id);
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

        [HttpGet("subespmed")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult SubEspMed()
        {
            try
            {
                var query = from a in Db.SubEspMed
                            select new SespecialidadMed
                            {
                                IdSubEspMed = a.IdSubEspMed,
                                Descripcion = a.Descripcion


                            };

                var datos = query.ToList().OrderBy(t => t.Descripcion);
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
        [HttpPost("subespmed")]
        public IActionResult PostSubEspMed([FromBody] SespecialidadMedData data)
        {
            try
            {
                SubEspMed datos = new SubEspMed()
                {

                    Descripcion = data.Descripcion,
                };
                Db.SubEspMed.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("subespmed/{id}")]
        public IActionResult PutSubEspMed([FromBody] SespecialidadMedData data, int id)
        {
            try
            {
                var query = Db.SubEspMed.Where(t => t.IdSubEspMed == id).ToList();

                foreach (var item in query)
                {
                    item.Descripcion = data.Descripcion;
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
        [HttpDelete("subespmed/{id}")]
        public IActionResult DeleteSubEspMed(int id)
        {
            try
            {
                var query = Db.SubEspMed.FirstOrDefault(t => t.IdSubEspMed == id);
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

        [HttpGet("subespped")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult SubEspPed()
        {
            try
            {
                var query = from a in Db.SubEspPed
                            select new SespecialidadPed
                            {
                                IdSubEspPed = a.IdSubEspPed,
                                Descripcion = a.Descripcion


                            };

                var datos = query.ToList().OrderBy(t => t.Descripcion);
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
        [HttpPost("subespped")]
        public IActionResult PostSubEspPed([FromBody] SespecialidadPedData data)
        {
            try
            {
                SubEspPed datos = new SubEspPed()
                {

                    Descripcion = data.Descripcion,
                };
                Db.SubEspPed.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("subespped/{id}")]
        public IActionResult PutSubEspPed([FromBody] SespecialidadPedData data, int id)
        {
            try
            {
                var query = Db.SubEspPed.Where(t => t.IdSubEspPed == id).ToList();

                foreach (var item in query)
                {
                    item.Descripcion = data.Descripcion;
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
        [HttpDelete("subespped/{id}")]
        public IActionResult DeleteSubEspPed(int id)
        {
            try
            {
                var query = Db.SubEspPed.FirstOrDefault(t => t.IdSubEspPed == id);
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


        [HttpGet("subespgobs")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult SubEspGObs()
        {
            try
            {
                var query = from a in Db.SubEspGobs
                            select new SespecialidadGObs
                            {
                                IdSubEspGobs = a.IdSubEspGobs,
                                Descripcion = a.Descripcion


                            };

                var datos = query.ToList().OrderBy(t => t.Descripcion);
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
        [HttpPost("subespgobs")]
        public IActionResult PostSubEspGobs([FromBody] SespecialidadGObsData data)
        {
            try
            {
                SubEspGobs datos = new SubEspGobs()
                {

                    Descripcion = data.Descripcion,
                };
                Db.SubEspGobs.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("subespgobs/{id}")]
        public IActionResult PutSubEspGobs([FromBody] SespecialidadGObsData data, int id)
        {
            try
            {
                var query = Db.SubEspGobs.Where(t => t.IdSubEspGobs == id).ToList();

                foreach (var item in query)
                {
                    item.Descripcion = data.Descripcion;
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
        [HttpDelete("subespgobs/{id}")]
        public IActionResult DeleteSubEspGobs(int id)
        {
            try
            {
                var query = Db.SubEspGobs.FirstOrDefault(t => t.IdSubEspGobs == id);
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

        [HttpGet("subespanemedcri")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Subsubespanemedcri()
        {
            try
            {
                var query = from a in Db.SubEspAnMedCr
                            select new SespecialidadMedCrit
                            {
                                IdSubEspAnMedCr = a.IdSubEspAnMedCr,
                                Descripcion = a.Descripcion


                            };

                var datos = query.ToList().OrderBy(t => t.Descripcion);
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
        [HttpPost("subespanemedcri")]
        public IActionResult Postsubespanemedcri([FromBody] SespecialidadMedCritData data)
        {
            try
            {
                SubEspAnMedCr datos = new SubEspAnMedCr()
                {

                    Descripcion = data.Descripcion,
                };
                Db.SubEspAnMedCr.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("subespanemedcri/{id}")]
        public IActionResult Putsubespanemedcri([FromBody] SespecialidadMedCritData data, int id)
        {
            try
            {
                var query = Db.SubEspAnMedCr.Where(t => t.IdSubEspAnMedCr == id).ToList();

                foreach (var item in query)
                {
                    item.Descripcion = data.Descripcion;
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
        [HttpDelete("subespanemedcri/{id}")]
        public IActionResult Deletesubespanemedcri(int id)
        {
            try
            {
                var query = Db.SubEspAnMedCr.FirstOrDefault(t => t.IdSubEspAnMedCr == id);
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
        [HttpGet("subesprad")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult subesprad()
        {
            try
            {
                var query = from a in Db.SubEspRad
                            select new SespecialidadRad
                            {
                                IdSubEspRad = a.IdSubEspRad,
                                Descripcion = a.Descripcion


                            };

                var datos = query.ToList().OrderBy(t => t.Descripcion);
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
        [HttpPost("subesprad")]
        public IActionResult Postsubesprad([FromBody] SespecialidadRadData data)
        {
            try
            {
                SubEspRad datos = new SubEspRad()
                {

                    Descripcion = data.Descripcion,
                };
                Db.SubEspRad.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("subesprad/{id}")]
        public IActionResult Putsubesprad([FromBody] SespecialidadRadData data, int id)
        {
            try
            {
                var query = Db.SubEspRad.Where(t => t.IdSubEspRad == id).ToList();

                foreach (var item in query)
                {
                    item.Descripcion = data.Descripcion;
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
        [HttpDelete("subesprad/{id}")]
        public IActionResult Deletesubesprad(int id)
        {
            try
            {
                var query = Db.SubEspRad.FirstOrDefault(t => t.IdSubEspRad == id);
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

        [HttpGet("paises")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Paises()
        {
            try
            {
                var query = from a in Db.Pais
                            select new Paises
                            {
                                IdPais = a.IdPais,
                                Nombre = a.Nombre
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

        [HttpGet("nacionalidad")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Nacionalidad()
        {
            try
            {
                var query = from a in Db.Nacionalidad
                            select new NacionalidadR
                            {
                                IdNacionalidad = a.IdNacionalidad,
                                GentilicioNac = a.GentilicioNac
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

        [HttpGet("region")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Region()
        {
            try
            {
                var query = from a in Db.Region
                            select new Regiones
                            {
                                IdRegion = a.IdRegion,
                                Descripcion = a.Descripcion
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

        [HttpGet("precioatencion")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult PrecioAtencionGet()
        {
            try
            {
                var query = from a in Db.PrecioHoraAtencion
                            select new Precios
                            {
                                IdPrecio = a.IdPrecio,
                                Valor = a.Valor


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

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost("precioatencion")]
        public IActionResult Postprecioatencion([FromBody] PreciosData data)
        {
            try
            {
                PrecioHoraAtencion datos = new PrecioHoraAtencion()
                {

                    Valor = data.Valor,
                };
                Db.PrecioHoraAtencion.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("precioatencion/{id}")]
        public IActionResult Putprecioatencion([FromBody] PreciosData data, int id)
        {
            try
            {
                var query = Db.PrecioHoraAtencion.Where(t => t.IdPrecio == id).ToList();

                foreach (var item in query)
                {
                    item.Valor = data.Valor;
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
        [HttpDelete("precioatencion/{id}")]
        public IActionResult Deleteprecioatencion(int id)
        {
            try
            {
                var query = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == id);
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

        [HttpGet("bancos")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult GetBancos()
        {
            try
            {
                var query = from a in Db.Bancos
                            select new BancosModels
                            {
                                IdBanco = a.IdBanco,
                                Descripcion = a.Descripcion


                            };

                var datos = query.ToList().OrderBy(t => t.Descripcion);

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
        [HttpPost("bancos")]
        public IActionResult PostBancos([FromBody] BancosData data)
        {
            try
            {
                Bancos datos = new Bancos()
                {

                    Descripcion = data.Descripcion,
                };
                Db.Bancos.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("bancos/{id}")]
        public IActionResult PutBancos([FromBody] BancosData data, int id)
        {
            try
            {
                var query = Db.Bancos.Where(t => t.IdBanco == id).ToList();

                foreach (var item in query)
                {
                    item.Descripcion = data.Descripcion;
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
        [HttpDelete("bancos/{id}")]
        public IActionResult DeleteBancos(int id)
        {
            try
            {
                var query = Db.Bancos.FirstOrDefault(t => t.IdBanco == id);
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


        [HttpGet("experticias/especialidades")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult GetExperticiaEsp()
        {
            try
            {
                var query = from a in Db.ExperticiasEspecialidades
                            select new ExperticiaEspModels
                            {
                                IdExp = a.IdExp,
                                IdEspMad = a.IdEspMad,
                                IdSubEspGobs = a.IdSubEspGobs,
                                IdSubEspMed = a.IdSubEspMed,
                                IdSubEspPed = a.IdSubEspPed,
                                IdSubEspQx = a.IdSubEspQx,
                                IdSubEspAnMedCr = a.IdSubEspAnMedCr,
                                IdSubEspRad = a.IdSubEspRad,
                                IdSubEspOdo = a.IdSubEspOdo,
                                IdSubEspTec = a.IdSubEspTec,
                                Descripcion = a.Descripcion


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

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost("experticias/especialidades")]
        public IActionResult PostExpEspecialidades([FromBody] ExperticiaEspModelsData data)
        {
            try
            {
                ExperticiasEspecialidades datos = new ExperticiasEspecialidades()
                {

                    IdEspMad = data.IdEspMad,
                    IdSubEspGobs = data.IdSubEspGobs == 0 ? null : data.IdSubEspGobs,
                    IdSubEspMed = data.IdSubEspMed == 0 ? null : data.IdSubEspMed,
                    IdSubEspPed = data.IdSubEspPed == 0 ? null : data.IdSubEspPed,
                    IdSubEspQx = data.IdSubEspQx == 0 ? null : data.IdSubEspQx,
                    IdSubEspAnMedCr = data.IdSubEspAnMedCr == 0 ? null : data.IdSubEspAnMedCr,
                    IdSubEspRad = data.IdSubEspRad == 0 ? null : data.IdSubEspRad,
                    IdSubEspOdo = data.IdSubEspOdo == 0 ? null : data.IdSubEspOdo,
                    IdSubEspTec = data.IdSubEspTec == 0 ? null : data.IdSubEspTec,
                    Descripcion = data.Descripcion
                };
                Db.ExperticiasEspecialidades.Add(datos);
                Db.SaveChanges(true);

                return Ok("Guardado Exitoso");
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
        [HttpPut("experticias/especialidades/{id}")]
        public IActionResult PutExpEsp([FromBody] ExperticiaEspModelsData data, int id)
        {
            try
            {
                var query = Db.ExperticiasEspecialidades.Where(t => t.IdExp == id).ToList();

                foreach (var item in query)
                {
                    item.IdEspMad = data.IdEspMad;
                    item.IdSubEspGobs = data.IdSubEspGobs;
                    item.IdSubEspMed = data.IdSubEspMed;
                    item.IdSubEspPed = data.IdSubEspPed;
                    item.IdSubEspQx = data.IdSubEspQx;
                    item.IdSubEspAnMedCr = data.IdSubEspAnMedCr;
                    item.IdSubEspRad = data.IdSubEspRad;
                    item.IdSubEspOdo = data.IdSubEspOdo;
                    item.IdSubEspTec = data.IdSubEspTec;
                    item.Descripcion = data.Descripcion;
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
        [HttpPut("disponible/helper/{iduser}/{dispo}")]
        public IActionResult PutDisponibilidadHelper(int iduser, int dispo)
        {
            try
            {
                var query = Db.Users.Where(t => t.IdUser == iduser).ToList();

                foreach (var item in query)
                {
                    item.Disponible = dispo == 1 ? 1 : 0;
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
        [HttpDelete("experticias/especialidades/{id}")]
        public IActionResult DeleteExpEsp(int id)
        {
            try
            {
                var query = Db.ExperticiasEspecialidades.FirstOrDefault(t => t.IdExp == id);
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

        [HttpGet("experticias/especialidades/{idmadre}/{idsub}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult GetExperticia(int idmadre, int idsub)
        {
            try
            {
                var List = new List<ExperticiaEspModelsGet>();
                if (idmadre == 2)
                {
                    var query = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == idmadre && t.IdSubEspMed == idsub).OrderBy(t=>t.Descripcion);
                    foreach (var item in query)
                    {

                        var datos = new ExperticiaEspModelsGet()
                        {
                            IdExp = item.IdExp,
                            Descripcion = item.Descripcion
                        };
                        List.Add(datos);
                    }
                    return Ok(List);
                }
                if (idmadre == 3)
                {
                    var query = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == idmadre && t.IdSubEspQx == idsub).OrderBy(t => t.Descripcion);
                    foreach (var item in query)
                    {

                        var datos = new ExperticiaEspModelsGet()
                        {
                            IdExp = item.IdExp,
                            Descripcion = item.Descripcion
                        };
                        List.Add(datos);
                    }
                    return Ok(List);
                }
                if (idmadre == 4)
                {
                    var query = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == idmadre && t.IdSubEspGobs == idsub).OrderBy(t => t.Descripcion);
                    foreach (var item in query)
                    {

                        var datos = new ExperticiaEspModelsGet()
                        {
                            IdExp = item.IdExp,
                            Descripcion = item.Descripcion
                        };
                        List.Add(datos);
                    }
                    return Ok(List);
                }
                if (idmadre == 5)
                {
                    var query = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == idmadre && t.IdSubEspPed == idsub).OrderBy(t => t.Descripcion);
                    foreach (var item in query)
                    {

                        var datos = new ExperticiaEspModelsGet()
                        {
                            IdExp = item.IdExp,
                            Descripcion = item.Descripcion
                        };
                        List.Add(datos);
                    }
                    return Ok(List);
                }
                if (idmadre == 7)
                {
                    var query = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == idmadre && t.IdSubEspAnMedCr == idsub).OrderBy(t => t.Descripcion);
                    foreach (var item in query)
                    {

                        var datos = new ExperticiaEspModelsGet()
                        {
                            IdExp = item.IdExp,
                            Descripcion = item.Descripcion
                        };
                        List.Add(datos);
                    }
                    return Ok(List);
                }
                if (idmadre == 8)
                {
                    var query = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == idmadre && t.IdSubEspRad == idsub).OrderBy(t => t.Descripcion);
                    foreach (var item in query)
                    {

                        var datos = new ExperticiaEspModelsGet()
                        {
                            IdExp = item.IdExp,
                            Descripcion = item.Descripcion
                        };
                        List.Add(datos);
                    }
                    return Ok(List);
                }
                if (idmadre == 9)
                {
                    var query = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == idmadre && t.IdSubEspOdo == idsub).OrderBy(t => t.Descripcion);
                    foreach (var item in query)
                    {

                        var datos = new ExperticiaEspModelsGet()
                        {
                            IdExp = item.IdExp,
                            Descripcion = item.Descripcion
                        };
                        List.Add(datos);
                    }
                    return Ok(List);
                }
                if (idmadre == 10)
                {
                    var query = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == idmadre && t.IdSubEspTec == idsub).OrderBy(t => t.Descripcion);
                    foreach (var item in query)
                    {

                        var datos = new ExperticiaEspModelsGet()
                        {
                            IdExp = item.IdExp,
                            Descripcion = item.Descripcion
                        };
                        List.Add(datos);
                    }
                    return Ok(List);
                }


                return Ok("");
            }
            catch (Exception ex)
            {
                // Logger.LogError(ex, "AuthController Register", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}
