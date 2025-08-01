﻿document.addEventListener("DOMContentLoaded", function () {
    listarPaises();

    document.getElementById("formPais").addEventListener("submit", function (e) {
        e.preventDefault();
        guardarPais();
    });
});

function abrirModalNuevo() {
    document.getElementById("tituloModal").innerText = "Crear País";
    document.getElementById("txtPaisID").value = "0";
    document.getElementById("txtNombre").value = "";
    document.getElementById("txtCodigoFIFA").value = "";
    document.getElementById("selectConfederacion").value = "";
    cargarConfederaciones();
    mostrarModal();
}

function cerrarModal() {
    ocultarModal();
    document.getElementById("formPais").reset();
}

function mostrarModal() {
    const modal = document.getElementById("modalPais");
    const modalContent = document.getElementById("modalPaisContent");
    modal.classList.remove("hidden");
    modal.classList.add("flex");
    setTimeout(() => {
        modalContent.classList.remove("opacity-0", "scale-95");
        modalContent.classList.add("opacity-100", "scale-100");
    }, 10);
}

function ocultarModal() {
    const modal = document.getElementById("modalPais");
    const modalContent = document.getElementById("modalPaisContent");
    modalContent.classList.remove("opacity-100", "scale-100");
    modalContent.classList.add("opacity-0", "scale-95");
    setTimeout(() => {
        modal.classList.remove("flex");
        modal.classList.add("hidden");
    }, 200);
}

function listarPaises() {
    fetch("/Paises/Listar")
        .then(res => res.json())
        .then(data => {
            const tablaBody = document.querySelector("#tablaPaises tbody");
            tablaBody.innerHTML = "";
            let filaPar = true;

            data.data.forEach((p, index) => {
                const claseFila = filaPar ? "table-row-even" : "table-row-odd";
                filaPar = !filaPar;

                const row = document.createElement("tr");
                row.className = `${claseFila} hover:bg-gray-600/50 transition-colors`;

                const tdNombre = document.createElement("td");
                tdNombre.className = "p-4 font-medium";
                tdNombre.textContent = p.Nombre;

                const tdCodigoFIFA = document.createElement("td");
                tdCodigoFIFA.className = "p-4";
                tdCodigoFIFA.textContent = p.CodigoFIFA;

                const tdConfederacion = document.createElement("td");
                tdConfederacion.className = "p-4";
                tdConfederacion.textContent = p.NombreConfederacion;

                const tdAcciones = document.createElement("td");
                tdAcciones.className = "p-4 text-center";
                tdAcciones.innerHTML = `
                    <div class="flex justify-center gap-2">
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Editar" onclick="editarPais(${p.PaisID})">
                            <i data-lucide="edit" class="w-4 h-4 text-yellow-400"></i>
                        </button>
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Eliminar" onclick="eliminarPais(${p.PaisID})">
                            <i data-lucide="trash-2" class="w-4 h-4 text-red-400"></i>
                        </button>
                    </div>
                `;

                row.appendChild(tdNombre);
                row.appendChild(tdCodigoFIFA);
                row.appendChild(tdConfederacion);
                row.appendChild(tdAcciones);
                tablaBody.appendChild(row);
            });

            lucide.createIcons();
        })
        .catch(error => {
            console.error("Error al listar países:", error);
            alert("Error al cargar los países.");
        });
}

function cargarConfederaciones() {
    return fetch("/Paises/GetConfederaciones")
        .then(res => res.json())
        .then(data => {
            const selectConfederacion = document.getElementById("selectConfederacion");
            selectConfederacion.innerHTML = "<option value=''>-- Seleccione Confederación --</option>";
            data.forEach(c => {
                const option = document.createElement("option");
                option.value = c.ConfederacionID;
                option.innerText = c.Nombre;
                selectConfederacion.appendChild(option);
            });
        })
        .catch(error => {
            console.error("Error al cargar confederaciones:", error);
            alert("Error al cargar las confederaciones.");
            return Promise.reject(error);
        });
}

function guardarPais() {
    const pais = {
        PaisID: parseInt(document.getElementById("txtPaisID").value),
        Nombre: document.getElementById("txtNombre").value.trim(),
        CodigoFIFA: document.getElementById("txtCodigoFIFA").value.trim(),
        ConfederacionID: parseInt(document.getElementById("selectConfederacion").value)
    };

    if (pais.Nombre === "") {
        alert("El nombre del país es obligatorio.");
        return;
    }
    if (isNaN(pais.ConfederacionID) || pais.ConfederacionID === 0) {
        alert("Debe seleccionar una Confederación.");
        return;
    }

    fetch("/Paises/Guardar", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(pais)
    })
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al guardar el país."); });
            }
            return res.json();
        })
        .then(data => {
            if (data.success) {
                cerrarModal();
                listarPaises();
                alert("País guardado con éxito!");
            } else {
                alert("Error al guardar el país: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error al guardar país:", error);
            alert("Error al guardar el país: " + error.message);
        });
}

function editarPais(id) {
    cargarConfederaciones().then(() => {
        fetch(`/Paises/Buscar?id=${id}`)
            .then(res => {
                if (!res.ok) {
                    return res.json().then(err => { throw new Error(err.message || "Error al buscar el país para edición."); });
                }
                return res.json();
            })
            .then(p => {
                document.getElementById("tituloModal").innerText = "Editar País";
                document.getElementById("txtPaisID").value = p.PaisID;
                document.getElementById("txtNombre").value = p.Nombre;
                document.getElementById("txtCodigoFIFA").value = p.CodigoFIFA;
                document.getElementById("selectConfederacion").value = p.ConfederacionID;
                mostrarModal();
            })
            .catch(error => {
                console.error("Error al cargar país para edición:", error);
                alert("Error al cargar los datos del país para edición: " + error.message);
            });
    });
}

function eliminarPais(id) {
    if (!confirm("¿Estás seguro de que deseas eliminar este país?")) {
        return;
    }

    fetch(`/Paises/Eliminar?id=${id}`, { method: "POST" })
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al eliminar el país."); });
            }
            return res.json();
        })
        .then(data => {
            if (data.success) {
                alert("País eliminado correctamente!");
                listarPaises();
            } else {
                alert("Error al eliminar el país: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error al eliminar país:", error);
            alert("Error al eliminar el país: " + error.message);
        });
}

document.addEventListener('click', function (event) {
    const modal = document.getElementById('modalPais');
    const content = document.getElementById('modalPaisContent');
    if (event.target === modal && !content.contains(event.target)) {
        cerrarModal();
    }
});

document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape') {
        const modal = document.getElementById('modalPais');
        if (!modal.classList.contains('hidden')) {
            cerrarModal();
        }
    }
});