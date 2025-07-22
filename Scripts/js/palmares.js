document.addEventListener("DOMContentLoaded", function () {
    //listarPalmares(); // For individual titles
    listarTotalTitulos(); // For total titles per team
    // Call other functions here as you implement them for other statistics
    // listarDetalleTorneosResultados();
    // listarDesgloseTitulosPorTorneo();
    // listarTotalSubcampeonatos();
});

// Function to load and display individual Palmares entries
function listarPalmares() {
    fetch("/Palmares/Listar")
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => {
                    throw new Error(err.error || "Error en el servidor al cargar palmares.");
                });
            }
            return res.json();
        })
        .then(data => {
            const tablaBody = document.querySelector("#tablaPalmares tbody");
            tablaBody.innerHTML = ""; // Clear existing rows

            if (data.error) {
                alert(data.error);
                console.error("Error del servidor (palmares individual):", data.error);
                return;
            }

            if (!data.data || data.data.length === 0) {
                const row = document.createElement("tr");
                row.innerHTML = `
                    <td colspan="4" class="p-4 text-center text-gray-400">
                        No hay palmares registrados individualmente.
                    </td>
                `;
                tablaBody.appendChild(row);
                return;
            }

            let filaPar = true;
            data.data.forEach(p => {
                const claseFila = filaPar ? "table-row-even" : "table-row-odd";
                filaPar = !filaPar;

                const row = document.createElement("tr");
                row.className = `${claseFila} hover:bg-gray-600/50 transition-colors`;

                const tdAnioTitulo = document.createElement("td");
                tdAnioTitulo.className = "p-4 text-center";
                tdAnioTitulo.textContent = p.AñoTitulo;

                const tdNombreEquipo = document.createElement("td");
                tdNombreEquipo.className = "p-4 font-medium";
                tdNombreEquipo.textContent = p.NombreEquipo;

                const tdNombreTorneo = document.createElement("td");
                tdNombreTorneo.className = "p-4";
                tdNombreTorneo.textContent = p.NombreTorneo;

                const tdAcciones = document.createElement("td");
                tdAcciones.className = "p-4 text-center";
                tdAcciones.textContent = "N/A";

                row.appendChild(tdAnioTitulo);
                row.appendChild(tdNombreEquipo);
                row.appendChild(tdNombreTorneo);
                row.appendChild(tdAcciones);
                tablaBody.appendChild(row);
            });
        })
        .catch(error => {
            console.error("Error al cargar palmares (individual):", error);
            alert("Error al cargar los palmares individuales: " + error.message);
        });
}

// Function to load and display total titles per team
function listarTotalTitulos() {
    fetch("/Palmares/GetTotalTitulos")
        .then(res => {
            if (!res.ok) {
                return res.json().then(err => {
                    throw new Error(err.error || "Error en el servidor al cargar el total de títulos.");
                });
            }
            return res.json();
        })
        .then(data => {
            const tablaBody = document.querySelector("#tablaTotalTitulos tbody"); // Targeting a new table body
            tablaBody.innerHTML = ""; // Clear existing rows

            if (data.error) {
                alert(data.error);
                console.error("Error del servidor (total títulos):", data.error);
                return;
            }

            if (!data.data || data.data.length === 0) {
                const row = document.createElement("tr");
                row.innerHTML = `
                    <td colspan="2" class="p-4 text-center text-gray-400">
                        No hay datos de títulos totales.
                    </td>
                `;
                tablaBody.appendChild(row);
                return;
            }

            let filaPar = true;
            data.data.forEach(item => {
                const claseFila = filaPar ? "table-row-even" : "table-row-odd";
                filaPar = !filaPar;

                const row = document.createElement("tr");
                row.className = `${claseFila} hover:bg-gray-600/50 transition-colors`;

                const tdEquipo = document.createElement("td");
                tdEquipo.className = "p-4 font-medium";
                tdEquipo.textContent = item.Equipo;

                const tdTotalTitulos = document.createElement("td");
                tdTotalTitulos.className = "p-4 text-center font-bold";
                tdTotalTitulos.textContent = item.TotalTitulos;

                row.appendChild(tdEquipo);
                row.appendChild(tdTotalTitulos);
                tablaBody.appendChild(row);
            });
        })
        .catch(error => {
            console.error("Error al cargar el total de títulos:", error);
            alert("Error al cargar el total de títulos: " + error.message);
        });
}

// Placeholder functions for future statistics:
/*
function listarDetalleTorneosResultados() {
    fetch("/Palmares/GetDetalleTorneosResultados")
        .then(res => res.json())
        .then(data => {
            // Logic to populate #tablaDetalleTorneos tbody
            // ...
        })
        .catch(error => console.error("Error al cargar detalle de torneos:", error));
}

function listarDesgloseTitulosPorTorneo() {
    fetch("/Palmares/GetDesgloseTitulosPorTorneo")
        .then(res => res.json())
        .then(data => {
            // Logic to populate #tablaDesgloseTitulos tbody
            // ...
        })
        .catch(error => console.error("Error al cargar desglose de títulos:", error));
}

function listarTotalSubcampeonatos() {
    fetch("/Palmares/GetTotalSubcampeonatos")
        .then(res => res.json())
        .then(data => {
            // Logic to populate #tablaSubcampeonatos tbody
            // ...
        })
        .catch(error => console.error("Error al cargar subcampeonatos:", error));
}
*/