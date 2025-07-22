document.addEventListener("DOMContentLoaded", function () {
    listarCiudades();

    document.getElementById("formCiudad").addEventListener("submit", function (e) {
        e.preventDefault();
        guardarCiudad();
    });
});

function abrirModalNuevo() {
    document.getElementById("tituloModal").innerText = "Crear Ciudad";
    document.getElementById("txtCiudadID").value = "0";
    document.getElementById("txtNombre").value = "";
    document.getElementById("chkEsCapital").checked = false;
    document.getElementById("selectRegion").value = "";
    cargarRegiones();
    mostrarModal();
}

function cerrarModal() {
    ocultarModal();
    document.getElementById("formCiudad").reset();
}

function mostrarModal() {
    const modal = document.getElementById("modalCiudad");
    const modalContent = document.getElementById("modalCiudadContent");
    modal.classList.remove("hidden");
    modal.classList.add("flex");
    setTimeout(() => {
        modalContent.classList.remove("opacity-0", "scale-95");
        modalContent.classList.add("opacity-100", "scale-100");
    }, 10);
}

function ocultarModal() {
    const modal = document.getElementById("modalCiudad");
    const modalContent = document.getElementById("modalCiudadContent");
    modalContent.classList.remove("opacity-100", "scale-100");
    modalContent.classList.add("opacity-0", "scale-95");
    setTimeout(() => {
        modal.classList.remove("flex");
        modal.classList.add("hidden");
    }, 200);
}

function listarCiudades() {
    fetch("/Ciudad/Listar")
        .then(res => res.json())
        .then(data => {
            const tablaBody = document.querySelector("#tablaCiudades tbody");
            tablaBody.innerHTML = "";
            let filaPar = true;

            data.data.forEach((c, index) => {
                const claseFila = filaPar ? "table-row-even" : "table-row-odd";
                filaPar = !filaPar;

                const row = document.createElement("tr");
                row.className = `${claseFila} hover:bg-gray-600/50 transition-colors`;

                const tdNombre = document.createElement("td");
                tdNombre.className = "p-4 font-medium";
                tdNombre.textContent = c.Nombre;

                const tdRegion = document.createElement("td");
                tdRegion.className = "p-4";
                tdRegion.textContent = c.NombreRegion;

                const tdEsCapital = document.createElement("td");
                tdEsCapital.className = "p-4 text-center";
                tdEsCapital.textContent = c.EsCapital ? "Sí" : "No";

                const tdAcciones = document.createElement("td");
                tdAcciones.className = "p-4 text-center";
                tdAcciones.innerHTML = `
                    <div class="flex justify-center gap-2">
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Editar" onclick="editarCiudad(${c.CiudadID})">
                            <i data-lucide="edit" class="w-4 h-4 text-yellow-400"></i>
                        </button>
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Eliminar" onclick="eliminarCiudad(${c.CiudadID})">
                            <i data-lucide="trash-2" class="w-4 h-4 text-red-400"></i>
                        </button>
                    </div>
                `;

                row.appendChild(tdNombre);
                row.appendChild(tdRegion);
                row.appendChild(tdEsCapital);
                row.appendChild(tdAcciones);
                tablaBody.appendChild(row);
            });

            lucide.createIcons();
        })
        .catch(error => {
            console.error("Error al listar ciudades:", error);
            alert("Error al cargar las ciudades.");
        });
}

function cargarRegiones() {
    return fetch("/Ciudad/GetRegiones")
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

function guardarCiudad() {
    const ciudad = {
        CiudadID: parseInt(document.getElementById("txtCiudadID").value),
        Nombre: document.getElementById("txtNombre").value.trim(),
        RegionID: parseInt(document.getElementById("selectRegion").value),
        EsCapital: document.getElementById("chkEsCapital").checked
    };

    if (ciudad.Nombre === "") {
        alert("El nombre de la ciudad es obligatorio.");
        return;
    }
    if (isNaN(ciudad.RegionID) || ciudad.RegionID === 0) {
        alert("Debe seleccionar una Región.");
        return;
    }

    fetch("/Ciudad/Guardar", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(ciudad)
    })
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al guardar la ciudad."); });
            }
            return res.json();
        })
        .then(data => {
            if (data.success) {
                cerrarModal();
                listarCiudades();
                alert("Ciudad guardada con éxito!");
            } else {
                alert("Error al guardar la ciudad: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error al guardar ciudad:", error);
            alert("Error al guardar la ciudad: " + error.message);
        });
}

function editarCiudad(id) {
    cargarRegiones().then(() => {
        fetch(`/Ciudad/Buscar?id=${id}`)
            .then(res => {
                if (!res.ok) {
                    return res.json().then(err => { throw new Error(err.message || "Error al buscar la ciudad para edición."); });
                }
                return res.json();
            })
            .then(c => {
                document.getElementById("tituloModal").innerText = "Editar Ciudad";
                document.getElementById("txtCiudadID").value = c.CiudadID;
                document.getElementById("txtNombre").value = c.Nombre;
                document.getElementById("selectRegion").value = c.RegionID;
                document.getElementById("chkEsCapital").checked = c.EsCapital;
                mostrarModal();
            })
            .catch(error => {
                console.error("Error al cargar ciudad para edición:", error);
                alert("Error al cargar los datos de la ciudad para edición: " + error.message);
            });
    });
}

function eliminarCiudad(id) {
    if (!confirm("¿Estás seguro de que deseas eliminar esta ciudad?")) {
        return;
    }

    fetch(`/Ciudad/Eliminar?id=${id}`, { method: "POST" })
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al eliminar la ciudad."); });
            }
            return res.json();
        })
        .then(data => {
            if (data.success) {
                alert("Ciudad eliminada correctamente!");
                listarCiudades();
            } else {
                alert("Error al eliminar la ciudad: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error al eliminar ciudad:", error);
            alert("Error al eliminar la ciudad: " + error.message);
        });
}

document.addEventListener('click', function (event) {
    const modal = document.getElementById('modalCiudad');
    const content = document.getElementById('modalCiudadContent');
    if (event.target === modal && !content.contains(event.target)) {
        cerrarModal();
    }
});

document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape') {
        const modal = document.getElementById('modalCiudad');
        if (!modal.classList.contains('hidden')) {
            cerrarModal();
        }
    }
});