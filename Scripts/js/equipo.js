document.addEventListener("DOMContentLoaded", () => {
    listarEquipos();
});

// Listar Equipos
function listarEquipos() {
    fetch("/Equipos/Listar")
        .then(res => res.json())
        .then(result => {
            const equipos = result.data;
            const tbody = document.querySelector("#tablaEquipos tbody");
            tbody.innerHTML = "";

            equipos.forEach(e => {
                const tr = document.createElement("tr");
                tr.innerHTML = `
            <td>${e.nombre}</td>
            <td>${e.codigoEquipo}</td>
            <td>${e.tipoEquipo || "-"}</td>
            <td>${e.añoFundacion || "-"}</td>
            <td>${e.elo?.toFixed(2) || "-"}</td>
            <td><span class="px-2 py-1 rounded bg-green-200 text-green-800 text-xs">${e.estado}</span></td>
            <td>${e.ciudad || "-"}</td>
            <td>${e.region || "-"}</td>
            <td>
                <button onclick="editarEquipo(${e.equipoID})" class="text-yellow-600 hover:text-yellow-800 mr-2">✏️</button>
                <button onclick="eliminarEquipo(${e.equipoID})" class="text-red-600 hover:text-red-800">🗑️</button>
            </td>
        `;
                tbody.appendChild(tr);
            });
        });


// Abrir Modal Nuevo
function abrirModalNuevo() {
    document.getElementById("formEquipo").reset();
    document.getElementById("txtEquipoID").value = "";
    document.getElementById("tituloModal").innerText = "Crear Equipo";
    document.getElementById("modalEquipo").classList.remove("hidden");

    cargarComboBoxesJerarquicos(); // Resetea todos
}

// Cerrar Modal
function cerrarModal() {
    document.getElementById("modalEquipo").classList.add("hidden");
}

// Guardar Equipo
document.getElementById("formEquipo").addEventListener("submit", function (e) {
    e.preventDefault();

    const equipo = {
        EquipoID: document.getElementById("txtEquipoID").value || 0,
        Nombre: document.getElementById("txtNombre").value,
        CodigoEquipo: document.getElementById("txtCodigo").value,
        TipoEquipo: document.getElementById("txtTipo").value,
        AñoFundacion: document.getElementById("txtAnho").value,
        ELO: document.getElementById("txtELO").value,
        Estado: document.getElementById("txtEstado").value,
        CiudadID: document.getElementById("txtCiudadID").value,
        RegionID: document.getElementById("txtRegionID").value
    };

    fetch("/Equipos/Guardar", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(equipo)
    })
        .then(res => {
            if (!res.ok) {
                // Si la respuesta no es OK (ej. 400, 500), lanzamos un error
                return res.json().then(err => { throw new Error(err.message || "Error al guardar el equipo."); });
            }
            return res.json();
        })
        .then(() => {
            cerrarModal();
            listarEquipos();
            alert("Equipo guardado con éxito!"); // Mensaje de éxito
        })
        .catch(error => {
            console.error("Error al guardar equipo:", error);
            alert("Error al guardar el equipo: " + error.message); // Muestra el mensaje de error al usuario
        });
});

// Editar Equipo
    function editarEquipo(id) {
        fetch(`/Equipos/Buscar?id=${id}`)
            .then(res => {
                if (!res.ok) {
                    return res.json().then(err => { throw new Error(err.message || "Error al buscar el equipo para edición."); });
                }
                return res.json();
            })
            .then(e => {
                document.getElementById("txtEquipoID").value = e.equipoID;
                document.getElementById("txtNombre").value = e.nombre;
                document.getElementById("txtCodigo").value = e.codigoEquipo;
                document.getElementById("txtTipo").value = e.tipoEquipo || "";
                document.getElementById("txtAnho").value = e.añoFundacion || "";
                document.getElementById("txtELO").value = e.elo || "";
                document.getElementById("txtEstado").value = e.estado;

                // Abrir el modal ANTES de cargar los datos para que los elementos existan
                document.getElementById("tituloModal").innerText = "Editar Equipo";
                document.getElementById("modalEquipo").classList.remove("hidden");
                // Opcional: limpiar los combos para evitar duplicados si ya estaban llenos
                document.getElementById("txtConfederacionID").innerHTML = "<option value=''>Seleccione</option>";
                document.getElementById("txtPaisID").innerHTML = "<option value=''>Seleccione</option>";
                document.getElementById("txtRegionID").innerHTML = "<option value=''>Seleccione</option>";
                document.getElementById("txtCiudadID").innerHTML = "<option value=''>Seleccione</option>";

                // Cargar los combos jerárquicos en cascada, usando los callbacks para asegurar el orden
                cargarConfederaciones(() => {
                    document.getElementById("txtConfederacionID").value = e.confederacionID || "";
                    if (e.confederacionID) {
                        cargarPaises(e.confederacionID, () => {
                            document.getElementById("txtPaisID").value = e.paisID || "";
                            if (e.paisID) {
                                cargarRegiones(e.paisID, () => {
                                    document.getElementById("txtRegionID").value = e.regionID || "";
                                    if (e.regionID) {
                                        cargarCiudades(e.regionID, () => {
                                            document.getElementById("txtCiudadID").value = e.ciudadID || "";
                                        });
                                    }
                                });
                            }
                        });
                    }
                });
            })
            .catch(error => {
                console.error("Error al cargar equipo para edición:", error);
                alert("Error al cargar los datos del equipo para edición: " + error.message);
            });
    }

