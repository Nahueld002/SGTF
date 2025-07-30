Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Web.Mvc
Imports System.Data.Entity.Core.EntityClient

Namespace Controllers
    Public Class TorneoController
        Inherits Controller
        Dim db As FutbolDB2Entities = New FutbolDB2Entities()

        Function Index() As ActionResult
            Return View()
        End Function

        Function ListarTorneos() As JsonResult
            Dim listado = From t In db.Torneo
                          Select New With {
                              .TorneoID = t.TorneoID,
                              .Nombre = t.Nombre,
                              .TipoTorneo = t.TipoTorneo,
                              .Categoria = t.Categoria,
                              .Estado = t.Estado,
                              .Confederacion = If(t.Confederacion IsNot Nothing, t.Confederacion.Nombre, ""),
                              .Pais = If(t.Pais IsNot Nothing, t.Pais.Nombre, ""),
                              .Region = If(t.Region IsNot Nothing, t.Region.Nombre, ""),
                              .Ciudad = If(t.Ciudad IsNot Nothing, t.Ciudad.Nombre, "")
                          }
            Return New JsonResult With {.Data = listado, .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        Function Guardar(objTorneo As Torneo) As Integer
            Dim respuesta As Integer
            Try
                If objTorneo.TorneoID = 0 Then
                    db.Torneo.Add(objTorneo)
                    db.SaveChanges()
                    respuesta = 1
                Else
                    Dim registro = (From t In db.Torneo
                                    Where t.TorneoID = objTorneo.TorneoID
                                    Select t).FirstOrDefault()
                    If registro IsNot Nothing Then
                        registro.Nombre = objTorneo.Nombre
                        registro.TipoTorneo = objTorneo.TipoTorneo
                        registro.Categoria = objTorneo.Categoria
                        registro.Estado = objTorneo.Estado
                        registro.CiudadID = objTorneo.CiudadID
                        registro.PaisID = objTorneo.PaisID
                        registro.RegionID = objTorneo.RegionID
                        registro.ConfederacionID = objTorneo.ConfederacionID
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
                Dim objTorneo As Torneo = db.Torneo.Find(id)
                If objTorneo IsNot Nothing Then
                    db.Torneo.Remove(objTorneo)
                    db.SaveChanges()
                    respuesta = 1
                End If
            Catch ex As Exception
                respuesta = 0
            End Try
            Return respuesta
        End Function

        Function RecuperarTorneo(id As Integer) As JsonResult
            Dim torneo = From t In db.Torneo
                         Where t.TorneoID = id
                         Select New With {
                              .TorneoID = t.TorneoID,
                              .Nombre = t.Nombre,
                              .TipoTorneo = t.TipoTorneo,
                              .Categoria = t.Categoria,
                              .Estado = t.Estado,
                              .CiudadID = t.CiudadID,
                              .PaisID = t.PaisID,
                              .RegionID = t.RegionID,
                              .ConfederacionID = t.ConfederacionID
                         }
            Return New JsonResult With {.Data = torneo, .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        Function CargarCiudades() As JsonResult
            Try
                Dim ciudades = From c In db.Ciudad
                               Order By c.Nombre
                               Select New With {
                                   c.CiudadID,
                                   c.Nombre
                               }
                Return New JsonResult With {.Data = ciudades.ToList(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al cargar ciudades: {ex.Message}")
                Return New JsonResult With {.Data = New List(Of Object)(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            End Try
        End Function

        Function CargarPaises() As JsonResult
            Try
                Dim paises = From p In db.Pais
                             Order By p.Nombre
                             Select New With {
                                 p.PaisID,
                                 p.Nombre
                             }
                Return New JsonResult With {.Data = paises.ToList(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al cargar países: {ex.Message}")
                Return New JsonResult With {.Data = New List(Of Object)(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            End Try
        End Function

        Function CargarRegiones() As JsonResult
            Try
                Dim regiones = From r In db.Region
                               Order By r.Nombre
                               Select New With {
                                   r.RegionID,
                                   r.Nombre
                               }
                Return New JsonResult With {.Data = regiones.ToList(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al cargar regiones: {ex.Message}")
                Return New JsonResult With {.Data = New List(Of Object)(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            End Try
        End Function

        Function CargarConfederaciones() As JsonResult
            Try
                Dim confederaciones = From c In db.Confederacion
                                      Order By c.Nombre
                                      Select New With {
                                          c.ConfederacionID,
                                          c.Nombre
                                      }
                Return New JsonResult With {.Data = confederaciones.ToList(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al cargar confederaciones: {ex.Message}")
                Return New JsonResult With {.Data = New List(Of Object)(), .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            End Try
        End Function

        Function CargarPaisesPorConfederacion(confederacionID As Integer) As JsonResult
            Dim paises = From p In db.Pais
                         Where p.ConfederacionID = confederacionID
                         Order By p.Nombre
                         Select New With {.PaisID = p.PaisID, .Nombre = p.Nombre}
            Return Json(paises.ToList(), JsonRequestBehavior.AllowGet)
        End Function

        Function CargarRegionesPorPais(paisID As Integer) As JsonResult
            Dim regiones = From r In db.Region
                           Where r.PaisID = paisID
                           Order By r.Nombre
                           Select New With {.RegionID = r.RegionID, .Nombre = r.Nombre}
            Return Json(regiones.ToList(), JsonRequestBehavior.AllowGet)
        End Function

        Function CargarCiudadesPorRegion(regionID As Integer) As JsonResult
            Dim ciudades = From c In db.Ciudad
                           Where c.RegionID = regionID
                           Order By c.Nombre
                           Select New With {.CiudadID = c.CiudadID, .Nombre = c.Nombre}
            Return Json(ciudades.ToList(), JsonRequestBehavior.AllowGet)
        End Function

        <HttpGet()>
        Function RecuperarTablaPosiciones(torneoID As Integer, año As Integer) As JsonResult
            Try
                Dim equiposParticipantes = From te In db.TorneoEquipo
                                           Join e In db.Equipo On te.EquipoID Equals e.EquipoID
                                           Where te.TorneoID = torneoID And te.AñoParticipacion = año
                                           Select New With {
                                               .EquipoID = e.EquipoID,
                                               .EquipoNombre = e.Nombre
                                           }

                Dim tablaPosiciones = New List(Of PosicionTabla)()
                Dim equiposStats As New Dictionary(Of Integer, PosicionTabla)

                For Each equipoData In equiposParticipantes
                    equiposStats.Add(equipoData.EquipoID, New PosicionTabla With {
                    .Equipo = equipoData.EquipoNombre,
                    .PJ = 0, .PG = 0, .PE = 0, .PP = 0,
                    .GF = 0, .GC = 0, .DG = 0, .PTS = 0
                    })
                Next

                Dim partidosDelTorneo = From p In db.Partido
                                        Join teLocal In db.TorneoEquipo On p.EquipoLocalTorneoEquipoID Equals teLocal.TorneoEquipoID
                                        Join teVisitante In db.TorneoEquipo On p.EquipoVisitanteTorneoEquipoID Equals teVisitante.TorneoEquipoID
                                        Where p.TorneoID = torneoID And p.AñoParticipacion = año And p.Estado = "Finalizado"
                                        Select New With {
                                        .Partido = p,
                                        .EquipoLocalID = teLocal.EquipoID,
                                        .EquipoVisitanteID = teVisitante.EquipoID
                                        }

                For Each partidoData In partidosDelTorneo
                    Dim partido = partidoData.Partido
                    Dim idLocal = partidoData.EquipoLocalID
                    Dim idVisitante = partidoData.EquipoVisitanteID

                    If equiposStats.ContainsKey(idLocal) AndAlso equiposStats.ContainsKey(idVisitante) Then
                        Dim statsLocal = equiposStats(idLocal)
                        Dim statsVisitante = equiposStats(idVisitante)

                        statsLocal.PJ += 1
                        statsVisitante.PJ += 1

                        statsLocal.GF += partido.GolesLocal.GetValueOrDefault()
                        statsLocal.GC += partido.GolesVisitante.GetValueOrDefault()
                        statsVisitante.GF += partido.GolesVisitante.GetValueOrDefault()
                        statsVisitante.GC += partido.GolesLocal.GetValueOrDefault()

                        If partido.GolesLocal > partido.GolesVisitante Then
                            statsLocal.PG += 1
                            statsLocal.PTS += 3
                            statsVisitante.PP += 1
                        ElseIf partido.GolesLocal < partido.GolesVisitante Then
                            statsVisitante.PG += 1
                            statsVisitante.PTS += 3
                            statsLocal.PP += 1
                        Else
                            statsLocal.PE += 1
                            statsLocal.PTS += 1
                            statsVisitante.PE += 1
                            statsVisitante.PTS += 1
                        End If
                    End If
                Next

                For Each kvp In equiposStats
                    kvp.Value.DG = kvp.Value.GF - kvp.Value.GC
                    tablaPosiciones.Add(kvp.Value)
                Next

                Dim posicionesOrdenadas = tablaPosiciones.OrderByDescending(Function(p) p.PTS) _
                                                         .ThenByDescending(Function(p) p.DG) _
                                                         .ThenByDescending(Function(p) p.GF) _
                                                         .ToList()

                Return Json(posicionesOrdenadas, JsonRequestBehavior.AllowGet)

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al recuperar tabla de posiciones: {ex.Message}")
                Return Json(New List(Of PosicionTabla)(), JsonRequestBehavior.AllowGet)
            End Try
        End Function

        Function TablaPosiciones(torneoID As Integer, Optional torneoNombre As String = "") As ActionResult
            ViewBag.TorneoID = torneoID
            ViewBag.TorneoNombre = torneoNombre
            Return View()
        End Function

        <HttpPost()>
        Function SimularPartidos(torneoID As Integer, torneoNombre As String, año As Integer, fase As String) As ActionResult
            Try
                Dim efConnectionString As String = ConfigurationManager.ConnectionStrings("FutbolDB2Entities").ConnectionString
                Dim entityBuilder As New EntityConnectionStringBuilder(efConnectionString)
                Dim sqlConnectionString As String = entityBuilder.ProviderConnectionString

                Using conn As New SqlConnection(sqlConnectionString)
                    Using cmd As New SqlCommand("spSimularTorneoElo", conn)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@TorneoNombre", torneoNombre)
                        cmd.Parameters.AddWithValue("@AñoParticipacion", año)
                        conn.Open()
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                Return Json(New With {.success = True, .message = "Simulación de torneo completa exitosamente."})

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error en SimularPartidos: {ex.Message}")
                If ex.InnerException IsNot Nothing Then
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}")
                End If
                Return Json(New With {.success = False, .message = "Error al simular torneo: " & ex.Message & If(ex.InnerException IsNot Nothing, " (Detalle: " & ex.InnerException.Message & ")", "")})
            End Try
        End Function

    End Class

    Public Class PosicionTabla
        Public Property Equipo As String
        Public Property PJ As Integer
        Public Property PG As Integer
        Public Property PE As Integer
        Public Property PP As Integer
        Public Property GF As Integer
        Public Property GC As Integer
        Public Property DG As Integer
        Public Property PTS As Integer
    End Class

End Namespace