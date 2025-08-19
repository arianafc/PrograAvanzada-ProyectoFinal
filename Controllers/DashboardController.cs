using ProyectoFinal.EF;
using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    public class DashboardController : Controller
    {
        #region Dashboard
        [HttpGet]
        public ActionResult Dashboard()
        {

            try
            {
                using (var dbContext = new CASA_NATURAEntities())
                {
                    var NumeroActividadesActivas = dbContext.ACTIVIDADES_TB.Where(ac => ac.ID_ESTADO == 1).Count();
                    var VentasTotal = dbContext.USUARIO_ACTIVIDAD_TB.Sum(u => u.TOTAL);
                    var NumeroUsuariosActivos = dbContext.USUARIOS_TB
                        .Where(r => r.ID_ROL == 1 && r.ID_ESTADO == 1)
                                    .Count();
                    var NumeroAnimales = dbContext.ANIMAL_TB.Where(a => a.ID_ESTADO == 1).Count();
                    var NumeroAnimalesApadrinados = dbContext.ANIMAL_TB.Where(ap => ap.ID_ESTADO == 3).Count();
                    var NumeroUsuariosInactivos = dbContext.USUARIOS_TB
                        .Where(r => r.ID_ROL == 1 && r.ID_ESTADO == 2)
                                    .Count();

                    var VentasPorMes = dbContext.USUARIO_ACTIVIDAD_TB
                  .GroupBy(u => DbFunctions.TruncateTime(u.FECHA).Value.Month)
                  .Select(g => new VentasMes
                  {
                      Mes = g.Key,
                      Total = (decimal)g.Sum(x => x.TOTAL)
                  })
                  .OrderBy(v => v.Mes)
                  .ToList();

                    var NombresMeses = VentasPorMes
                .Select(v => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(v.Mes))
                .ToList();

                    // Top 3 usuarios con más donaciones
                    var TopUsuarios = dbContext.DONACIONES_TB
                        .GroupBy(u => u.ID_USUARIO)
                        .Select(g => new
                        {
                            UsuarioId = g.Key,
                            TotalDonaciones = g.Sum(x => x.MONTO)
                        })
                        .OrderByDescending(u => u.TotalDonaciones)
                        .Take(3)
                        .Join(dbContext.USUARIOS_TB,
                              d => d.UsuarioId,
                              u => u.ID_USUARIO,
                              (d, u) => new Usuario
                              {
                                  NombreCompleto = u.NOMBRE + " " + u.APELLIDO1 + " " + u.APELLIDO2,
                                  TotalDonaciones = (decimal)d.TotalDonaciones
                              })
                        .ToList();


                    var DatosVista = new DashboardModel
                    {
                        TotalVentas = (decimal)VentasTotal,
                        NumeroAnimalesApadrinados = NumeroAnimalesApadrinados,
                        NumeroAnimalesTotales = NumeroAnimales,
                        NumeroUsuariosActivos=NumeroUsuariosActivos,
                        NumeroUsuariosInactivos = NumeroUsuariosInactivos,
                        VentasPorMes = VentasPorMes,
                        NombresMeses = NombresMeses,
                        TopUsuarios = TopUsuarios,
                        NumeroActividadesActivadas = NumeroActividadesActivas
                    };

                   
                    return View(DatosVista);
                }
            }
            catch (Exception ex)
            {

                TempData["SwalError"] = "Ocurrió un error al intentar cargar el dashboard." + ex.Message;
                return View();
            }



        }
    }
    #endregion
}