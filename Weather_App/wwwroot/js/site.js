$(document).ready(function () {
    // Načtení oblíbených položek po načtení stránky
    $.ajax({
        url: '/Home/Favorite',
        type: 'POST',
        success: function (data) {
            $('#favorite').html(data);

            // Po načtení oblíbených položek znovu přiřadit event listener na toggleButton
            assignToggleListener();
        }
    });
});

function assignToggleListener() {
    var toggleButton = document.getElementById('toggleSidebar');
    var sidebar = document.getElementById('sidebar');
    var content = document.querySelector('.content');

    toggleButton.addEventListener('click', function () {
        sidebar.classList.toggle('visible');
        content.classList.toggle('shifted');
    });
}

// Zavolejte funkci pro přiřazení event listeneru po načtení stránky
document.addEventListener('DOMContentLoaded', function () {
    assignToggleListener();
});

$(document).ready(function () {
    assignToggleListener();
});

$(document).ready(function () {
    assignToggleListener();
});

function assignToggleListener() {
    var toggleButton = document.getElementById('toggleSidebar');
    var sidebar = document.getElementById('sidebar');
    var content = document.querySelector('.content');

    toggleButton.addEventListener('click', function () {
        sidebar.classList.toggle('visible');
        content.classList.toggle('shifted');

        // Načíst oblíbené položky pouze při prvním zobrazení sidebaru
        if (!sidebar.classList.contains('loaded')) {
            $.ajax({
                url: '/Home/Favorite',
                type: 'POST',
                success: function (data) {
                    $('#favorite').html(data);
                    sidebar.classList.add('loaded'); // Označit, že oblíbené položky byly načteny
                }
            });
        }
    });
}




