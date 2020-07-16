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
using RestSharp;
using Newtonsoft.Json;

namespace ApiSharedMed.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ServicesController : Controller
    {
        private IConfiguration _configuration;
        private devsharedmedContext _db;

        protected IConfiguration Configuration => _configuration;
        protected ContextBD.Models.devsharedmedContext Db => _db;

        public ServicesController(devsharedmedContext eFContext, IConfiguration configuration)
        {
            _db = eFContext;
            _configuration = configuration;
        }




        [HttpPost("search/helper")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult SearchServicio(ServicioSearch data)
        {
            try
            {
                

                //-------------busqueda Helper----------
                var List = new List<ServicioSearchResult>();
                var List2 = new List<ServicioSearchResult>();
                var ListSer = new List<ServicioData>();
                var query = new List<Users>();

                var datosServicio = new ServicioData()
                {

                    IdUserService = 0,
                    IdUser = data.idUser,
                    Prioridad = data.Prioridad,
                    Idioma = data.Idioma,
                    IdEspMad = data.IdEspMad,
                    IdSubEsp = data.IdSubEsp,
                    Experticias = string.Join(",", data.Experticias),
                    Tiposervicio = data.Tiposervicio,
                    Tituloservicio = data.Tituloservicio,
                    Descripcioncaso = data.Descripcioncaso

                };
                ListSer.Add(datosServicio);
                foreach (var itemp in data.Experticias)
                {
                    var que = Db.ExperticiaUser.Where(t => t.IdExp == itemp && t.Result == "Experiencia").ToList().Distinct();
                    foreach(var itemc in que)
                    {
                        //medicina
                        var use = Db.Users.FirstOrDefault(t => t.IdUser == itemc.IdUser);
                        if (data.IdEspMad == 2)
                        {
                            if (data.Idioma == "Ambos")
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspMed == data.IdSubEsp && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == use.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                            else
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspMed == data.IdSubEsp && t.Idioma == data.Idioma && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == itemc.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }

                        }
                        //cirugia
                        if (data.IdEspMad == 3)
                        {
                            if (data.Idioma == "Ambos")
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspQx == data.IdSubEsp && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == use.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                            else
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspQx == data.IdSubEsp && t.Idioma == data.Idioma && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == itemc.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                        }
                        //Ginecobstetricia
                        if (data.IdEspMad == 4)
                        {
                            if (data.Idioma == "Ambos")
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspGobs == data.IdSubEsp && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == use.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                            else
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspGobs == data.IdSubEsp && t.Idioma == data.Idioma && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == itemc.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                        }
                        //pediatria
                        if (data.IdEspMad == 5)
                        {
                            if (data.Idioma == "Ambos")
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspPed == data.IdSubEsp && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == use.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                            else
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspPed == data.IdSubEsp && t.Idioma == data.Idioma && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == itemc.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                        }
                        //Anestesiología y Medicina Crítica
                        if (data.IdEspMad == 7)
                        {
                           if(data.Idioma == "Ambos")
                           {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspAnMedCr == data.IdSubEsp && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == use.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if(List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };
                                                    
                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                            else
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspAnMedCr == data.IdSubEsp && t.Idioma == data.Idioma && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == itemc.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }


                        }
                        //Radiología
                        if (data.IdEspMad == 8)
                        {
                            if (data.Idioma == "Ambos")
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspRad == data.IdSubEsp && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == use.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                            else
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspRad == data.IdSubEsp && t.Idioma == data.Idioma && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == itemc.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                        }
                        //odontologia
                        if (data.IdEspMad == 9)
                        {
                            if (data.Idioma == "Ambos")
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspOdo == data.IdSubEsp && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == use.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                            else
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspOdo == data.IdSubEsp && t.Idioma == data.Idioma && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == itemc.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                        }
                        //Técnicos
                        if (data.IdEspMad == 10)
                        {
                            if (data.Idioma == "Ambos")
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspTec == data.IdSubEsp && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == use.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                            else
                            {
                                query = Db.Users.Where(t => t.IdEspMad == data.IdEspMad && t.IdSubEspTec == data.IdSubEsp && t.Idioma == data.Idioma && t.Helper == 1 && t.IdUser != data.idUser && t.Disponible == 1 && t.IdUser == itemc.IdUser).ToList();
                                //busqueda helper
                                if (query.Count > 0)
                                {
                                    //post service
                                    if (List.Count > 0)
                                    {
                                        foreach (var item in query)
                                        {

                                            foreach (var itemL in List)
                                            {
                                                if (itemL.IdUserService != item.IdUser)
                                                {

                                                    var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                                    var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                                    var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                                    var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                                    var datos = new ServicioSearchResult()
                                                    {
                                                        status = "Autorizado",
                                                        Especialidad = esp.Descripcion,
                                                        Idioma = item.Idioma,
                                                        Pais = pais.Nombre,
                                                        Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                        ValorServicio = precio.Valor,
                                                        IdUserService = item.IdUser,
                                                        IdUserSolicitanteService = data.idUser

                                                    };

                                                    datos.dataServicio = ListSer;
                                                    List2.Add(datos);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var item in query)
                                        {
                                            var esp = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == item.IdEspMad);
                                            var pais = Db.Pais.FirstOrDefault(t => t.IdPais == item.IdPais);
                                            var PrecioB = Db.UserBanco.FirstOrDefault(t => t.IdUser == item.IdUser);
                                            var precio = Db.PrecioHoraAtencion.FirstOrDefault(t => t.IdPrecio == PrecioB.IdPrecio);
                                            var datos = new ServicioSearchResult()
                                            {
                                                status = "Autorizado",
                                                Especialidad = esp.Descripcion,
                                                Idioma = item.Idioma,
                                                Pais = pais.Nombre,
                                                Name = item.Sexo == "Femenino" ? "Dra. " + item.Nombres + " " + item.PApellido + "" : "Dr. " + item.Nombres + " " + item.PApellido + "",
                                                ValorServicio = precio.Valor,
                                                IdUserService = item.IdUser,
                                                IdUserSolicitanteService = data.idUser

                                            };


                                            datos.dataServicio = ListSer;
                                            List.Add(datos);
                                            List2.Add(datos);

                                        }

                                    }



                                }
                            }
                        }
                    }
                }
                return Ok(List2);
            }
            catch (Exception ex)
            {
                // Logger.LogError(ex, "AuthController Register", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


        [HttpPost("servicio")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult PostServicio(ServicioSearchData data)
        {
            try
            {
                /*
                var client = new RestClient("https://199jg9.api.infobip.com/sms/2/text/advanced");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "App 4aab7f8a9600034652840ed085b8fc20-88b4c81f-6540-4a89-b9b3-3d732ae2200f");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                request.AddParameter("application/json", "{\"messages\":[{\"from\":\"InfoSMS\",\"destinations\":[{\"to\":\""+nro+"\",\"messageId\":\"MESSAGE-ID-123-xyz\"},{\"to\":\"41793026834\"}],\"text\":\"Artık Ulusal Dil Tanımlayıcısı ile Türkçe karakterli smslerinizi rahatlıkla iletebilirsiniz.\",\"flash\":false,\"language\":{\"languageCode\":\"TR\"},\"transliteration\":\"TURKISH\",\"intermediateReport\":true,\"notifyUrl\":\"https://www.example.com/sms/advanced\",\"notifyContentType\":\"application/json\",\"callbackData\":\"DLR callback data\",\"validityPeriod\":720},{\"from\":\"41793026700\",\"destinations\":[{\"to\":\"41793026700\"}],\"text\":\"A long time ago, in a galaxy far, far away... It is a period of civil war. Rebel spaceships, striking from a hidden base, have won their first victory against the evil Galactic Empire.\",\"sendAt\":\"2021-08-25T16:00:00.000+0000\",\"deliveryTimeWindow\":{\"from\":{\"hour\":6,\"minute\":0},\"to\":{\"hour\":15,\"minute\":30},\"days\":[\"MONDAY\",\"TUESDAY\",\"WEDNESDAY\",\"THURSDAY\",\"FRIDAY\",\"SATURDAY\",\"SUNDAY\"]}}],\"bulkId\":\"BULK-ID-123-xyz\",\"tracking\":{\"track\":\"SMS\",\"type\":\"MY_CAMPAIGN\"}}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine(response.Content);
                */
                
                Servicio datosServicio = new Servicio()
                {

                    IdUserService = data.idUserService,
                    IdUser = data.idUser,
                    Prioridad = data.Prioridad,
                    Idioma = data.Idioma,
                    IdEspMad = data.IdEspMad,
                    IdSubEsp = data.IdSubEsp,
                    Experticias = data.Experticias,
                    Tiposervicio = data.Tiposervicio,
                    Tituloservicio = data.Tituloservicio,
                    Descripcioncaso = data.Descripcioncaso,
                    Status = "Pendiente"

                };
                Db.Servicio.Add(datosServicio);
                Db.SaveChanges(true);

                




                return Ok("Servicio exitoso");
            }
            catch (Exception ex)
            {
                // Logger.LogError(ex, "AuthController Register", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("services/helpers/{iduser}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult GetServiceHelper(int iduser)
        {
            try
            {
                var List = new List<ServicioHelperResult>();
                var query = Db.Servicio.Where(t=>t.IdUserService == iduser).ToList();
                foreach (var item in query)
                {
                    var userS = Db.Users.FirstOrDefault(t => t.IdUser == item.IdUser);
                    var espM = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == userS.IdEspMad);
                    //medicina
                    if (userS.IdEspMad == 2)
                    {
                        var subE = Db.SubEspMed.FirstOrDefault(t => t.IdSubEspMed == userS.IdSubEspMed);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //cirugia
                    if (userS.IdEspMad == 3)
                    {
                         var subE = Db.SubEspQx.FirstOrDefault(t => t.IdSubEspQx == userS.IdSubEspQx);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //Ginecobstetricia
                    if (userS.IdEspMad == 4)
                    {
                        var subE = Db.SubEspGobs.FirstOrDefault(t => t.IdSubEspGobs == userS.IdSubEspGobs);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //pediatria
                    if (userS.IdEspMad == 5)
                    {
                         var subE = Db.SubEspPed.FirstOrDefault(t => t.IdSubEspPed == userS.IdSubEspPed);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //Anestesiología y Medicina Crítica
                    if (userS.IdEspMad == 7)
                    {
                        var subE = Db.SubEspAnMedCr.FirstOrDefault(t => t.IdSubEspAnMedCr == userS.IdSubEspAnMedCr);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //Radiología
                    if (userS.IdEspMad == 8)
                    {
                         var subE = Db.SubEspRad.FirstOrDefault(t => t.IdSubEspRad == userS.IdSubEspRad);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //odontologia
                    if (userS.IdEspMad == 9)
                    {
                         var subE = Db.SubEspOdo.FirstOrDefault(t => t.IdSubEspOdo == userS.IdSubEspOdo);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descrpcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //Técnicos
                    if (userS.IdEspMad == 10)
                    {
                         var subE = Db.SubEspTec.FirstOrDefault(t => t.IdSubEspTec == userS.IdSubEspTec);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                }

                return Ok(List);
            }
            catch (Exception ex)
            {
                // Logger.LogError(ex, "AuthController Register", DateTime.UtcNow);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("services/helpers/solicitados/{iduser}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult GetServiceHelperSolicitados(int iduser)
        {
            try
            {
                var List = new List<ServicioHelperResult>();
                var query = Db.Servicio.Where(t => t.IdUser == iduser).ToList();
                foreach (var item in query)
                {
                    var userS = Db.Users.FirstOrDefault(t => t.IdUser == item.IdUserService);
                    var espM = Db.EspecialidadMadre.FirstOrDefault(t => t.IdEspMad == userS.IdEspMad);
                    //medicina
                    if (userS.IdEspMad == 2)
                    {
                        var subE = Db.SubEspMed.FirstOrDefault(t => t.IdSubEspMed == userS.IdSubEspMed);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status
                        };
                        List.Add(datos);
                    }
                    //cirugia
                    if (userS.IdEspMad == 3)
                    {
                        var subE = Db.SubEspQx.FirstOrDefault(t => t.IdSubEspQx == userS.IdSubEspQx);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //Ginecobstetricia
                    if (userS.IdEspMad == 4)
                    {
                        var subE = Db.SubEspGobs.FirstOrDefault(t => t.IdSubEspGobs == userS.IdSubEspGobs);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //pediatria
                    if (userS.IdEspMad == 5)
                    {
                        var subE = Db.SubEspPed.FirstOrDefault(t => t.IdSubEspPed == userS.IdSubEspPed);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //Anestesiología y Medicina Crítica
                    if (userS.IdEspMad == 7)
                    {
                        var subE = Db.SubEspAnMedCr.FirstOrDefault(t => t.IdSubEspAnMedCr == userS.IdSubEspAnMedCr);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //Radiología
                    if (userS.IdEspMad == 8)
                    {
                        var subE = Db.SubEspRad.FirstOrDefault(t => t.IdSubEspRad == userS.IdSubEspRad);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //odontologia
                    if (userS.IdEspMad == 9)
                    {
                        var subE = Db.SubEspOdo.FirstOrDefault(t => t.IdSubEspOdo == userS.IdSubEspOdo);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descrpcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
                    //Técnicos
                    if (userS.IdEspMad == 10)
                    {
                        var subE = Db.SubEspTec.FirstOrDefault(t => t.IdSubEspTec == userS.IdSubEspTec);
                        var datos = new ServicioHelperResult()
                        {
                            Descripcioncaso = item.Descripcioncaso,
                            Tituloservicio = item.Tituloservicio,
                            Prioridad = item.Prioridad,
                            Tiposervicio = item.Tiposervicio,
                            Idioma = item.Idioma,
                            EspMad = espM.Descripcion,
                            SubEsp = subE.Descripcion,
                            UserSolicitante = userS.Sexo == "Femenino" ? "Dra. " + userS.Nombres + " " + userS.PApellido + "" : "Dr. " + userS.Nombres + " " + userS.PApellido + "",
                            idService = item.IdServicio,
                            status = item.Status

                        };
                        List.Add(datos);
                    }
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
        [HttpPut("aceptar/servicio/{id}")]
        public IActionResult PutServicioStatus(int id)
        {
            try
            {
                var query = Db.Servicio.Where(t => t.IdServicio == id).ToList();

                foreach (var item in query)
                {
                    if(item.Tiposervicio == "Consulta a Expert@")
                    {
                        item.Status = "Pago Pendiente";
                    }
                    else
                    {
                        item.Status = "Aceptado";
                    }
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
    }
}
