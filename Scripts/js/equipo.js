document.addEventListener("DOMContentLoaded", function () {
    listarEquipos();

    document.getElementById("formEquipo").addEventListener("submit", function (e) {
        e.preventDefault();
        guardarEquipo();
    });

    document.getElementById("selectRegion").addEventListener("change", function () {
        const regionID = this.value;
        cargarCiudades(regionID);
    });
});

function abrirModalNuevo() {
    document.getElementById("tituloModal").innerText = "Crear Equipo";
    document.getElementById("txtEquipoID").value = "0";
    document.getElementById("txtNombre").value = "";
    document.getElementById("txtCodigoEquipo").value = "";
    document.getElementById("selectRegion").value = "";
    document.getElementById("selectCiudad").innerHTML = "<option value=''>-- Seleccione Ciudad --</option>";
    document.getElementById("txtAnoFundacion").value = "";
    document.getElementById("txtELO").value = "1000.00";
    document.getElementById("selectTipoEquipo").value = "";
    document.getElementById("selectEstado").value = "Activo";

    cargarRegiones();
    mostrarModal();
}

function cerrarModal() {
    ocultarModal();
    document.getElementById("formEquipo").reset();
}

function mostrarModal() {
    const modal = document.getElementById("modalEquipo");
    const modalContent = document.getElementById("modalEquipoContent");
    modal.classList.remove("hidden");
    modal.classList.add("flex");
    setTimeout(() => {
        modalContent.classList.remove("opacity-0", "scale-95");
        modalContent.classList.add("opacity-100", "scale-100");
    }, 10);
}

function ocultarModal() {
    const modal = document.getElementById("modalEquipo");
    const modalContent = document.getElementById("modalEquipoContent");
    modalContent.classList.remove("opacity-100", "scale-100");
    modalContent.classList.add("opacity-0", "scale-95");
    setTimeout(() => {
        modal.classList.remove("flex");
        modal.classList.add("hidden");
    }, 200);
}

function listarEquipos() {
    fetch("/Equipos/Listar") // Corrected URL
        .then(res => res.json())
        .then(data => {
            const tablaBody = document.querySelector("#tablaEquipos tbody");
            tablaBody.innerHTML = "";
            let filaPar = true;

            data.data.forEach((e, index) => {
                const claseFila = filaPar ? "table-row-even" : "table-row-odd";
                filaPar = !filaPar;

                const row = document.createElement("tr");
                row.className = `${claseFila} hover:bg-gray-600/50 transition-colors`;

                const tdNombre = document.createElement("td");
                tdNombre.className = "p-4 font-medium";
                tdNombre.textContent = e.Nombre;

                const tdCodigoEquipo = document.createElement("td");
                tdCodigoEquipo.className = "p-4";
                tdCodigoEquipo.textContent = e.CodigoEquipo;

                const tdRegion = document.createElement("td");
                tdRegion.className = "p-4";
                tdRegion.textContent = e.NombreRegion || "N/A";

                const tdCiudad = document.createElement("td");
                tdCiudad.className = "p-4";
                tdCiudad.textContent = e.NombreCiudad || "N/A";

                const tdAnoFundacion = document.createElement("td");
                tdAnoFundacion.className = "p-4 text-center";
                tdAnoFundacion.textContent = e.AñoFundacion || "N/A";

                const tdELO = document.createElement("td");
                tdELO.className = "p-4 text-center";
                tdELO.textContent = e.ELO !== null ? e.ELO.toFixed(2) : "N/A";

                const tdTipoEquipo = document.createElement("td");
                tdTipoEquipo.className = "p-4";
                tdTipoEquipo.textContent = e.TipoEquipo;

                const tdEstado = document.createElement("td");
                tdEstado.className = "p-4";
                tdEstado.textContent = e.Estado;

                const tdAcciones = document.createElement("td");
                tdAcciones.className = "p-4 text-center";
                tdAcciones.innerHTML = `
                    <div class="flex justify-center gap-2">
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Editar" onclick="editarEquipo(${e.EquipoID})">
                            <i data-lucide="edit" class="w-4 h-4 text-yellow-400"></i>
                        </button>
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Eliminar" onclick="eliminarEquipo(${e.EquipoID})">
                            <i data-lucide="trash-2" class="w-4 h-4 text-red-400"></i>
                        </button>
                    </div>
                `;

                row.appendChild(tdNombre);
                row.appendChild(tdCodigoEquipo);
                row.appendChild(tdRegion);
                row.appendChild(tdCiudad);
                row.appendChild(tdAnoFundacion);
                row.appendChild(tdELO);
                row.appendChild(tdTipoEquipo);
                row.appendChild(tdEstado);
                row.appendChild(tdAcciones);
                tablaBody.appendChild(row);
            });

            lucide.createIcons();
        })
        .catch(error => {
            console.error("Error al listar equipos:", error);
            alert("Error al cargar los equipos.");
        });
}

function cargarRegiones() {
    return fetch("/Equipos/GetRegiones") // Corrected URL
        .then(res => res.json())
        .then(data => {
            const selectRegion = document.getElementById("selectRegion");
            selectRegion.innerHTML = "<option value=''>-- Seleccione Región --</option>";
            data.forEach(r => {
                const option = document.createElement("option");
                option.value = r.RegionID;
                option.innerText = r.Nombre;
                selectRegion.appendChild(option);
            });
        })
        .catch(error => {
            console.error("Error al cargar regiones:", error);
            alert("Error al cargar las regiones.");
            return Promise.reject(error);
        });
}

