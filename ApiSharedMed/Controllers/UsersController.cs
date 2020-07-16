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
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Globalization;
using System.Net;

namespace ApiSharedMed.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UsersController : Controller
    {
        private IConfiguration _configuration;
        private devsharedmedContext _db;

        protected IConfiguration Configuration => _configuration;
        protected ContextBD.Models.devsharedmedContext Db => _db;
        public UsersController(devsharedmedContext eFContext, IConfiguration configuration)
        {
            _db = eFContext;
            _configuration = configuration;
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpPost("users")]
        public IActionResult PostUsers([FromBody] UsersData data)
        {
            try
            {


                if (data.Password != data.ConfPassword)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Las contraseñas no coinciden, Verifique! ");
                }

                var nac = Db.Nacionalidad.FirstOrDefault(t => t.IdNacionalidad == data.IdNacionalidad);
                var region = Db.Region.FirstOrDefault(t => t.IdRegion == data.IdRegion);
                var pais = Db.Pais.FirstOrDefault(t => t.IdPais == data.IdPais);
                var tuser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == data.IdTipoUser);
                var espmad = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == data.IdEspecialidadMadre);
                var subqx = Db.SubEspQx.FirstOrDefault(t => t.IdSubEspQx ==  data.IdSubEspQx);
                var submed = Db.SubEspMed.FirstOrDefault(t => t.IdSubEspMed == data.IdSubEspMed);
                var subped = Db.SubEspPed.FirstOrDefault(t => t.IdSubEspPed ==  data.IdSubEspPed);
                var subgobs = Db.SubEspGobs.FirstOrDefault(t => t.IdSubEspGobs == data.IdSubEspGobs);
                var submedcri = Db.SubEspAnMedCr.FirstOrDefault(t => t.IdSubEspAnMedCr == data.IdSubEspAnMedCr);
                var subrad = Db.SubEspRad.FirstOrDefault(t => t.IdSubEspRad == data.IdSubEspRad);
                var submedodo = Db.SubEspOdo.FirstOrDefault(t => t.IdSubEspOdo == data.IdSubEspOdo);
                var subtec = Db.SubEspTec.FirstOrDefault(t => t.IdSubEspTec == data.IdSubEspTec);
                var hosp = Db.Clinicas.FirstOrDefault(t => t.IdClinica == data.IdClinica);
                data.FechaNac = System.DateTime.SpecifyKind(data.FechaNac, DateTimeKind.Utc);
                var utcValue = ((DateTimeOffset)data.FechaNac).ToUnixTimeMilliseconds();
                int iduser = 0;

                var prevUser = Db.Users.FirstOrDefault(t => t.RutPass == data.RutPass);

                if (prevUser != null)
                    return StatusCode(StatusCodes.Status401Unauthorized, "Usuario ya esta registrado en SharedMed");

                //si pais es chile
                if (data.IdPais == 46)
                {
                    /*
                    var response = ConexionSupIntSalud.GetApi("/prestadores/" + data.RutPass + ".json/?auth_key=" + Configuration["auth_key"] + "");



                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, "Usuario no existe en Superintendencia de Chile !");
                    }
                    */
                    //else
                   // {
                        var hapikey = Configuration["keyHubSpot"];
                        var client = new RestClient("https://api.hubapi.com/");
                        var request = new RestRequest("contacts/v1/contact", Method.POST);
                        request.AddQueryParameter("hapikey", hapikey);
                        request.RequestFormat = DataFormat.Json;

                        request.AddJsonBody(new
                        {
                            properties = new[]
                            {
                                new{property="firstname",value=data.Nombres},
                                new{property="lastname",value=data.PrimerApellido},
                                new{property="segundo_apellido",value=data.SegundoApellido},
                                new{property="fecha_de_nacimiento",value=utcValue.ToString()},
                                new{property="rut_o_pasaporte",value=utcValue.ToString()},
                                new{property="nacionalidad",value=nac.PaisNac},
                                new{property="idioma_preferido",value=data.Idioma},
                                new{property="sexo",value=data.Sexo},
                                new{property="mobilephone",value=data.NroCelular},
                                new{property="work_email",value=data.Email},
                                new{property="email",value=data.Email},
                                new{property="region",value=region.Descripcion},
                                new{property="pais",value=pais.Nombre},
                                new{property="tipo_de_usuario",value=tuser.DescripcionInterna},
                                new{property="especialidad_madre",value=espmad.Descripcion},
                                new{property="sub_especialidad_qx_",value= data.IdSubEspQx == 0 ? "" :subqx.Descripcion},
                                new{property="sub_especialidad",value= data.IdSubEspMed == 0 ? "" :submed.Descripcion},
                                new{property="sub_especialidad_ped_",value= data.IdSubEspPed == 0 ? "":subped.Descripcion},
                                new{property="sub_especialidad_gobs_",value= data.IdSubEspGobs == 0 ? "": subgobs.Descripcion},
                                new{property="anestesiologia",value= data.IdSubEspAnMedCr == 0 ? "": submedcri.Descripcion},
                                new{property="radiologia",value= data.IdSubEspRad == 0 ? "": subrad.Descripcion},
                                new{property="odontologia",value= data.IdSubEspOdo == 0 ? "": submedodo.Descrpcion},
                                new{property="tecnicos",value= data.IdSubEspTec == 0 ? "": subtec.Descripcion},
                                new{property="lugar_de_trabajo_i_",value= data.IdClinica == 0 ? "": hosp.Descripcion},
                                new{property="lugar_de_trabajo_ii_",value= data.OtroLugarTrabajo},
                                new{property="otra_especialidad",value= data.OtraEsp },

                            }
                        });

                        IRestResponse responseHubSpot = client.Execute(request);
                        dynamic dynJson = JsonConvert.DeserializeObject(responseHubSpot.Content.ToString());
                        var exisContact = dynJson.message;
                        if (responseHubSpot.IsSuccessful)
                        {
                            var saltCode = HashServices.GetSaltCode();
                            Users datos = new Users()
                            {
                                Nombres = data.Nombres,
                                PApellido = data.PrimerApellido,
                                SApellido = data.SegundoApellido,
                                FechaNac = data.FechaNac,
                                RutPass = data.RutPass,
                                IdNacionalidad = data.IdNacionalidad,
                                Idioma = data.Idioma,
                                Sexo = data.Sexo,
                                NroCelular = data.NroCelular,
                                Email = data.Email,
                                Password = HashServices.HashPassword(data.Password, saltCode),
                                SaltCode = Convert.ToBase64String(saltCode),
                                IdRegion = data.IdRegion == 0 ? null : data.IdRegion,
                                IdPais = data.IdPais,
                                IdTipoUser = data.IdTipoUser,
                                IdEspMad = data.IdEspecialidadMadre,
                                IdSubEspQx = data.IdSubEspQx == 0 ? null : data.IdSubEspQx,
                                IdSubEspMed = data.IdSubEspMed == 0 ? null : data.IdSubEspMed,
                                IdSubEspPed = data.IdSubEspPed == 0 ? null : data.IdSubEspPed,
                                IdSubEspGobs = data.IdSubEspGobs == 0 ? null : data.IdSubEspGobs,
                                IdSubEspAnMedCr = data.IdSubEspAnMedCr == 0 ? null : data.IdSubEspAnMedCr,
                                IdSubEspRad = data.IdSubEspRad == 0 ? null : data.IdSubEspRad,
                                IdSubEspOdo = data.IdSubEspOdo == 0 ? null : data.IdSubEspOdo,
                                IdSubEspTec = data.IdSubEspTec == 0 ? null : data.IdSubEspTec,
                                OtroLugTrab = data.OtroLugarTrabajo,
                                IdClinica = data.IdClinica == 0 ? null : data.IdClinica,
                                OtraEsp = data.OtraEsp,
                                Helper = 0,
                                Disponible = 0
                                
                            };
                            Db.Users.Add(datos);
                            Db.SaveChanges(true);
                            iduser = datos.IdUser;

                            //experticia
                            if (data.IdEspecialidadMadre == 2)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspMed == data.IdSubEspMed).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 3)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspQx == data.IdSubEspQx).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 4)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspGobs == data.IdSubEspGobs).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 5)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspPed == data.IdSubEspPed).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }

                        if (data.IdEspecialidadMadre == 7)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspAnMedCr == data.IdSubEspAnMedCr).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        if (data.IdEspecialidadMadre == 8)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspRad == data.IdSubEspRad).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        if (data.IdEspecialidadMadre == 9)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspOdo == data.IdSubEspOdo).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        if (data.IdEspecialidadMadre == 10)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspTec == data.IdSubEspTec).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        }else if (exisContact == "Contact already exists"){
                            var idContact = dynJson.identityProfile.vid;
                            var clientExist = new RestClient("https://api.hubapi.com/");
                            var requestExist = new RestRequest("contacts/v1/contact/vid/" + idContact + "/profile", Method.POST);
                            requestExist.AddQueryParameter("hapikey", hapikey);
                            requestExist.RequestFormat = DataFormat.Json;
                            requestExist.AddJsonBody(new
                                    {
                                        properties = new[]
                                    {
                                            new{property="firstname",value=data.Nombres},
                                            new{property="lastname",value=data.PrimerApellido},
                                            new{property="segundo_apellido",value=data.SegundoApellido},
                                            new{property="fecha_de_nacimiento",value=utcValue.ToString()},
                                            new{property="rut_o_pasaporte",value=utcValue.ToString()},
                                            new{property="nacionalidad",value=nac.PaisNac},
                                            new{property="idioma_preferido",value=data.Idioma},
                                            new{property="sexo",value=data.Sexo},
                                            new{property="mobilephone",value=data.NroCelular},
                                            new{property="work_email",value=data.Email},
                                            new{property="email",value=data.Email},
                                            new{property="region",value=region.Descripcion},
                                            new{property="pais",value=pais.Nombre},
                                            new{property="tipo_de_usuario",value=tuser.DescripcionInterna},
                                            new{property="especialidad_madre",value=espmad.Descripcion},
                                            new{property="sub_especialidad_qx_",value= data.IdSubEspQx == 0 ? "" :subqx.Descripcion},
                                            new{property="sub_especialidad",value= data.IdSubEspMed == 0 ? "" :submed.Descripcion},
                                            new{property="sub_especialidad_ped_",value= data.IdSubEspPed == 0 ? "":subped.Descripcion},
                                            new{property="sub_especialidad_gobs_",value= data.IdSubEspGobs == 0 ? "": subgobs.Descripcion},
                                            new{property="anestesiologia",value= data.IdSubEspAnMedCr == 0 ? "": submedcri.Descripcion},
                                            new{property="radiologia",value= data.IdSubEspRad == 0 ? "": subrad.Descripcion},
                                            new{property="odontologia",value= data.IdSubEspOdo == 0 ? "": submedodo.Descrpcion},
                                            new{property="tecnicos",value= data.IdSubEspTec == 0 ? "": subtec.Descripcion},
                                            new{property="lugar_de_trabajo_i_",value= data.IdClinica == 0 ? "": hosp.Descripcion},
                                            new{property="lugar_de_trabajo_ii_",value= data.OtroLugarTrabajo},
                                            new{property="otra_especialidad",value= data.OtraEsp },

                                        }
                                    });

                            IRestResponse responseHubSpotExist = clientExist.Execute(requestExist);
                        if (responseHubSpotExist.IsSuccessful)
                        {
                            var saltCode = HashServices.GetSaltCode();
                            Users datos = new Users()
                            {
                                Nombres = data.Nombres,
                                PApellido = data.PrimerApellido,
                                SApellido = data.SegundoApellido,
                                FechaNac = data.FechaNac,
                                RutPass = data.RutPass,
                                IdNacionalidad = data.IdNacionalidad,
                                Idioma = data.Idioma,
                                Sexo = data.Sexo,
                                NroCelular = data.NroCelular,
                                Email = data.Email,
                                Password = HashServices.HashPassword(data.Password, saltCode),
                                SaltCode = Convert.ToBase64String(saltCode),
                                IdRegion = data.IdRegion == 0 ? null : data.IdRegion,
                                IdPais = data.IdPais,
                                IdTipoUser = data.IdTipoUser,
                                IdEspMad = data.IdEspecialidadMadre,
                                IdSubEspQx = data.IdSubEspQx == 0 ? null : data.IdSubEspQx,
                                IdSubEspMed = data.IdSubEspMed == 0 ? null : data.IdSubEspMed,
                                IdSubEspPed = data.IdSubEspPed == 0 ? null : data.IdSubEspPed,
                                IdSubEspGobs = data.IdSubEspGobs == 0 ? null : data.IdSubEspGobs,
                                IdSubEspAnMedCr = data.IdSubEspAnMedCr == 0 ? null : data.IdSubEspAnMedCr,
                                IdSubEspRad = data.IdSubEspRad == 0 ? null : data.IdSubEspRad,
                                IdSubEspOdo = data.IdSubEspOdo == 0 ? null : data.IdSubEspOdo,
                                IdSubEspTec = data.IdSubEspTec == 0 ? null : data.IdSubEspTec,
                                OtroLugTrab = data.OtroLugarTrabajo,
                                IdClinica = data.IdClinica == 0 ? null : data.IdClinica,
                                OtraEsp = data.OtraEsp,
                                Helper = 0,
                                Disponible = 0

                            };
                            Db.Users.Add(datos);
                            Db.SaveChanges(true);
                            iduser = datos.IdUser;

                            //experticia
                            if (data.IdEspecialidadMadre == 2)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspMed == data.IdSubEspMed).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 3)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspQx == data.IdSubEspQx).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 4)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspGobs == data.IdSubEspGobs).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 5)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspPed == data.IdSubEspPed).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }

                            if (data.IdEspecialidadMadre == 7)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspAnMedCr == data.IdSubEspAnMedCr).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 8)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspRad == data.IdSubEspRad).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 9)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspOdo == data.IdSubEspOdo).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 10)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspTec == data.IdSubEspTec).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                        } 
                    }
                        else
                        {
                            var mssge = dynJson.message;
                            
                           return StatusCode(StatusCodes.Status401Unauthorized, "Error ApiHubSpot " + mssge + "!");
                            

                        }



                    //}else
                    int? idSub = 0;

                    if (data.IdEspecialidadMadre == 2)
                    {
                        idSub = data.IdSubEspMed;
                    }
                    if (data.IdEspecialidadMadre == 3)
                    {
                        idSub = data.IdSubEspQx;
                    }
                    if (data.IdEspecialidadMadre == 4)
                    {
                        idSub = data.IdSubEspGobs;
                    }
                    if (data.IdEspecialidadMadre == 5)
                    {
                        idSub = data.IdSubEspPed;
                    }
                    if (data.IdEspecialidadMadre == 7)
                    {
                        idSub = data.IdSubEspAnMedCr;
                    }
                    if (data.IdEspecialidadMadre == 8)
                    {
                        idSub = data.IdSubEspRad;
                    }
                    if (data.IdEspecialidadMadre == 9)
                    {
                        idSub = data.IdSubEspOdo;
                    }
                    if (data.IdEspecialidadMadre == 10)
                    {
                        idSub = data.IdSubEspTec;
                    }

                    var datosA = new LoginResult()
                    {
                        status = "Autorizado",
                        emailUser = data.Email,
                        idUser = iduser,
                        nameUser = data.Nombres + " " + data.PrimerApellido,
                        idEspMad = data.IdEspecialidadMadre,
                        idSubEsp = idSub,
                        idPais = data.IdPais,
                        disponible = 0

                    };
                    return Ok(datosA);

                }//si es otro pais
                else
                {
                    var hapikey = Configuration["keyHubSpot"];
                    var client = new RestClient("https://api.hubapi.com/");
                    var request = new RestRequest("contacts/v1/contact", Method.POST);
                    request.AddQueryParameter("hapikey", hapikey);
                    request.RequestFormat = DataFormat.Json;

                    request.AddJsonBody(new
                    {
                        properties = new[]
                        {
                                new{property="firstname",value=data.Nombres},
                                new{property="lastname",value=data.PrimerApellido},
                                new{property="segundo_apellido",value=data.SegundoApellido},
                                new{property="fecha_de_nacimiento",value=utcValue.ToString()},
                                new{property="rut_o_pasaporte",value=utcValue.ToString()},
                                new{property="nacionalidad",value=nac.PaisNac},
                                new{property="idioma_preferido",value=data.Idioma},
                                new{property="sexo",value=data.Sexo},
                                new{property="mobilephone",value=data.NroCelular},
                                new{property="work_email",value=data.Email},
                                new{property="email",value=data.Email},
                                new{property="pais",value=pais.Nombre},
                                new{property="tipo_de_usuario",value=tuser.DescripcionInterna},
                                new{property="especialidad_madre",value=espmad.Descripcion},
                                new{property="sub_especialidad_qx_",value= data.IdSubEspQx == 0 ? "" :subqx.Descripcion},
                                new{property="sub_especialidad",value= data.IdSubEspMed == 0 ? "" :submed.Descripcion},
                                new{property="sub_especialidad_ped_",value= data.IdSubEspPed == 0 ? "":subped.Descripcion},
                                new{property="sub_especialidad_gobs_",value= data.IdSubEspGobs == 0 ? "": subgobs.Descripcion},
                                new{property="anestesiologia",value= data.IdSubEspAnMedCr == 0 ? "": submedcri.Descripcion},
                                new{property="radiologia",value= data.IdSubEspRad == 0 ? "": subrad.Descripcion},
                                new{property="odontologia",value= data.IdSubEspOdo == 0 ? "": submedodo.Descrpcion},
                                new{property="tecnicos",value= data.IdSubEspTec == 0 ? "": subtec.Descripcion},
                                new{property="lugar_de_trabajo_i_",value= data.IdClinica == 0 ? "": hosp.Descripcion},
                                new{property="lugar_de_trabajo_ii_",value= data.OtroLugarTrabajo},
                                new{property="otra_especialidad",value= data.OtraEsp },

                            }
                    });

                    IRestResponse responseHubSpot = client.Execute(request);
                    dynamic dynJson = JsonConvert.DeserializeObject(responseHubSpot.Content.ToString());
                    var exisContact = dynJson.message;
                    if (responseHubSpot.IsSuccessful)
                    {
                        var saltCode = HashServices.GetSaltCode();
                        Users datos = new Users()
                        {
                            Nombres = data.Nombres,
                            PApellido = data.PrimerApellido,
                            SApellido = data.SegundoApellido,
                            FechaNac = data.FechaNac,
                            RutPass = data.RutPass,
                            IdNacionalidad = data.IdNacionalidad,
                            Idioma = data.Idioma,
                            Sexo = data.Sexo,
                            NroCelular = data.NroCelular,
                            Email = data.Email,
                            Password = HashServices.HashPassword(data.Password, saltCode),
                            SaltCode = Convert.ToBase64String(saltCode),
                            IdRegion = data.IdRegion == 0 ? null : data.IdRegion,
                            IdPais = data.IdPais,
                            IdTipoUser = data.IdTipoUser,
                            IdEspMad = data.IdEspecialidadMadre,
                            IdSubEspQx = data.IdSubEspQx == 0 ? null : data.IdSubEspQx,
                            IdSubEspMed = data.IdSubEspMed == 0 ? null : data.IdSubEspMed,
                            IdSubEspPed = data.IdSubEspPed == 0 ? null : data.IdSubEspPed,
                            IdSubEspGobs = data.IdSubEspGobs == 0 ? null : data.IdSubEspGobs,
                            IdSubEspAnMedCr = data.IdSubEspAnMedCr == 0 ? null : data.IdSubEspAnMedCr,
                            IdSubEspRad = data.IdSubEspRad == 0 ? null : data.IdSubEspRad,
                            IdSubEspOdo = data.IdSubEspOdo == 0 ? null : data.IdSubEspOdo,
                            IdSubEspTec = data.IdSubEspTec == 0 ? null : data.IdSubEspTec,
                            OtroLugTrab = data.OtroLugarTrabajo,
                            IdClinica = data.IdClinica == 0 ? null : data.IdClinica,
                            OtraEsp = data.OtraEsp,
                            Helper = 0,
                            Disponible = 0
                        };
                        Db.Users.Add(datos);
                        Db.SaveChanges(true);
                        iduser = datos.IdUser;
                        //experticia
                        if (data.IdEspecialidadMadre == 2)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspMed == data.IdSubEspMed).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        if (data.IdEspecialidadMadre == 3)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspQx == data.IdSubEspQx).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        if (data.IdEspecialidadMadre == 4)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspGobs == data.IdSubEspGobs).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        if (data.IdEspecialidadMadre == 5)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspPed == data.IdSubEspPed).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        if (data.IdEspecialidadMadre == 7)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspAnMedCr == data.IdSubEspAnMedCr).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        if (data.IdEspecialidadMadre == 8)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspRad == data.IdSubEspRad).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        if (data.IdEspecialidadMadre == 9)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspOdo == data.IdSubEspOdo).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                        if (data.IdEspecialidadMadre == 10)
                        {
                            var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspTec == data.IdSubEspTec).ToList();
                            foreach (var item in queryEsp)
                            {
                                ExperticiaUser datosExpUser = new ExperticiaUser()
                                {
                                    IdUser = datos.IdUser,
                                    IdExp = item.IdExp,
                                    Result = ""

                                };
                                Db.ExperticiaUser.Add(datosExpUser);
                                Db.SaveChanges(true);
                            }
                        }
                    } else if (exisContact == "Contact already exists") {
                        //si existe contacto se actualiza en hubspot

                            var idContact = dynJson.identityProfile.vid;
                            var clientExist = new RestClient("https://api.hubapi.com/");
                            var requestExist = new RestRequest("contacts/v1/contact/vid/" + idContact + "/profile", Method.POST);
                            requestExist.AddQueryParameter("hapikey", hapikey);
                            requestExist.RequestFormat = DataFormat.Json;

                            requestExist.AddJsonBody(new
                            {
                                properties = new[]
                                {
                                new{property="firstname",value=data.Nombres},
                                new{property="lastname",value=data.PrimerApellido},
                                new{property="segundo_apellido",value=data.SegundoApellido},
                                new{property="fecha_de_nacimiento",value=utcValue.ToString()},
                                new{property="rut_o_pasaporte",value=utcValue.ToString()},
                                new{property="nacionalidad",value=nac.PaisNac},
                                new{property="idioma_preferido",value=data.Idioma},
                                new{property="sexo",value=data.Sexo},
                                new{property="mobilephone",value=data.NroCelular},
                                new{property="work_email",value=data.Email},
                                new{property="email",value=data.Email},
                                new{property="pais",value=pais.Nombre},
                                new{property="tipo_de_usuario",value=tuser.DescripcionInterna},
                                new{property="especialidad_madre",value=espmad.Descripcion},
                                new{property="sub_especialidad_qx_",value= data.IdSubEspQx == 0 ? "" :subqx.Descripcion},
                                new{property="sub_especialidad",value= data.IdSubEspMed == 0 ? "" :submed.Descripcion},
                                new{property="sub_especialidad_ped_",value= data.IdSubEspPed == 0 ? "":subped.Descripcion},
                                new{property="sub_especialidad_gobs_",value= data.IdSubEspGobs == 0 ? "": subgobs.Descripcion},
                                new{property="anestesiologia",value= data.IdSubEspAnMedCr == 0 ? "": submedcri.Descripcion},
                                new{property="radiologia",value= data.IdSubEspRad == 0 ? "": subrad.Descripcion},
                                new{property="odontologia",value= data.IdSubEspOdo == 0 ? "": submedodo.Descrpcion},
                                new{property="tecnicos",value= data.IdSubEspTec == 0 ? "": subtec.Descripcion},
                                new{property="lugar_de_trabajo_i_",value= data.IdClinica == 0 ? "": hosp.Descripcion},
                                new{property="lugar_de_trabajo_ii_",value= data.OtroLugarTrabajo},
                                new{property="otra_especialidad",value= data.OtraEsp },

                            }
                            });

                            IRestResponse responseHubSpotExist = clientExist.Execute(requestExist);

                        if (responseHubSpotExist.IsSuccessful)
                        {
                            var saltCode = HashServices.GetSaltCode();
                            Users datos = new Users()
                            {
                                Nombres = data.Nombres,
                                PApellido = data.PrimerApellido,
                                SApellido = data.SegundoApellido,
                                FechaNac = data.FechaNac,
                                RutPass = data.RutPass,
                                IdNacionalidad = data.IdNacionalidad,
                                Idioma = data.Idioma,
                                Sexo = data.Sexo,
                                NroCelular = data.NroCelular,
                                Email = data.Email,
                                Password = HashServices.HashPassword(data.Password, saltCode),
                                SaltCode = Convert.ToBase64String(saltCode),
                                IdRegion = data.IdRegion == 0 ? null : data.IdRegion,
                                IdPais = data.IdPais,
                                IdTipoUser = data.IdTipoUser,
                                IdEspMad = data.IdEspecialidadMadre,
                                IdSubEspQx = data.IdSubEspQx == 0 ? null : data.IdSubEspQx,
                                IdSubEspMed = data.IdSubEspMed == 0 ? null : data.IdSubEspMed,
                                IdSubEspPed = data.IdSubEspPed == 0 ? null : data.IdSubEspPed,
                                IdSubEspGobs = data.IdSubEspGobs == 0 ? null : data.IdSubEspGobs,
                                IdSubEspAnMedCr = data.IdSubEspAnMedCr == 0 ? null : data.IdSubEspAnMedCr,
                                IdSubEspRad = data.IdSubEspRad == 0 ? null : data.IdSubEspRad,
                                IdSubEspOdo = data.IdSubEspOdo == 0 ? null : data.IdSubEspOdo,
                                IdSubEspTec = data.IdSubEspTec == 0 ? null : data.IdSubEspTec,
                                OtroLugTrab = data.OtroLugarTrabajo,
                                IdClinica = data.IdClinica == 0 ? null : data.IdClinica,
                                OtraEsp = data.OtraEsp,
                                Helper = 0,
                                Disponible = 0
                            };
                            Db.Users.Add(datos);
                            Db.SaveChanges(true);
                            iduser = datos.IdUser;
                            //experticia
                            if (data.IdEspecialidadMadre == 2)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspMed == data.IdSubEspMed).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 3)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspQx == data.IdSubEspQx).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 4)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspGobs == data.IdSubEspGobs).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 5)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspPed == data.IdSubEspPed).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 7)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspAnMedCr == data.IdSubEspAnMedCr).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 8)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspRad == data.IdSubEspRad).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 9)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspOdo == data.IdSubEspOdo).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                            if (data.IdEspecialidadMadre == 10)
                            {
                                var queryEsp = Db.ExperticiasEspecialidades.Where(t => t.IdEspMad == data.IdEspecialidadMadre && t.IdSubEspTec == data.IdSubEspTec).ToList();
                                foreach (var item in queryEsp)
                                {
                                    ExperticiaUser datosExpUser = new ExperticiaUser()
                                    {
                                        IdUser = datos.IdUser,
                                        IdExp = item.IdExp,
                                        Result = ""

                                    };
                                    Db.ExperticiaUser.Add(datosExpUser);
                                    Db.SaveChanges(true);
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        var mssge = dynJson.message;

                        return StatusCode(StatusCodes.Status401Unauthorized, "Error ApiHubSpot " + mssge + "!");
                    }



                    int? idSub = 0;

                    if (data.IdEspecialidadMadre == 2)
                    {
                        idSub = data.IdSubEspMed;
                    }
                    if (data.IdEspecialidadMadre == 3)
                    {
                        idSub = data.IdSubEspQx;
                    }
                    if (data.IdEspecialidadMadre == 4)
                    {
                        idSub = data.IdSubEspGobs;
                    }
                    if (data.IdEspecialidadMadre == 5)
                    {
                        idSub = data.IdSubEspPed;
                    }
                    if (data.IdEspecialidadMadre == 7)
                    {
                        idSub = data.IdSubEspAnMedCr;
                    }
                    if (data.IdEspecialidadMadre == 8)
                    {
                        idSub = data.IdSubEspRad;
                    }
                    if (data.IdEspecialidadMadre == 9)
                    {
                        idSub = data.IdSubEspOdo;
                    }
                    if (data.IdEspecialidadMadre == 10)
                    {
                        idSub = data.IdSubEspTec;
                    }

                    var datosA = new LoginResult()
                    {
                        status = "Autorizado",
                        emailUser = data.Email,
                        idUser = iduser,
                        nameUser = data.Nombres + " " + data.PrimerApellido,
                        nameUserDr = data.Sexo == "Femenino" ? "Dra. " + data.Nombres + " " + data.PrimerApellido + "" : "Dr. " + data.Nombres + " " + data.PrimerApellido + "",
                        idEspMad = data.IdEspecialidadMadre,
                        idSubEsp = idSub,
                        idPais = data.IdPais,
                        disponible = 0

                    };
                    return Ok(datosA);
                }


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }



        [HttpGet("users/{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult GetUsersid(int id)
        {
            try
            {
                var List = new List<UsersResult>();
                var query = Db.Users.FirstOrDefault(t => t.IdUser == id);

                if(query.IdEspMad == 2)
                {
                    var nac = Db.Nacionalidad.FirstOrDefault(t=>t.IdNacionalidad == query.IdNacionalidad);
                    var reg = Db.Region.FirstOrDefault(t => t.IdRegion == query.IdRegion);
                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == query.IdPais);
                    var tipouser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == query.IdTipoUser);
                    var espmadre = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == query.IdEspMad);
                    var espSub = Db.SubEspMed.FirstOrDefault(t => t.IdSubEspMed == query.IdSubEspMed);
                    var hosp = Db.Clinicas.FirstOrDefault(t => t.IdClinica == query.IdClinica);
                    var datos = new UsersResult()
                    {
                        IdUser = query.IdUser,
                        Nombres = query.Nombres,
                        PrimerApellido = query.PApellido,
                        SegundoApellido = query.SApellido,
                        FechaNac = query.FechaNac == null ? "" : query.FechaNac.ToString("dd-MM-yyyy"),
                        RutPass = query.RutPass,
                        Nacionalidad = nac.GentilicioNac,
                        Sexo = query.Sexo,
                        NroCelular = query.NroCelular,
                        Email = query.Email,
                        Region = query.IdPais != 46 ? "" : reg.Descripcion,
                        Pais = pais.Nombre,
                        TipoUser = tipouser.Descripcion,
                        EspecialidadMadre = espmadre.Descripcion,
                        SubEsp = espSub.Descripcion,
                        LugarTrabajo = query.IdPais == 46 ? hosp.Descripcion : query.OtroLugTrab
                    };
                    List.Add(datos);
                }
                if (query.IdEspMad == 3)
                {
                    var nac = Db.Nacionalidad.FirstOrDefault(t => t.IdNacionalidad == query.IdNacionalidad);
                    var reg = Db.Region.FirstOrDefault(t => t.IdRegion == query.IdRegion);
                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == query.IdPais);
                    var tipouser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == query.IdTipoUser);
                    var espmadre = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == query.IdEspMad);
                    var espSub = Db.SubEspQx.FirstOrDefault(t => t.IdSubEspQx == query.IdSubEspQx);
                    var hosp = Db.Clinicas.FirstOrDefault(t => t.IdClinica == query.IdClinica);
                    var datos = new UsersResult()
                    {
                        IdUser = query.IdUser,
                        Nombres = query.Nombres,
                        PrimerApellido = query.PApellido,
                        SegundoApellido = query.SApellido,
                        FechaNac = query.FechaNac == null ? "" : query.FechaNac.ToString("dd-MM-yyyy"),
                        RutPass = query.RutPass,
                        Nacionalidad = nac.GentilicioNac,
                        Sexo = query.Sexo,
                        NroCelular = query.NroCelular,
                        Email = query.Email,
                        Region = query.IdPais != 46 ? "": reg.Descripcion,
                        Pais = pais.Nombre,
                        TipoUser = tipouser.Descripcion,
                        EspecialidadMadre = espmadre.Descripcion,
                        SubEsp = espSub.Descripcion,
                        LugarTrabajo = query.IdPais == 46 ? hosp.Descripcion : query.OtroLugTrab
                    };
                    List.Add(datos);
                }
                if (query.IdEspMad == 4)
                {
                    var nac = Db.Nacionalidad.FirstOrDefault(t => t.IdNacionalidad == query.IdNacionalidad);
                    var reg = Db.Region.FirstOrDefault(t => t.IdRegion == query.IdRegion);
                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == query.IdPais);
                    var tipouser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == query.IdTipoUser);
                    var espmadre = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == query.IdEspMad);
                    var espSub = Db.SubEspGobs.FirstOrDefault(t => t.IdSubEspGobs == query.IdSubEspGobs);
                    var hosp = Db.Clinicas.FirstOrDefault(t => t.IdClinica == query.IdClinica);
                    var datos = new UsersResult()
                    {
                        IdUser = query.IdUser,
                        Nombres = query.Nombres,
                        PrimerApellido = query.PApellido,
                        SegundoApellido = query.SApellido,
                        FechaNac = query.FechaNac == null ? "" : query.FechaNac.ToString("dd-MM-yyyy"),
                        RutPass = query.RutPass,
                        Nacionalidad = nac.GentilicioNac,
                        Sexo = query.Sexo,
                        NroCelular = query.NroCelular,
                        Email = query.Email,
                        Region = query.IdPais != 46 ? "" : reg.Descripcion,
                        Pais = pais.Nombre,
                        TipoUser = tipouser.Descripcion,
                        EspecialidadMadre = espmadre.Descripcion,
                        SubEsp = espSub.Descripcion,
                        LugarTrabajo = query.IdPais == 46 ? hosp.Descripcion : query.OtroLugTrab
                    };
                    List.Add(datos);
                }
                if (query.IdEspMad == 5)
                {
                    var nac = Db.Nacionalidad.FirstOrDefault(t => t.IdNacionalidad == query.IdNacionalidad);
                    var reg = Db.Region.FirstOrDefault(t => t.IdRegion == query.IdRegion);
                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == query.IdPais);
                    var tipouser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == query.IdTipoUser);
                    var espmadre = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == query.IdEspMad);
                    var espSub = Db.SubEspPed.FirstOrDefault(t => t.IdSubEspPed == query.IdSubEspPed);
                    var hosp = Db.Clinicas.FirstOrDefault(t => t.IdClinica == query.IdClinica);
                    var datos = new UsersResult()
                    {
                        IdUser = query.IdUser,
                        Nombres = query.Nombres,
                        PrimerApellido = query.PApellido,
                        SegundoApellido = query.SApellido,
                        FechaNac = query.FechaNac == null ? "" : query.FechaNac.ToString("dd-MM-yyyy"),
                        RutPass = query.RutPass,
                        Nacionalidad = nac.GentilicioNac,
                        Sexo = query.Sexo,
                        NroCelular = query.NroCelular,
                        Email = query.Email,
                        Region = query.IdPais != 46 ? "" : reg.Descripcion,
                        Pais = pais.Nombre,
                        TipoUser = tipouser.Descripcion,
                        EspecialidadMadre = espmadre.Descripcion,
                        SubEsp = espSub.Descripcion,
                        LugarTrabajo = query.IdPais == 46 ? hosp.Descripcion : query.OtroLugTrab
                    };
                    List.Add(datos);
                }
                if (query.IdEspMad == 7)
                {
                    var nac = Db.Nacionalidad.FirstOrDefault(t => t.IdNacionalidad == query.IdNacionalidad);
                    var reg = Db.Region.FirstOrDefault(t => t.IdRegion == query.IdRegion);
                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == query.IdPais);
                    var tipouser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == query.IdTipoUser);
                    var espmadre = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == query.IdEspMad);
                    var espSub = Db.SubEspAnMedCr.FirstOrDefault(t => t.IdSubEspAnMedCr == query.IdSubEspAnMedCr);
                    var hosp = Db.Clinicas.FirstOrDefault(t => t.IdClinica == query.IdClinica);
                    var datos = new UsersResult()
                    {
                        IdUser = query.IdUser,
                        Nombres = query.Nombres,
                        PrimerApellido = query.PApellido,
                        SegundoApellido = query.SApellido,
                        FechaNac = query.FechaNac == null ? "" : query.FechaNac.ToString("dd-MM-yyyy"),
                        RutPass = query.RutPass,
                        Nacionalidad = nac.GentilicioNac,
                        Sexo = query.Sexo,
                        NroCelular = query.NroCelular,
                        Email = query.Email,
                        Region = query.IdPais != 46 ? "" : reg.Descripcion,
                        Pais = pais.Nombre,
                        TipoUser = tipouser.Descripcion,
                        EspecialidadMadre = espmadre.Descripcion,
                        SubEsp = espSub.Descripcion,
                        LugarTrabajo = query.IdPais == 46 ? hosp.Descripcion : query.OtroLugTrab
                    };
                    List.Add(datos);
                }
                if (query.IdEspMad == 8)
                {
                    var nac = Db.Nacionalidad.FirstOrDefault(t => t.IdNacionalidad == query.IdNacionalidad);
                    var reg = Db.Region.FirstOrDefault(t => t.IdRegion == query.IdRegion);
                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == query.IdPais);
                    var tipouser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == query.IdTipoUser);
                    var espmadre = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == query.IdEspMad);
                    var espSub = Db.SubEspRad.FirstOrDefault(t => t.IdSubEspRad == query.IdSubEspRad);
                    var hosp = Db.Clinicas.FirstOrDefault(t => t.IdClinica == query.IdClinica);
                    var datos = new UsersResult()
                    {
                        IdUser = query.IdUser,
                        Nombres = query.Nombres,
                        PrimerApellido = query.PApellido,
                        SegundoApellido = query.SApellido,
                        FechaNac = query.FechaNac == null ? "" : query.FechaNac.ToString("dd-MM-yyyy"),
                        RutPass = query.RutPass,
                        Nacionalidad = nac.GentilicioNac,
                        Sexo = query.Sexo,
                        NroCelular = query.NroCelular,
                        Email = query.Email,
                        Region = query.IdPais != 46 ? "" : reg.Descripcion,
                        Pais = pais.Nombre,
                        TipoUser = tipouser.Descripcion,
                        EspecialidadMadre = espmadre.Descripcion,
                        SubEsp = espSub.Descripcion,
                        LugarTrabajo = query.IdPais == 46 ? hosp.Descripcion : query.OtroLugTrab
                    };
                    List.Add(datos);
                }
                if (query.IdEspMad == 9)
                {
                    var nac = Db.Nacionalidad.FirstOrDefault(t => t.IdNacionalidad == query.IdNacionalidad);
                    var reg = Db.Region.FirstOrDefault(t => t.IdRegion == query.IdRegion);
                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == query.IdPais);
                    var tipouser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == query.IdTipoUser);
                    var espmadre = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == query.IdEspMad);
                    var espSub = Db.SubEspOdo.FirstOrDefault(t => t.IdSubEspOdo == query.IdSubEspOdo);
                    var hosp = Db.Clinicas.FirstOrDefault(t => t.IdClinica == query.IdClinica);
                    var datos = new UsersResult()
                    {
                        IdUser = query.IdUser,
                        Nombres = query.Nombres,
                        PrimerApellido = query.PApellido,
                        SegundoApellido = query.SApellido,
                        FechaNac = query.FechaNac == null ? "" : query.FechaNac.ToString("dd-MM-yyyy"),
                        RutPass = query.RutPass,
                        Nacionalidad = nac.GentilicioNac,
                        Sexo = query.Sexo,
                        NroCelular = query.NroCelular,
                        Email = query.Email,
                        Region = query.IdPais != 46 ? "" : reg.Descripcion,
                        Pais = pais.Nombre,
                        TipoUser = tipouser.Descripcion,
                        EspecialidadMadre = espmadre.Descripcion,
                        SubEsp = espSub.Descrpcion,
                        LugarTrabajo = query.IdPais == 46 ? hosp.Descripcion : query.OtroLugTrab
                    };
                    List.Add(datos);
                }
                if (query.IdEspMad == 10)
                {
                    var nac = Db.Nacionalidad.FirstOrDefault(t => t.IdNacionalidad == query.IdNacionalidad);
                    var reg = Db.Region.FirstOrDefault(t => t.IdRegion == query.IdRegion);
                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == query.IdPais);
                    var tipouser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == query.IdTipoUser);
                    var espmadre = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == query.IdEspMad);
                    var espSub = Db.SubEspTec.FirstOrDefault(t => t.IdSubEspTec == query.IdSubEspTec);
                    var hosp = Db.Clinicas.FirstOrDefault(t => t.IdClinica == query.IdClinica);
                    var datos = new UsersResult()
                    {
                        IdUser = query.IdUser,
                        Nombres = query.Nombres,
                        PrimerApellido = query.PApellido,
                        SegundoApellido = query.SApellido,
                        FechaNac = query.FechaNac == null ? "" : query.FechaNac.ToString("dd-MM-yyyy"),
                        RutPass = query.RutPass,
                        Nacionalidad = nac.GentilicioNac,
                        Sexo = query.Sexo,
                        NroCelular = query.NroCelular,
                        Email = query.Email,
                        Region = query.IdPais != 46 ? "" : reg.Descripcion,
                        Pais = pais.Nombre,
                        TipoUser = tipouser.Descripcion,
                        EspecialidadMadre = espmadre.Descripcion,
                        SubEsp = espSub.Descripcion,
                        LugarTrabajo = query.IdPais == 46 ? hosp.Descripcion : query.OtroLugTrab
                    };
                    List.Add(datos);
                }
                if (query.IdEspMad == 11)
                {
                    var nac = Db.Nacionalidad.FirstOrDefault(t => t.IdNacionalidad == query.IdNacionalidad);
                    var reg = Db.Region.FirstOrDefault(t => t.IdRegion == query.IdRegion);
                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == query.IdPais);
                    var tipouser = Db.TipoUser.FirstOrDefault(t => t.IdTipoUser == query.IdTipoUser);
                    var espmadre = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == query.IdEspMad);
                    var hosp = Db.Clinicas.FirstOrDefault(t => t.IdClinica == query.IdClinica);
                    var datos = new UsersResult()
                    {
                        IdUser = query.IdUser,
                        Nombres = query.Nombres,
                        PrimerApellido = query.PApellido,
                        SegundoApellido = query.SApellido,
                        FechaNac = query.FechaNac == null ? "" : query.FechaNac.ToString("dd-MM-yyyy"),
                        RutPass = query.RutPass,
                        Nacionalidad = nac.GentilicioNac,
                        Sexo = query.Sexo,
                        NroCelular = query.NroCelular,
                        Email = query.Email,
                        Region = query.IdPais != 46 ? "" : reg.Descripcion,
                        Pais = pais.Nombre,
                        TipoUser = tipouser.Descripcion,
                        EspecialidadMadre = espmadre.Descripcion,
                        SubEsp = query.OtraEsp,
                        LugarTrabajo = query.IdPais == 46 ? hosp.Descripcion : query.OtroLugTrab
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
    }
}
