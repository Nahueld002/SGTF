Imports System.Web.Mvc
Namespace Controllers
    Public Class PartidoController
        Inherits Controller
        Dim db As FutbolDB2Entities = New FutbolDB2Entities()

        ' GET: Torneo
        Function Index() As ActionResult
            Return View()
        End Function
        Function ListarPartido() As JsonResult
            Dim listado = (From p In db.Partido.AsEnumerable()  ' Forzamos la ejecución en memoria
                           Let torneo = p.Torneo
                           Let localEquipo = If(p.TorneoEquipo IsNot Nothing AndAlso p.TorneoEquipo.Equipo IsNot Nothing, p.TorneoEquipo.Equipo, Nothing)
                           Let visitanteEquipo = If(p.TorneoEquipo1 IsNot Nothing AndAlso p.TorneoEquipo1.Equipo IsNot Nothing, p.TorneoEquipo1.Equipo, Nothing)
                           Select New With {
                       .PartidoID = p.PartidoID,
                       .Torneo = torneo?.Nombre,
                       .Fecha = p.NroFecha,
                       .Año = p.AñoParticipacion,
                       .Fase = p.Fase,
                       .Grupo = p.Grupo,
                       .Estado = p.Estado,
                       .Local = If(localEquipo IsNot Nothing, localEquipo.Nombre, "Equipo Local"),
                       .Visitante = If(visitanteEquipo IsNot Nothing, visitanteEquipo.Nombre, "Equipo Visitante"),
                       .Marcador = p.GolesLocal.ToString() & " - " & p.GolesVisitante.ToString()
                   }).ToList()

            Return Json(listado, JsonRequestBehavior.AllowGet)
        End Function


        Function Guardar(objPartido As Partido) As Integer
            Dim respuesta As Integer
            Try
                If objPartido.PartidoID = 0 Then
                    db.Partido.Add(objPartido)
                    db.SaveChanges()
                    respuesta = 1
                Else
                    Dim registro = (From p In db.Partido
                                    Where p.PartidoID = objPartido.PartidoID
                                    Select p).FirstOrDefault()

                    If registro IsNot Nothing Then
                        registro.TorneoID = objPartido.TorneoID
                        registro.EquipoLocalTorneoEquipoID = objPartido.EquipoLocalTorneoEquipoID
                        registro.EquipoVisitanteTorneoEquipoID = objPartido.EquipoVisitanteTorneoEquipoID
                        registro.GolesLocal = objPartido.GolesLocal
                        registro.GolesVisitante = objPartido.GolesVisitante
                        registro.NroFecha = objPartido.NroFecha
                        registro.AñoParticipacion = objPartido.AñoParticipacion
                        registro.Fase = objPartido.Fase
                        registro.Grupo = objPartido.Grupo
                        registro.Estado = objPartido.Estado
                        db.SaveChanges()
                        respuesta = 1
                    End If
                End If
            Catch ex As Exception
                respuesta = 0
            End Try
            Return respuesta
        End Function
        Function Eliminar(id As Integer) As Integer
            Dim respuesta As Integer
            Try
                Dim objPartido As Partido = db.Partido.Find(id)
                If objPartido IsNot Nothing Then
                    db.Partido.Remove(objPartido)
                    db.SaveChanges()
                    respuesta = 1
                End If
            Catch ex As Exception
                respuesta = 0
            End Try
            Return respuesta
        End Function

        Function RecuperarPartido(id As Integer) As JsonResult
            Dim partido = From p In db.Partido
                          Where p.PartidoID = id
                          Select New With {
                      p.PartidoID,
                      p.TorneoID,
                      p.EquipoLocalTorneoEquipoID,
                      p.EquipoVisitanteTorneoEquipoID,
                      p.GolesLocal,
                      p.GolesVisitante,
                      p.NroFecha,
                      p.AñoParticipacion,
                      p.Fase,
                      p.Grupo,
                      p.Estado
                  }
            Return New JsonResult With {.Data = partido, .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        Function CargarTorneos() As JsonResult
            Try
                Dim torneos = From t In db.Torneo
                              Order By t.Nombre
                              Select New With {
                          t.TorneoID,
                          t.Nombre
                      }
                Return New JsonResult With {.Data = torneos.ToList(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            Catch ex As Exception
                Return New JsonResult With {.Data = New List(Of Object)(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            End Try
        End Function
        Function CargarTorneoEquipos() As JsonResult
            Try
                Dim equipos = From te In db.TorneoEquipo
                              Where te.Equipo IsNot Nothing
                              Order By te.Equipo.Nombre
                              Select New With {
                          te.TorneoEquipoID,
                          .Nombre = te.Equipo.Nombre & " (" & te.AñoParticipacion & ")"
                      }
                Return New JsonResult With {.Data = equipos.ToList(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            Catch ex As Exception
                Return New JsonResult With {.Data = New List(Of Object)(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            End Try
        End Function
        Function CargarFases() As JsonResult
            Dim fases = New List(Of Object) From {
        New With {.Clave = "Grupos", .Nombre = "Fase de Grupos"},
        New With {.Clave = "Semifinal", .Nombre = "Semifinal"},
        New With {.Clave = "Final", .Nombre = "Final"}
    }
            Return New JsonResult With {.Data = fases, .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function
    End Class
End Namespace