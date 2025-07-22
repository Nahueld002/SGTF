Imports System.Web.Mvc

Namespace Controllers
    Public Class HomeController
        Inherits Controller

        Dim db As New FutbolDB2Entities()

        Function Index() As ActionResult
            Return View()
        End Function

        Function ObtenerResumen() As JsonResult
            Try
                Dim torneosActivos = db.Torneo.Count(Function(t) t.Estado = "Activo")
                Dim totalEquipos = db.Equipo.Count()
                Dim confederacionTop = db.Confederacion.
                    Where(Function(c) c.Nombre.Contains("CONMEBOL")).
                    Select(Function(c) c.Nombre).
                    FirstOrDefault()

                Return Json(New With {
                    .TorneosActivos = torneosActivos,
                    .TotalEquipos = totalEquipos,
                    .Confederacion = If(confederacionTop, "No definida")
                }, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return Json(New With {.Error = ex.Message}, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        Function ResultadosRecientes() As JsonResult
            Dim partidos = (From p In db.Partido
                            Where p.Estado = "Jugado"
                            Order By p.PartidoID Descending
                            Take 3
                            Select New With {
                                .EquipoLocal = p.TorneoEquipo.Equipo.Nombre,
                                .EquipoVisitante = p.TorneoEquipo1.Equipo.Nombre,
                                .GolesLocal = p.GolesLocal,
                                .GolesVisitante = p.GolesVisitante
                            }).ToList()

            Return Json(partidos, JsonRequestBehavior.AllowGet)
        End Function

        Function EstadisticaDestacada() As JsonResult
            Try
                ' Crear lista de participaciones por equipo
                Dim participaciones = (From p In db.Partido.AsEnumerable()
                                       Where p.Estado = "Jugado"
                                       Select New With {
                                   .EquipoID = p.TorneoEquipo.Equipo.EquipoID,
                                   .Nombre = p.TorneoEquipo.Equipo.Nombre,
                                   .TorneoEquipoID = p.EquipoLocalTorneoEquipoID,
                                   .EsLocal = True,
                                   .EsGanador = p.GolesLocal > p.GolesVisitante
                               }).Union(
                               From p In db.Partido.AsEnumerable()
                               Where p.Estado = "Jugado"
                               Select New With {
                                   .EquipoID = p.TorneoEquipo1.Equipo.EquipoID,
                                   .Nombre = p.TorneoEquipo1.Equipo.Nombre,
                                   .TorneoEquipoID = p.EquipoVisitanteTorneoEquipoID,
                                   .EsLocal = False,
                                   .EsGanador = p.GolesVisitante > p.GolesLocal
                               }).ToList()

                ' Agrupar por equipo
                Dim agrupado = (From x In participaciones
                                Group x By x.EquipoID, x.Nombre Into Grupo = Group
                                Let Partidos = Grupo.Count()
                                Let Victorias = Grupo.Count(Function(g) g.EsGanador)
                                Let Porcentaje = If(Partidos > 0, Math.Round((Victorias * 100.0) / Partidos, 2), 0)
                                Order By Porcentaje Descending
                                Select New With {
                            .EquipoID = EquipoID,
                            .Nombre = Nombre,
                            .PartidosJugados = Partidos,
                            .PorcentajeVictorias = Porcentaje
                        }).FirstOrDefault()

                Return Json(agrupado, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                Return Json(New With {.Error = ex.Message}, JsonRequestBehavior.AllowGet)
            End Try
        End Function

    End Class
End Namespace
