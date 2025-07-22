// En Scripts/js/confederacion.js

document.addEventListener("DOMContentLoaded", function () {
    listarConfederaciones(); // Carga las confederaciones al cargar la página

    // Manejar el envío del formulario
    document.getElementById("formConfederacion").addEventListener("submit", function (e) {
        e.preventDefault(); // Previene el envío tradicional del formulario
        guardarConfederacion();
    });
});

// Función para abrir el modal en modo "Crear Nuevo"
function abrirModalNuevo() {
    document.getElementById("tituloModal").innerText = "Crear Confederación";
    document.getElementById("txtConfederacionID").value = "0";
    document.getElementById("txtNombre").value = "";

    const modal = document.getElementById("modalConfederacion");
    const content = document.getElementById("modalConfederacionContent");

    modal.classList.remove("hidden");
    modal.classList.add("flex");

    setTimeout(() => {
        content.classList.remove("scale-95", "opacity-0");
        content.classList.add("scale-100", "opacity-100");
    }, 10);
}

function cerrarModal() {
    const modal = document.getElementById("modalConfederacion");
    const content = document.getElementById("modalConfederacionContent");

    content.classList.add("scale-95", "opacity-0");
    content.classList.remove("scale-100", "opacity-100");

    setTimeout(() => {
        modal.classList.add("hidden");
        modal.classList.remove("flex");
        document.getElementById("formConfederacion").reset();
    }, 200);
}


// Función para cerrar el modal
function cerrarModal() {
    document.getElementById("modalConfederacion").classList.add("hidden");
    document.getElementById("modalConfederacion").classList.remove("flex");
    document.getElementById("formConfederacion").reset(); // Resetear el formulario
}

// Función para listar confederaciones en la tabla
function listarConfederaciones() {
    fetch("/Confederaciones/Listar")
        .then(res => res.json())
        .then(data => {
            const tablaBody = document.querySelector("#tablaConfederaciones tbody");
            tablaBody.innerHTML = "";
            let filaPar = true;

            data.data.forEach(c => {
                const claseFila = filaPar ? "table-row-even" : "table-row-odd";
                filaPar = !filaPar;

                const row = document.createElement("tr");
                row.className = `${claseFila} hover:bg-gray-600/50 transition-colors`;

                const tdNombre = document.createElement("td");
                tdNombre.className = "p-4 font-medium";
                tdNombre.textContent = c.Nombre;

                const tdAcciones = document.createElement("td");
                tdAcciones.className = "p-4 text-center";
                tdAcciones.innerHTML = `
                    <div class="flex justify-center gap-2">
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Editar" onclick="editarConfederacion(${c.ConfederacionID})">
                            <i data-lucide="edit" class="w-4 h-4 text-yellow-400"></i>
                        </button>
                        <button class="p-2 rounded-md hover:bg-gray-600" title="Eliminar" onclick="eliminarConfederacion(${c.ConfederacionID})">
                            <i data-lucide="trash-2" class="w-4 h-4 text-red-400"></i>
                        </button>
                    </div>
                `;

                row.appendChild(tdNombre);
                row.appendChild(tdAcciones);
                tablaBody.appendChild(row);
            });

            lucide.createIcons();
        })
        .catch(error => {
            console.error("Error al listar confederaciones:", error);
            alert("Error al cargar las confederaciones.");
        });
}


// Función para guardar o actualizar una confederación
function guardarConfederacion() {
    const confederacion = {
        ConfederacionID: parseInt(document.getElementById("txtConfederacionID").value),
        Nombre: document.getElementById("txtNombre").value.trim()
        // No se envía Siglas
    };

    if (confederacion.Nombre === "") {
        alert("El nombre de la confederación es obligatorio.");
        return;
    }

    fetch("/Confederaciones/Guardar", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(confederacion)
    })
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al guardar la confederación."); });
            }
            return res.json();
        })
        .then(data => {
            if (data.success) {
                cerrarModal();
                listarConfederaciones();
                alert("Confederación guardada con éxito!");
            } else {
                alert("Error al guardar la confederación: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error al guardar confederación:", error);
            alert("Error al guardar la confederación: " + error.message);
        });
}

// Función para editar una confederación (cargar datos en el modal)
function editarConfederacion(id) {
    fetch(`/Confederaciones/Buscar?id=${id}`)
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al buscar la confederación para edición."); });
            }
            return res.json();
        })
        .then(c => {
            document.getElementById("tituloModal").innerText = "Editar Confederación";
            document.getElementById("txtConfederacionID").value = c.ConfederacionID;
            document.getElementById("txtNombre").value = c.Nombre;
            // No se hace referencia a txtSiglas.value
            document.getElementById("modalConfederacion").classList.remove("hidden");
            document.getElementById("modalConfederacion").classList.add("flex");
        })
        .catch(error => {
            console.error("Error al cargar confederación para edición:", error);
            alert("Error al cargar los datos de la confederación para edición: " + error.message);
        });
}

// Función para eliminar una confederación
function eliminarConfederacion(id) {
    if (!confirm("¿Estás seguro de que deseas eliminar esta confederación?")) {
        return;
    }

    fetch(`/Confederaciones/Eliminar?id=${id}`, { method: "POST" })
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => { throw new Error(err.message || "Error al eliminar la confederación."); });
            }
            return res.json();
        })
        .then(data => {
            if (data.success) {
                alert("Confederación eliminada correctamente!");
                listarConfederaciones();
            } else {
                alert("Error al eliminar la confederación: " + data.message);
            }
        })
        .catch(error => {
            console.error("Error al eliminar confederación:", error);
            alert("Error al eliminar la confederación: " + error.message);
        });
}

document.addEventListener('click', function (event) {
    const modal = document.getElementById('modalConfederacion');
    const content = document.getElementById('modalConfederacionContent');
    if (event.target === modal && !content.contains(event.target)) {
        cerrarModal();
    }
});

document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape') {
        const modal = document.getElementById('modalConfederacion');
        if (!modal.classList.contains('hidden')) {
            cerrarModal();
        }
    }
});
