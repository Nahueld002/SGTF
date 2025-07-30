document.addEventListener("DOMContentLoaded", () => {
    listarConfederaciones();

    document.getElementById("formConfederacion")
        .addEventListener("submit", e => {
            e.preventDefault();
            guardarConfederacion();
        });
});

function mostrarModal() {
    const modal = document.getElementById("modalConfederacion");
    const content = document.getElementById("modalConfederacionContent");

    modal.classList.remove("hidden"); modal.classList.add("flex");
    setTimeout(() => {
        content.classList.remove("scale-95", "opacity-0");
        content.classList.add("scale-100", "opacity-100");
    }, 10);
}

function ocultarModal() {
    const modal = document.getElementById("modalConfederacion");
    const content = document.getElementById("modalConfederacionContent");

    content.classList.add("scale-95", "opacity-0");
    content.classList.remove("scale-100", "opacity-100");

    setTimeout(() => {
        modal.classList.add("hidden"); modal.classList.remove("flex");
        document.getElementById("formConfederacion").reset();
    }, 200);
}

function abrirModalNuevo() {
    document.getElementById("tituloModal").innerText = "Crear Confederación";
    document.getElementById("txtConfederacionID").value = "0";
    document.getElementById("txtNombre").value = "";
    mostrarModal();
}

function listarConfederaciones() {
    fetch("/Confederaciones/Listar")
        .then(r => r.json())
        .then(({ success, data, message }) => {
            if (success === false) throw new Error(message);

            const tbody = document.querySelector("#tablaConfederaciones tbody");
            tbody.innerHTML = "";
            let par = true;

            data.forEach(c => {
                tbody.insertAdjacentHTML("beforeend", `
                    <tr class="${par ? "table-row-even" : "table-row-odd"}
                                hover:bg-gray-600/50 transition-colors">
                        <td class="p-4 font-medium">${c.Nombre}</td>
                        <td class="p-4 text-center">
                            <div class="flex justify-center gap-2">
                                <button class="p-2 rounded-md hover:bg-gray-600"
                                        title="Editar"
                                        onclick="editarConfederacion(${c.ConfederacionID})">
                                    <i data-lucide="edit"
                                       class="w-4 h-4 text-yellow-400"></i>
                                </button>
                                <button class="p-2 rounded-md hover:bg-gray-600"
                                        title="Eliminar"
                                        onclick="eliminarConfederacion(${c.ConfederacionID})">
                                    <i data-lucide="trash-2"
                                       class="w-4 h-4 text-red-400"></i>
                                </button>
                            </div>
                        </td>
                    </tr>`);
                par = !par;
            });
            if (window.lucide?.createIcons) lucide.createIcons();
        })
        .catch(err => {
            console.error("Listar confederaciones:", err);
            alert("Error al cargar las confederaciones: " + err.message);
        });
}

function guardarConfederacion() {
    const conf = {
        ConfederacionID: +document.getElementById("txtConfederacionID").value,
        Nombre: document.getElementById("txtNombre").value.trim()
    };
    if (!conf.Nombre) return alert("El nombre de la confederación es obligatorio.");

    fetch("/Confederaciones/Guardar", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(conf)
    })
        .then(r => r.json().then(j => ({ ok: r.ok, ...j })))
        .then(({ ok, success, message }) => {
            if (!ok || success === false) throw new Error(message);
            ocultarModal();
            listarConfederaciones();
            alert("Confederación guardada con éxito!");
        })
        .catch(err => {
            console.error("Guardar confederación:", err);
            alert("Error al guardar la confederación: " + err.message);
        });
}

function editarConfederacion(id) {
    fetch(`/Confederaciones/Buscar?id=${id}`)
        .then(r => r.json().then(j => ({ ok: r.ok, ...j })))
        .then(({ ok, success, message, ConfederacionID, Nombre }) => {
            if (!ok || success === false) throw new Error(message);

            document.getElementById("tituloModal").innerText = "Editar Confederación";
            document.getElementById("txtConfederacionID").value = ConfederacionID;
            document.getElementById("txtNombre").value = Nombre;
            mostrarModal();
        })
        .catch(err => {
            console.error("Editar confederación:", err);
            alert("Error al cargar los datos: " + err.message);
        });
}

function eliminarConfederacion(id) {
    if (!confirm("¿Estás seguro de que deseas eliminar esta confederación?")) return;

    fetch(`/Confederaciones/Eliminar?id=${id}`, { method: "POST" })
        .then(r => r.json().then(j => ({ ok: r.ok, ...j })))
        .then(({ ok, success, message }) => {
            if (!ok || success === false) throw new Error(message);
            alert("Confederación eliminada correctamente!");
            listarConfederaciones();
        })
        .catch(err => {
            console.error("Eliminar confederación:", err);
            alert("Error al eliminar la confederación: " + err.message);
        });
}

document.addEventListener("click", e => {
    const modal = document.getElementById("modalConfederacion");
    const content = document.getElementById("modalConfederacionContent");
    if (e.target === modal && !content.contains(e.target)) ocultarModal();
});
document.addEventListener("keydown", e => {
    if (e.key === "Escape" &&
        !document.getElementById("modalConfederacion").classList.contains("hidden"))
        ocultarModal();
});