function cargarCiudades(regionId) {
    const selectCiudad = document.getElementById("selectCiudad");
    selectCiudad.innerHTML = "<option value=''>-- Seleccione Ciudad --</option>";

    if (!regionId) {
        return Promise.resolve();
    }

    return fetch(`/Equipos/GetCiudades?regionId=${regionId}`) // Corrected URL
        .then(res => res.json())
        .then(data => {
            data.forEach(c => {
                const option = document.createElement("option");
                option.value = c.CiudadID;
                option.innerText = c.Nombre;
                selectCiudad.appendChild(option);
            });
        })
        .catch(error => {
            console.error("Error al cargar ciudades:", error);
            alert("Error al cargar las ciudades.");
            return Promise.reject(error);
        });
}

function guardarEquipo() {
    const equipo = {
        EquipoID: parseInt(document.getElementById("txtEquipoID").value),
        Nombre: document.getElementById("txtNombre").value.trim(),
        CodigoEquipo: document.getElementById("txtCodigoEquipo").value.trim(),
        RegionID: parseInt(document.getElementById("selectRegion").value) || null,
        CiudadID: parseInt(document.getElementById("selectCiudad").value) || null,
        AñoFundacion: parseInt(document.getElementById("txtAnoFundacion").value) || null,
        ELO: parseFloat(document.getElementById("txtELO").value) || null,
        TipoEquipo: document.getElementById("selectTipoEquipo").value,
        Estado: document.getElementById("selectEstado").value
    };

    if (equipo.Nombre === "") {
        alert("El nombre del equipo es obligatorio.");
        return;
    }
    if (equipo.CodigoEquipo === "") {
        alert("El código del equipo es obligatorio.");
        return;
    }
    if (equipo.TipoEquipo === "") {
        alert("El tipo de equipo es obligatorio.");
        return;
    }
    if (equipo.Estado === "") {
        alert("El estado del equipo es obligatorio.");
        return;
    }
    if (equipo.ELO !== null && (isNaN(equipo.ELO) || equipo.ELO <= 0)) {
        alert("El ELO debe ser un número positivo.");
        return;
    }
    if (equipo.AñoFundacion !== null && (isNaN(equipo.AñoFundacion) || equipo.AñoFundacion <= 0)) {
        alert("El año de fundación debe ser un número positivo.");
        return;
    }

    fetch("/Equipos/Guardar", { // Corrected URL
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(equipo)
    })
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al guardar el equipo."); });
            }
            return res.json();
        })
        .then(data => {
            if (data.success) {
                cerrarModal();
                listarEquipos();
                alert("Equipo guardado con éxito!");
            } else {
                alert("Error al guardar el equipo: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error al guardar equipo:", error);
            alert("Error al guardar el equipo: " + error.message);
        });
}

function editarEquipo(id) {
    cargarRegiones()
        .then(() => {
            fetch(`/Equipos/Buscar?id=${id}`) // Corrected URL
                .then(res => {
                    if (!res.ok) {
                        return res.json().then(err => { throw new Error(err.message || "Error al buscar el equipo para edición."); });
                    }
                    return res.json();
                })
                .then(e => {
                    document.getElementById("tituloModal").innerText = "Editar Equipo";
                    document.getElementById("txtEquipoID").value = e.EquipoID;
                    document.getElementById("txtNombre").value = e.Nombre;
                    document.getElementById("txtCodigoEquipo").value = e.CodigoEquipo;
                    document.getElementById("selectRegion").value = e.RegionID || "";
                    return cargarCiudades(e.RegionID).then(() => {
                        document.getElementById("selectCiudad").value = e.CiudadID || "";
                    });
                })
                .then(() => {
                    const equipoData = document.getElementById("txtEquipoID").value;
                    fetch(`/Equipos/Buscar?id=${equipoData}`) // Corrected URL
                        .then(res => res.json())
                        .then(e => {
                            document.getElementById("txtAnoFundacion").value = e.AñoFundacion || "";
                            document.getElementById("txtELO").value = e.ELO !== null ? e.ELO.toFixed(2) : "";
                            document.getElementById("selectTipoEquipo").value = e.TipoEquipo || "";
                            document.getElementById("selectEstado").value = e.Estado || "Activo";
                            mostrarModal();
                        });
                })
                .catch(error => {
                    console.error("Error al cargar equipo para edición:", error);
                    alert("Error al cargar los datos del equipo para edición: " + error.message);
                });
        });
}

function eliminarEquipo(id) {
    if (!confirm("¿Estás seguro de que deseas eliminar este equipo?")) {
        return;
    }

    fetch(`/Equipos/Eliminar?id=${id}`, { method: "POST" }) // Corrected URL
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al eliminar el equipo."); });
            }
            return res.json();
        })
        .then(data => {
            if (data.success) {
                alert("Equipo eliminado correctamente!");
                listarEquipos();
            } else {
                alert("Error al eliminar el equipo: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error al eliminar equipo:", error);
            alert("Error al eliminar el equipo: " + error.message);
        });
}

document.addEventListener('click', function (event) {
    const modal = document.getElementById('modalEquipo');
    const content = document.getElementById('modalEquipoContent');
    if (event.target === modal && !content.contains(event.target)) {
        cerrarModal();
    }
});

document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape') {
        const modal = document.getElementById('modalEquipo');
        if (!modal.classList.contains('hidden')) {
            cerrarModal();
        }
    }
});