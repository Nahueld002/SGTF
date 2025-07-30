const filtrosEquipos = {
    nombre: '',
    codigo: '',
    tipo: '',
    estado: '',
    region: '',
    ciudad: ''
};

document.addEventListener("DOMContentLoaded", function () {
    $(document).ready(function () {
        listarEquipos();
        cargarRegionesEnFiltro();
        cargarRegionesParaFormulario();

        document.getElementById('filter-nombreEquipo').addEventListener('input', () => listarEquipos());
        document.getElementById('filter-codigoEquipo').addEventListener('input', () => listarEquipos());
        document.getElementById('filter-tipoEquipo').addEventListener('change', () => listarEquipos());
        document.getElementById('filter-estadoEquipo').addEventListener('change', () => listarEquipos());

        document.getElementById('filter-regionEquipo').addEventListener('change', function () {
            const regionID = this.value;
            cargarCiudadesEnFiltro(regionID);
            listarEquipos();
        });

        document.getElementById('filter-ciudadEquipo').addEventListener('change', () => listarEquipos());

        const btnAbrirModalNuevo = document.getElementById('btnAbrirModalNuevoEquipo');
        if (btnAbrirModalNuevo) {
            btnAbrirModalNuevo.addEventListener('click', abrirModalNuevo);
        }

        document.getElementById("formEquipo").addEventListener("submit", function (event) {
            event.preventDefault();
            guardarEquipo();
        });

        document.getElementById("selectRegion").addEventListener("change", function () {
            const regionID = this.value;
            cargarCiudades(regionID);
        });

        document.addEventListener('click', function (event) {
            const modal = document.getElementById('modalEquipo');
            const modalContent = document.getElementById('modalEquipoContent');
            if (event.target === modal && !modalContent.contains(event.target)) {
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

    cargarRegionesParaFormulario();
    mostrarModal();
}

function cerrarModal() {
    ocultarModal();
    document.getElementById("formEquipo").reset();
    document.getElementById("selectCiudad").innerHTML = "<option value=''>-- Seleccione Ciudad --</option>";
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
    const nombre = document.getElementById('filter-nombreEquipo').value.trim();
    const codigo = document.getElementById('filter-codigoEquipo').value.trim();
    const tipo = document.getElementById('filter-tipoEquipo').value;
    const estado = document.getElementById('filter-estadoEquipo').value;
    const regionId = document.getElementById('filter-regionEquipo').value;
    const ciudadId = document.getElementById('filter-ciudadEquipo').value;

    const queryParams = new URLSearchParams();
    if (nombre) queryParams.append('nombre', nombre);
    if (codigo) queryParams.append('codigo', codigo);
    if (tipo) queryParams.append('tipo', tipo);
    if (estado) queryParams.append('estado', estado);
    if (regionId) queryParams.append('regionId', regionId);
    if (ciudadId) queryParams.append('ciudadId', ciudadId);

    fetch(`/Equipos/Listar?${queryParams.toString()}`)
        .then(res => res.json())
        .then(response => {
            if (response.success) {
                const equipos = response.data && Array.isArray(response.data) ? response.data : [];
                crearListadoEquipos(equipos);
            } else {
                console.error("Error del servidor al listar equipos:", response.message);
                const tablaBody = document.querySelector("#tablaEquipos tbody");
                if (tablaBody) {
                    tablaBody.innerHTML = `<tr><td colspan="9" class="p-4 text-center text-red-400">Error al cargar los equipos: ${response.message}</td></tr>`;
                }
            }
        })
        .catch(error => {
            console.error("Error de red o procesamiento al listar equipos:", error);
            const tablaBody = document.querySelector("#tablaEquipos tbody");
            if (tablaBody) {
                tablaBody.innerHTML = '<tr><td colspan="9" class="p-4 text-center text-red-400">Error de conexión al cargar los equipos.</td></tr>';
            }
        });
}

function crearListadoEquipos(data) {
    const tablaBody = document.querySelector("#tablaEquipos tbody");
    if (!tablaBody) {
        console.error("Error: Elemento <tbody> con ID #tablaEquipos tbody no encontrado.");
        return;
    }
    tablaBody.innerHTML = "";
    let filaPar = true;

    if (data && Array.isArray(data) && data.length > 0) {
        data.forEach(e => {
            const claseFila = filaPar ? "table-row-even" : "table-row-odd";
            filaPar = !filaPar;

            const row = `
                <tr class="${claseFila} hover:bg-gray-600/50 transition-colors">
                    <td class="p-4 font-medium text-white">${e.Nombre || ''}</td>
                    <td class="p-4 text-white">${e.CodigoEquipo || ''}</td>
                    <td class="p-4 text-white">${e.NombreRegion || 'N/A'}</td>
                    <td class="p-4 text-white">${e.NombreCiudad || 'N/A'}</td>
                    <td class="p-4 text-center text-white">${e.AñoFundacion || 'N/A'}</td>
                    <td class="p-4 text-center text-white">${e.ELO !== null ? e.ELO.toFixed(2) : 'N/A'}</td>
                    <td class="p-4 text-white">${e.TipoEquipo || ''}</td>
                    <td class="p-4 text-white">${e.Estado || ''}</td>
                    <td class="p-4 text-center">
                        <div class="flex justify-center gap-2">
                            <button class="p-2 rounded-md hover:bg-gray-600" title="Editar" onclick="editarEquipo(${e.EquipoID})">
                                <i data-lucide="edit" class="w-4 h-4 text-yellow-400"></i>
                            </button>
                            <button class="p-2 rounded-md hover:bg-gray-600" title="Eliminar" onclick="eliminarEquipo(${e.EquipoID})">
                                <i data-lucide="trash-2" class="w-4 h-4 text-red-400"></i>
                            </button>
                        </div>
                    </td>
                </tr>
            `;
            tablaBody.insertAdjacentHTML('beforeend', row);
        });
        if (typeof lucide !== 'undefined' && lucide.createIcons) {
            lucide.createIcons();
        }
    } else {
        tablaBody.innerHTML = '<tr><td colspan="9" class="px-6 py-4 text-center text-gray-400">No se encontraron equipos que coincidan con los filtros.</td></tr>';
    }
}

function cargarRegionesEnFiltro() {
    return fetch("/Equipos/GetRegiones")
        .then(res => res.json())
        .then(data => {
            const selectRegionFiltro = document.getElementById("filter-regionEquipo");
            selectRegionFiltro.innerHTML = "<option value=''>Todas las Regiones</option>";
            data.forEach(r => {
                const option = document.createElement("option");
                option.value = r.RegionID;
                option.innerText = r.Nombre;
                selectRegionFiltro.appendChild(option);
            });
        })
        .catch(error => {
            console.error("Error al cargar regiones para filtro:", error);
            alert("Error al cargar las regiones para el filtro.");
            return Promise.reject(error);
        });
}

function cargarCiudadesEnFiltro(regionId) {
    const selectCiudadFiltro = document.getElementById("filter-ciudadEquipo");
    selectCiudadFiltro.innerHTML = "<option value=''>Todas las Ciudades</option>";

    if (!regionId) {
        return Promise.resolve();
    }

    return fetch(`/Equipos/GetCiudades?regionId=${regionId}`)
        .then(res => res.json())
        .then(data => {
            data.forEach(c => {
                const option = document.createElement("option");
                option.value = c.CiudadID;
                option.innerText = c.Nombre;
                selectCiudadFiltro.appendChild(option);
            });
        })
        .catch(error => {
            console.error("Error al cargar ciudades para filtro:", error);
            alert("Error al cargar las ciudades para el filtro.");
            return Promise.reject(error);
        });
}

function cargarRegionesParaFormulario() {
    return fetch("/Equipos/GetRegiones")
        .then(res => res.json())
        .then(data => {
            const selectRegionForm = document.getElementById("selectRegion");
            selectRegionForm.innerHTML = "<option value=''>-- Seleccione Región --</option>";
            data.forEach(r => {
                const option = document.createElement("option");
                option.value = r.RegionID;
                option.innerText = r.Nombre;
                selectRegionForm.appendChild(option);
            });
        })
        .catch(error => {
            console.error("Error al cargar regiones para formulario:", error);
            alert("Error al cargar las regiones para el formulario.");
            return Promise.reject(error);
        });
}

function cargarCiudades(regionId) {
    const selectCiudad = document.getElementById("selectCiudad");
    selectCiudad.innerHTML = "<option value=''>-- Seleccione Ciudad --</option>";

    if (!regionId) {
        return Promise.resolve();
    }

    return fetch(`/Equipos/GetCiudades?regionId=${regionId}`)
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

    fetch("/Equipos/Guardar", {
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

window.editarEquipo = function (id) {
    cargarRegionesParaFormulario()
        .then(() => {
            fetch(`/Equipos/Buscar?id=${id}`)
                .then(res => {
                    if (!res.ok) {
                        return res.json().then(err => { throw new Error(err.message || "Error al buscar el equipo para edición."); });
                    }
                    return res.json();
                })
                .then(e => {
                    if (e.success === false) {
                        throw new Error(e.message || "Equipo no encontrado.");
                    }

                    document.getElementById("tituloModal").innerText = "Editar Equipo";
                    document.getElementById("txtEquipoID").value = e.EquipoID;
                    document.getElementById("txtNombre").value = e.Nombre;
                    document.getElementById("txtCodigoEquipo").value = e.CodigoEquipo;
                    document.getElementById("selectRegion").value = e.RegionID || "";

                    return cargarCiudades(e.RegionID).then(() => {
                        document.getElementById("selectCiudad").value = e.CiudadID || "";
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

window.eliminarEquipo = function (id) {
    if (!confirm("¿Estás seguro de que deseas eliminar este equipo?")) {
        return;
    }

    fetch(`/Equipos/Eliminar?id=${id}`, { method: "POST" })
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