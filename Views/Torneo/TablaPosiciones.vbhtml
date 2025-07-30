@Code
    ViewBag.Title = "Tabla de Posiciones"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="flex justify-between items-center mb-6">
    <h2 class="text-3xl font-bold text-white">Tabla de Posiciones: <span id="standingsTorneoName" class="text-green-400"></span></h2>
    <div class="flex items-center gap-4">
        <button id="btnSimulateFullTournament" class="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-lg flex items-center gap-2 transition-colors">
            <i data-lucide="play" class="w-5 h-5"></i> Simular Torneo Completo
        </button>
        <a href="@Url.Action("Index", "Torneo")" class="bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-lg flex items-center gap-2 transition-colors">
            <i data-lucide="arrow-left" class="w-5 h-5"></i> Volver a Torneos
        </a>
    </div>
</div>

<div class="card p-4 mb-6 rounded-lg">
    <div class="mb-4">
        <label for="standingsYear" class="block text-sm font-medium text-gray-300 mb-2">Año:</label>
        <select id="standingsYear" class="bg-gray-700 border border-gray-600 rounded-lg px-3 py-2 text-white w-full md:w-auto">
            <option value="2024">2024</option>
            <option value="2025" selected>2025</option>
            <option value="2026">2026</option>
            <option value="2027">2027</option>
            <option value="2028">2028</option>
            <option value="2029">2029</option>
            <option value="2030">2030</option>
        </select>
    </div>

    <div id="standingsTableContainer" class="overflow-x-auto rounded-lg">
        <table class="min-w-full text-sm text-left text-gray-400">
            <thead class="text-xs uppercase bg-gray-700 text-gray-400">
                <tr>
                    <th scope="col" class="px-6 py-3">Equipo</th>
                    <th scope="col" class="px-6 py-3">PJ</th>
                    <th scope="col" class="px-6 py-3">PG</th>
                    <th scope="col" class="px-6 py-3">PE</th>
                    <th scope="col" class="px-6 py-3">PP</th>
                    <th scope="col" class="px-6 py-3">GF</th>
                    <th scope="col" class="px-6 py-3">GC</th>
                    <th scope="col" class="px-6 py-3">DG</th>
                    <th scope="col" class="px-6 py-3">PTS</th>
                </tr>
            </thead>
            <tbody id="tblStandingsBody">
            </tbody>
        </table>
    </div>
    <p id="noStandingsMessage" class="text-gray-400 text-center mt-4 hidden">No hay partidos finalizados para mostrar la tabla de posiciones en el año seleccionado.</p>
</div>

<style>
    .table-row-even {
        background-color: rgba(55, 65, 81, 0.5);
    }

    .table-row-odd {
        background-color: rgba(75, 85, 99, 0.5);
    }

    .table-header {
        background-color: rgba(31, 41, 55, 0.8);
        border-bottom: 2px solid rgba(107, 114, 128, 0.5);
    }

    .card {
        background-color: rgba(31, 41, 55, 0.8);
        border: 1px solid rgba(107, 114, 128, 0.3);
    }

    .spinner {
        border: 4px solid rgba(255, 255, 255, 0.3);
        border-radius: 50%;
        border-top: 4px solid #fff;
        width: 24px;
        height: 24px;
        -webkit-animation: spin 1s linear infinite;
        animation: spin 1s linear infinite;
    }

    -webkit-keyframes spin {
        0%

    {
        -webkit-transform: rotate(0deg);
    }

    100% {
        -webkit-transform: rotate(360deg);
    }

    }

    keyframes spin {
        0%

    {
        transform: rotate(0deg);
    }

    100% {
        transform: rotate(360deg);
    }
    }
</style>

