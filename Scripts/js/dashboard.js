document.addEventListener('DOMContentLoaded', () => {
    cargarResumenDashboard();
    cargarResultadosRecientes();
    cargarEstadisticaDestacada();
});

function cargarResumenDashboard() {
    $.get('/Home/ObtenerResumen', function (data) {
        if (data && !data.Error) {
            document.getElementById('stat-activos').textContent = data.TorneosActivos;
            document.getElementById('stat-equipos').textContent = data.TotalEquipos;
            document.getElementById('stat-confederacion').textContent = data.Confederacion;
        } else {
            console.warn("Error al obtener resumen:", data.Error);
        }
    });
}

function cargarResultadosRecientes() {
    $.get('/Home/ResultadosRecientes', function (partidos) {
        const contenedor = document.querySelector('#dashboard-view .space-y-3');
        contenedor.innerHTML = '';
        partidos.forEach(p => {
            contenedor.innerHTML += `
                <div class="grid grid-cols-10 items-center gap-2 p-2 hover:bg-gray-800/50 rounded-lg">
                    <div class="col-span-4 flex items-center justify-end gap-3 font-medium"><span>${p.EquipoLocal}</span></div>
                    <div class="col-span-2 text-center font-bold text-lg bg-gray-700/80 rounded p-1">${p.GolesLocal} - ${p.GolesVisitante}</div>
                    <div class="col-span-4 flex items-center gap-3 font-medium"><span>${p.EquipoVisitante}</span></div>
                </div>`;
        });
    });
}

function cargarEstadisticaDestacada() {
    $.get('/Home/EstadisticaDestacada', function (data) {
        if (!data || data.Error) return;

        const contenedor = document.querySelector('#bloqueDestacado');
        contenedor.innerHTML = `
        <h3 class="text-xl font-semibold text-white mb-4">Estadísticas Destacadas</h3>
        <div class="p-4 bg-gray-900/50 rounded-lg flex flex-col items-center text-center">
            <img src="https://placehold.co/64x64/1f2937/ffffff?text=${data.Nombre}" alt="Logo Club ${data.Nombre}" />
            <p class="font-bold text-white">${data.Nombre}</p>
            <p class="text-sm text-gray-400">Equipo con mejor desempeño</p>
            <div class="mt-3 flex items-center gap-4 text-sm">
                <div class="text-green-400"><strong class="block text-lg">${data.PartidosJugados}</strong> Partidos Jug.</div>
                <div class="text-yellow-400"><strong class="block text-lg">${data.PorcentajeVictorias}%</strong> Victorias</div>
            </div>
        </div>`;
    });
}
