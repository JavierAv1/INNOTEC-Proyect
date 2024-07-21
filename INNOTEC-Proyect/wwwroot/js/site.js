document.addEventListener("DOMContentLoaded", function () {
    var dropdownElements = document.querySelectorAll('.dropdown-menu .dropdown-toggle');

    dropdownElements.forEach(function (dropdownToggle) {
        dropdownToggle.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            var submenu = this.nextElementSibling;
            var parentLi = this.closest('li');

            if (!submenu.classList.contains('show')) {
                var openMenus = this.closest('.dropdown-menu').querySelectorAll('.dropdown-menu.show');
                openMenus.forEach(function (openMenu) {
                    openMenu.classList.remove('show');
                    openMenu.closest('li').classList.remove('show');
                });

                submenu.classList.add('show');
                parentLi.classList.add('show');
            } else {
                submenu.classList.remove('show');
                parentLi.classList.remove('show');
            }
        });
    });

    document.addEventListener('click', function (e) {
        if (!e.target.matches('.dropdown-toggle')) {
            var openMenus = document.querySelectorAll('.dropdown-menu.show');
            openMenus.forEach(function (openMenu) {
                openMenu.classList.remove('show');
                if (openMenu.closest('li')) {
                    openMenu.closest('li').classList.remove('show');
                }
            });
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    var searchInput = document.querySelector('.search-input');
    var resultsContainer = document.querySelector('.search-results');
    var searchForm = document.getElementById('searchForm');

    searchInput.addEventListener('input', function () {
        var searchText = this.value.trim();

        if (searchText.length > 2) {
            fetch(`/GetByName?search=${encodeURIComponent(searchText)}`)
                .then(response => response.json())
                .then(data => {
                    resultsContainer.innerHTML = '';
                    if (data.length > 0) {
                        data.forEach(item => {
                            let div = document.createElement('div');
                            div.textContent = item.nombre;
                            div.className = 'result-item';
                            div.dataset.id = item.idProductos;
                            resultsContainer.appendChild(div);

                            div.addEventListener('click', () => {
                                window.location.href = `/OneProduct/${item.idProductos}`;
                            });
                        });
                    } else {
                        let div = document.createElement('div');
                        div.textContent = 'No results found';
                        div.className = 'result-item';
                        resultsContainer.appendChild(div);
                    }
                    resultsContainer.style.display = 'block';
                });
        } else {
            resultsContainer.style.display = 'none';
        }
    });

    searchForm.addEventListener('keypress', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault();
            var firstResult = resultsContainer.querySelector('.result-item');
            if (firstResult) {
                window.location.href = `/OneProduct/${firstResult.dataset.id}`;
            }
        }
    });

    document.addEventListener('click', function (e) {
        if (!searchInput.contains(e.target) && !resultsContainer.contains(e.target)) {
            resultsContainer.style.display = 'none';
        }
    });
});


