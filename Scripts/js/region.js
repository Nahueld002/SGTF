document.addEventListener("DOMContentLoaded", function () {
    listarRegiones();

    document.getElementById("formRegion").addEventListener("submit", function (e) {
        e.preventDefault();
        guardarRegion();
    });
});

function abrirModalNuevo() {
    document.getElementById("tituloModal").innerText = "Crear Región";
    document.getElementById("txtRegionID").value = "0";
    document.getElementById("txtNombre").value = "";
    document.getElementById("txtTipoRegion").value = "";
    document.getElementById("selectPais").value = "";
    cargarPaises();
    mostrarModal();
}

function cerrarModal() {
    ocultarModal();
    document.getElementById("formRegion").reset();
}

function mostrarModal() {
    const modal = document.getElementById("modalRegion");
    const modalContent = document.getElementById("modalRegionContent");
    modal.classList.remove("hidden");
    modal.classList.add("flex");
    setTimeout(() => {
        modalContent.classList.remove("opacity-0", "scale-95");
        modalContent.classList.add("opacity-100", "scale-100");
    }, 10);
}

function ocultarModal() {
    const modal = document.getElementById("modalRegion");
    const modalContent = document.getElementById("modalRegionContent");
    modalContent.classList.remove("opacity-100", "scale-100");
    modalContent.classList.add("opacity-0", "scale-95");
    setTimeout(() => {
        modal.classList.remove("flex");
        modal.classList.add("hidden");
    }, 200);
}

function listarRegiones() {
    fetch("/Region/Listar")
        .then(res => res.json())
        .then(data => {
            const tablaBody = document.querySelector("#tablaRegiones tbody");
            tablaBody.innerHTML = "";
            let filaPar = true;

            data.data.forEach((r, index) => {
                const claseFila = filaPar ? "table-row-even" : "table-row-odd";
                filaPar = !filaPar;

                const row = document.createElement("tr");
                row.className = `${claseFila} hover:bg-gray-600/50 transition-colors`;

                const tdNombre = document.createElement("td");
                tdNombre.className = "p-4 font-medium";
                tdNombre.textContent = r.Nombre;

                const tdTipoRegion = document.createElement("td");
                tdTipoRegion.className = "p-4";
                tdTipoRegion.textContent = r.TipoRegion;

                const tdPais = document.createElement("td");
                tdPais.className = "p-4";
                tdPais.textContent = r.NombrePais;

                const tdAcciones = document.createElement("td");
                tdAcciones.className = "p-4 text-center";
                tdAcciones.innerHTML = `
                    <div class="flex justify-center gap-2">
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Editar" onclick="editarRegion(${r.RegionID})">
                            <i data-lucide="edit" class="w-4 h-4 text-yellow-400"></i>
                        </button>
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Eliminar" onclick="eliminarRegion(${r.RegionID})">
                            <i data-lucide="trash-2" class="w-4 h-4 text-red-400"></i>
                        </button>
                    </div>
                `;

                row.appendChild(tdNombre);
                row.appendChild(tdTipoRegion);
                row.appendChild(tdPais);
                row.appendChild(tdAcciones);
                tablaBody.appendChild(row);
            });

            lucide.createIcons();
        })
        .catch(error => {
            console.error("Error al listar regiones:", error);
            alert("Error al cargar las regiones.");
        });
}

function cargarPaises() {
    return fetch("/Region/GetPaises")
        .then(res => res.json())
        .then(data => {
            const selectPais = document.getElementById("selectPais");
            selectPais.innerHTML = "<option value=''>-- Seleccione País --</option>";
            data.forEach(p => {
                const option = document.createElement("option");
                option.value = p.PaisID;
                option.innerText = p.Nombre;
                selectPais.appendChild(option);
            });
        })
        .catch(error => {
            console.error("Error al cargar países:", error);
            alert("Error al cargar los países.");
            return Promise.reject(error);
        });
}

function guardarRegion() {
    const region = {
        RegionID: parseInt(document.getElementById("txtRegionID").value),
        Nombre: document.getElementById("txtNombre").value.trim(),
        TipoRegion: document.getElementById("txtTipoRegion").value.trim(),
        PaisID: parseInt(document.getElementById("selectPais").value)
    };

    if (region.Nombre === "") {
        alert("El nombre de la región es obligatorio.");
        return;
    }
    if (region.TipoRegion === "") {
        alert("El tipo de región es obligatorio.");
        return;
    }
    if (isNaN(region.PaisID) || region.PaisID === 0) {
        alert("Debe seleccionar un País.");
        return;
    }

    fetch("/Region/Guardar", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(region)
    })
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al guardar la región."); });
            }
            return res.json();
        })
        .then(data => {
            if (data.success) {
                cerrarModal();
                listarRegiones();
                alert("Región guardada con éxito!");
            } else {
                alert("Error al guardar la región: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error al guardar región:", error);
            alert("Error al guardar la región: " + error.message);
        });
}

function editarRegion(id) {
    cargarPaises().then(() => {
        fetch(`/Region/Buscar?id=${id}`)
            .then(res => {
                if (!res.ok) {
                    return res.json().then(err => { throw new Error(err.message || "Error al buscar la región para edición."); });
                }
                return res.json();
            })
            .then(r => {
                document.getElementById("tituloModal").innerText = "Editar Región";
                document.getElementById("txtRegionID").value = r.RegionID;
                document.getElementById("txtNombre").value = r.Nombre;
                document.getElementById("txtTipoRegion").value = r.TipoRegion;
                document.getElementById("selectPais").value = r.PaisID;
                mostrarModal();
            })
            .catch(error => {
                console.error("Error al cargar región para edición:", error);
                alert("Error al cargar los datos de la región para edición: " + error.message);
            });
    });
}

function eliminarRegion(id) {
    if (!confirm("¿Estás seguro de que deseas eliminar esta región?")) {
        return;
    }

    fetch(`/Region/Eliminar?id=${id}`, { method: "POST" })
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al eliminar la región."); });
            }
            return res.json();
        })
        .then(data => {
            if (data.success) {
                alert("Región eliminada correctamente!");
                listarRegiones();
            } else {
                alert("Error al eliminar la región: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error al eliminar región:", error);
            alert("Error al eliminar la región: " + error.message);
        });
}

document.addEventListener('click', function (event) {
    const modal = document.getElementById('modalRegion');
    const content = document.getElementById('modalRegionContent');
    if (event.target === modal && !content.contains(event.target)) {
        cerrarModal();
    }
});

document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape') {
        const modal = document.getElementById('modalRegion');
        if (!modal.classList.contains('hidden')) {
            cerrarModal();
        }
    }
});