@section Scripts
    <script src="/scripts/jquery-3.7.1.min.js"></script>
    <script src="~/scripts/js/Torneo.js"></script>

    <script>
        $(document).ready(function () {
            const urlParams = new URLSearchParams(window.location.search);
            const torneoID = urlParams.get('torneoID');
            const torneoNombre = urlParams.get('torneoNombre');

            if (torneoID && torneoNombre) {
                $('#standingsTorneoName').text(decodeURIComponent(torneoNombre));
                loadStandings(torneoID, $('#standingsYear').val());
            } else {
                $('#standingsTorneoName').text('No se encontró el torneo.');
                $('#standingsTableContainer').addClass('hidden');
                $('#noStandingsMessage').removeClass('hidden').text('No se encontró un ID de torneo válido.');
                $('#btnSimulateFullTournament').prop('disabled', true);
            }

            $('#standingsYear').change(function () {
                if (torneoID) {
                    loadStandings(torneoID, $(this).val());
                }
            });

            $('#btnSimulateFullTournament').click(function () {
                const currentTorneoID = torneoID;
                const currentTorneoNombre = decodeURIComponent(torneoNombre);
                const currentYear = $('#standingsYear').val();

                if (!currentTorneoID || !currentTorneoNombre || !currentYear) {
                    alert('No se pudo obtener la información completa del torneo para simular.');
                    return;
                }

                if (!confirm(`¿Estás seguro de que quieres simular TODO el torneo "${currentTorneoNombre}" para el año ${currentYear}? Esto afectará todos los partidos pendientes y las tablas de posiciones.`)) {
                    return;
                }

                const $button = $(this);
                $button.prop('disabled', true).html('<div class="spinner"></div> Simulando...');

                const phaseForFullSimulation = "Fase de Grupos";

                $.ajax({
                    url: '@Url.Action("SimularPartidos", "Torneo")',
                    type: 'POST',
                    data: {
                        torneoID: currentTorneoID,
                        torneoNombre: currentTorneoNombre,
                        año: currentYear,
                        fase: phaseForFullSimulation
                    },
                    success: function (response) {
                        if (response.success) {
                            alert(response.message);
                            loadStandings(currentTorneoID, currentYear);
                        } else {
                            alert('Error: ' + response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error("Error al simular torneo completo:", error);
                        alert("Error al simular torneo completo: " + (xhr.responseJSON ? xhr.responseJSON.message : "Error desconocido"));
                    },
                    complete: function () {
                        $button.prop('disabled', false).html('<i data-lucide="play" class="w-5 h-5"></i> Simular Torneo Completo');
                        lucide.createIcons();
                    }
                });
            });

            lucide.createIcons();
        });

        function loadStandings(torneoID, year) {
            $.ajax({
                url: '@Url.Action("RecuperarTablaPosiciones", "Torneo")',
                type: 'GET',
                data: { torneoID: torneoID, año: year },
                dataType: 'json',
                success: function (data) {
                    var tbody = $('#tblStandingsBody');
                    tbody.empty();
                    if (data && data.length > 0) {
                        $('#standingsTableContainer').removeClass('hidden');
                        $('#noStandingsMessage').addClass('hidden');
                        $.each(data, function (i, item) {
                            var row = `
                                <tr class="${i % 2 === 0 ? 'table-row-even' : 'table-row-odd'} border-b border-gray-700">
                                    <td class="px-6 py-4 font-medium text-white">${item.Equipo}</td>
                                    <td class="px-6 py-4">${item.PJ}</td>
                                    <td class="px-6 py-4">${item.PG}</td>
                                    <td class="px-6 py-4">${item.PE}</td>
                                    <td class="px-6 py-4">${item.PP}</td>
                                    <td class="px-6 py-4">${item.GF}</td>
                                    <td class="px-6 py-4">${item.GC}</td>
                                    <td class="px-6 py-4">${item.DG}</td>
                                    <td class="px-6 py-4 font-bold text-green-400">${item.PTS}</td>
                                </tr>
                            `;
                            tbody.append(row);
                        });
                    } else {
                        $('#standingsTableContainer').addClass('hidden');
                        $('#noStandingsMessage').removeClass('hidden').text('No hay partidos finalizados para mostrar la tabla de posiciones en el año seleccionado.');
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error al cargar tabla de posiciones:", error);
                    alert("Error al cargar la tabla de posiciones.");
                    $('#standingsTableContainer').addClass('hidden');
                    $('#noStandingsMessage').removeClass('hidden').text('Error al cargar la tabla de posiciones.');
                }
            });
        }
    </script>
End Section