// Eliminar
    function eliminarEquipo(id) {
        if (!confirm("¿Estás seguro de que deseas eliminar este equipo?")) return;

        fetch(`/Equipos/Eliminar?id=${id}`, { method: "POST" }) // Cambiado a POST
            .then(res => {
                if (!res.ok) {
                    return res.json().then(err => { throw new Error(err.message || "Error al eliminar el equipo."); });
                }
                return res.json(); // O res.text() si el backend devuelve un string
            })
            .then(() => {
                alert("Equipo eliminado correctamente!"); // Mensaje de éxito
                listarEquipos();
            })
            .catch(error => {
                console.error("Error al eliminar equipo:", error);
                alert("Error al eliminar el equipo: " + error.message);
            });
    }

// Jerarquía: Confederación → País → Región → Ciudad
function cargarComboBoxesJerarquicos() {
    fetch("/Equipos/CargarConfederaciones")
        .then(res => res.json())
        .then(confeds => {
            const cbo = document.getElementById("txtConfederacionID");
            cbo.innerHTML = "<option value=''>Seleccione</option>";
            confeds.forEach(c => {
                cbo.innerHTML += `<option value="${c.confederacionID}">${c.nombre}</option>`;
            });
        });

    document.getElementById("txtConfederacionID").addEventListener("change", e => {
        const id = e.target.value;
        cargarPaises(id);
        document.getElementById("txtRegionID").innerHTML = "";
        document.getElementById("txtCiudadID").innerHTML = "";
    });

    document.getElementById("txtPaisID").addEventListener("change", e => {
        cargarRegiones(e.target.value);
        document.getElementById("txtCiudadID").innerHTML = "";
    });

    document.getElementById("txtRegionID").addEventListener("change", e => {
        cargarCiudades(e.target.value);
    });
}

    // (Ya tienes esto, solo asegúrate de que el callback se llame al final)
    function cargarPaises(confID, callback = () => { }) {
        fetch(`/Equipos/CargarPaisesPorConfederacion?id=${confID}`)
            .then(res => res.json())
            .then(paises => {
                const cbo = document.getElementById("txtPaisID");
                cbo.innerHTML = "<option value=''>Seleccione</option>";
                paises.forEach(p => {
                    cbo.innerHTML += `<option value="${p.paisID}">${p.nombre}</option>`;
                });
                callback(); // <-- Aquí se llama el callback
            })
            .catch(error => console.error("Error al cargar países:", error));
    }

    function cargarRegiones(paisID, callback = () => { }) {
        fetch(`/Equipos/CargarRegionesPorPais?id=${paisID}`)
            .then(res => res.json())
            .then(regiones => {
                const cbo = document.getElementById("txtRegionID");
                cbo.innerHTML = "<option value=''>Seleccione</option>";
                regiones.forEach(r => {
                    cbo.innerHTML += `<option value="${r.regionID}">${r.nombre}</option>`;
                });
                callback(); // <-- Aquí se llama el callback
            })
            .catch(error => console.error("Error al cargar regiones:", error));
    }

    function cargarCiudades(regionID, callback = () => { }) {
        fetch(`/Equipos/CargarCiudadesPorRegion?id=${regionID}`)
            .then(res => res.json())
            .then(ciudades => {
                const cbo = document.getElementById("txtCiudadID");
                cbo.innerHTML = "<option value=''>Seleccione</option>";
                ciudades.forEach(c => {
                    cbo.innerHTML += `<option value="${c.ciudadID}">${c.nombre}</option>`;
                });
                callback(); // <-- Aquí se llama el callback
            })
            .catch(error => console.error("Error al cargar ciudades:", error));
    }