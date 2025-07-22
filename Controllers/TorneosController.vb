Imports System.Web.Mvc
Namespace Controllers
    Public Class TorneoController
        Inherits Controller
        Dim db As FutbolDB2Entities = New FutbolDB2Entities()

        ' GET: Torneo
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
                              .Ciudad = If(t.Ciudad IsNot Nothing, t.Ciudad.Nombre, "Regional")
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
                             t.TorneoID,
                             t.Nombre,
                             t.TipoTorneo,
                             t.Categoria,
                             t.Estado,
                             t.CiudadID,
                             t.PaisID,
                             t.RegionID,
                             t.ConfederacionID
                         }
            Return New JsonResult With {.Data = torneo, .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
        End Function

        ' ============== MÉTODOS PARA CARGAR COMBOBOXES ==============
        ' Cargar todas las ciudades
        Function CargarCiudades() As JsonResult
            Try
                Dim ciudades = From c In db.Ciudad
                               Order By c.Nombre
                               Select New With {
                           c.CiudadID,
                           c.Nombre
                       }

                Dim listaciudades = ciudades.ToList()
                System.Diagnostics.Debug.WriteLine($"Ciudades encontradas: {listaciudades.Count}")

                Return New JsonResult With {
            .Data = listaciudades,
            .JsonRequestBehavior = JsonRequestBehavior.AllowGet
        }
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al cargar ciudades: {ex.Message}")
                Return New JsonResult With {
            .Data = New List(Of Object)(),
            .JsonRequestBehavior = JsonRequestBehavior.AllowGet
        }
            End Try
        End Function

        ' Cargar todos los países
        Function CargarPaises() As JsonResult
            Try
                Dim paises = From p In db.Pais
                             Order By p.Nombre
                             Select New With {
                         p.PaisID,
                         p.Nombre
                     }

                Dim listapaises = paises.ToList()
                System.Diagnostics.Debug.WriteLine($"Países encontrados: {listapaises.Count}")

                Return New JsonResult With {
            .Data = listapaises,
            .JsonRequestBehavior = JsonRequestBehavior.AllowGet
        }
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al cargar países: {ex.Message}")
                Return New JsonResult With {
            .Data = New List(Of Object)(),
            .JsonRequestBehavior = JsonRequestBehavior.AllowGet
        }
            End Try
        End Function

        ' Cargar todas las regiones
        Function CargarRegiones() As JsonResult
            Try
                Dim regiones = From r In db.Region
                               Order By r.Nombre
                               Select New With {
                                r.RegionID,
                                r.Nombre
                       }

                Dim listaregiones = regiones.ToList()
                System.Diagnostics.Debug.WriteLine($"Regiones encontradas: {listaregiones.Count}")

                Return New JsonResult With {
            .Data = listaregiones,
            .JsonRequestBehavior = JsonRequestBehavior.AllowGet
        }
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al cargar regiones: {ex.Message}")
                Return New JsonResult With {
            .Data = New List(Of Object)(),
            .JsonRequestBehavior = JsonRequestBehavior.AllowGet
        }
            End Try
        End Function

        ' Cargar todas las confederaciones
        Function CargarConfederaciones() As JsonResult
            Try
                Dim confederaciones = From c In db.Confederacion
                                      Order By c.Nombre
                                      Select New With {
                                    c.ConfederacionID,
                                    c.Nombre
                              }

                Dim listaconfederaciones = confederaciones.ToList()
                System.Diagnostics.Debug.WriteLine($"Confederaciones encontradas: {listaconfederaciones.Count}")

                Return New JsonResult With {
            .Data = listaconfederaciones,
            .JsonRequestBehavior = JsonRequestBehavior.AllowGet
        }
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Error al cargar confederaciones: {ex.Message}")
                Return New JsonResult With {
            .Data = New List(Of Object)(),
            .JsonRequestBehavior = JsonRequestBehavior.AllowGet
        }
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
    End Class
End Namespace