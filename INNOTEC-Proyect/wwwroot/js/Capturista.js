$(document).ready(function () {
    // Función para establecer la pestaña activa en el local storage
    function setActiveTab(tabName) {
        localStorage.setItem('activeTab', tabName);
    }

    // Función para obtener la pestaña activa desde el local storage
    function getActiveTab() {
        return localStorage.getItem('activeTab') || '#departamentos-tab';
    }

    // Función para mostrar mensajes de toast
    function showToastMessage(message) {
        var toastElement = $('#successToast');
        toastElement.find('.toast-body').text(message);
        var toast = new bootstrap.Toast(toastElement);
        toast.show();
    }

    // Función para refrescar la página
    function refreshPage(message) {
        if (message) {
            localStorage.setItem('toastMessage', message);
        }
        location.reload();
    }

    // Función para revisar mensajes de toast almacenados
    function checkForToastMessage() {
        var message = localStorage.getItem('toastMessage');
        if (message) {
            showToastMessage(message);
            localStorage.removeItem('toastMessage');
        }
    }

    // Mostrar la pestaña activa
    var activeTab = getActiveTab();
    $('[data-bs-target="' + activeTab + '"]').tab('show');

    // Establecer la pestaña activa al cambiar de pestaña
    $('button[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        var tabName = $(e.target).data('bs-target');
        setActiveTab(tabName);
    });

    // Función para obtener datos y llenar el modal
    function fetchDataAndPopulateModal(url, fields, modalId) {
        $.getJSON(url, function (response) {
            if (response) {
                for (const [key, value] of Object.entries(fields)) {
                    $(modalId).find(key).val(response[value]);
                }
            } else {
                alert('Error al cargar los datos.');
            }
        }).fail(function () {
            alert('Error al cargar los datos.');
        });
    }

    // Función para llenar campos del modal
    function populateModalFields(data, fields, modalId) {
        for (const [key, value] of Object.entries(fields)) {
            $(modalId).find(key).val(data[value]);
        }
    }

    // Función para cerrar el modal
    function closeModal(modalId) {
        $(modalId).modal('hide');
    }

    // Función para manejar el envío de formularios AJAX
    function ajaxFormSubmit(url, data, modalId, successMessage) {
        $.ajax({
            type: 'POST',
            url: url,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    closeModal(modalId);
                    showToastMessage(successMessage);
                    refreshPage(successMessage);
                } else {
                    alert(response.message);
                }
            }
        });
    }

    // Definimos las funciones en el ámbito global

    window.createProducto = function () {
        var formData = new FormData();
        formData.append('Nombre', $('#createProductoNombre').val());
        formData.append('DescripcionDelProducto', $('#createProductoDescripcion').val());
        formData.append('Precio', $('#createProductoPrecio').val());
        formData.append('Cantidad', $('#createProductoCantidad').val());
        formData.append('IdDepartamento', $('#createProductoDepartamento').val());
        formData.append('IdCategoria', $('#createProductoCategoria').val());
        formData.append('IdSubcategoria', $('#createProductoSubcategoria').val());
        formData.append('IdProveedor', $('#createProductoProveedor').val());

        // Si hay un archivo seleccionado
        var fileInput = $('#createProductoImagen')[0];
        if (fileInput.files.length > 0) {
            formData.append('ImagenDelProducto', fileInput.files[0]);
        }

        $.ajax({
            url: '/Capturista/CreateProducto',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    closeModal('#editModalProducto');
                    showToastMessage('Producto agregado con éxito');
                    refreshPage('Producto agregado con éxito');
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                alert('Error al agregar el producto: ' + xhr.responseText);
            }
        });
    };
    window.createDepartamento = function () {
        var data = {
            Nombre: $('#createDepartamentoNombre').val()
        };
        ajaxFormSubmit('/Capturista/CreateDepartamento', data, '#createModalDepartamento', 'Departamento agregado con éxito');
    }

    window.createProveedor = function () {
        var data = {
            Nombre: $('#createProveedorNombre').val(),
            Telefono: $('#createProveedorTelefono').val(),
            Direccion: $('#createProveedorDireccion').val()
        };
        ajaxFormSubmit('/Capturista/CreateProveedor', data, '#createModalProveedor', 'Proveedor agregado con éxito');
    }

    window.createCategoria = function () {
        var data = {
            Nombre: $('#createCategoriaNombre').val(),
            Descripcion: $('#createCategoriaDescripcion').val(),
            IdDepartamento: $('#createCategoriaDepartamento').val()
        };
        ajaxFormSubmit('/Capturista/CreateCategoria', data, '#createModalCategoria', 'Categoría agregada con éxito');
    }

    window.createSubcategoria = function () {
        var data = {
            Nombre: $('#createSubcategoriaNombre').val(),
            Descripcion: $('#createSubcategoriaDescripcion').val(),
            IdCategoria: $('#createSubcategoriaCategoria').val()
        };
        ajaxFormSubmit('/Capturista/CreateSubcategoria', data, '#createModalSubcategoria', 'Subcategoría agregada con éxito');
    }

    window.updateDepartamento = function () {
        var data = {
            IdDepartamento: $('#editDepartamentoId').val(),
            Nombre: $('#editDepartamentoNombre').val()
        };
        $.ajax({
            type: 'PUT',
            url: '/Capturista/UpdateDepartamento?id=' + data.IdDepartamento,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    closeModal('#editModalDepartamento');
                    showToastMessage('Departamento actualizado con éxito');
                    refreshPage('Departamento actualizado con éxito');
                } else {
                    alert(response.message);
                }
            }
        });
    }

    window.updateProveedor = function () {
        var data = {
            IdProveedor: $('#editProveedorId').val(),
            Nombre: $('#editProveedorNombre').val(),
            Telefono: $('#editProveedorTelefono').val(),
            Direccion: $('#editProveedorDireccion').val()
        };
        $.ajax({
            type: 'PUT',
            url: '/Capturista/UpdateProveedor?id=' + data.IdProveedor,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    closeModal('#editModalProveedor');
                    showToastMessage('Proveedor actualizado con éxito');
                    refreshPage('Proveedor actualizado con éxito');
                } else {
                    alert(response.message);
                }
            }
        });
    }

    window.updateCategoria = function () {
        var data = {
            IdCategoria: $('#editCategoriaId').val(),
            Nombre: $('#editCategoriaNombre').val(),
            Descripcion: $('#editCategoriaDescripcion').val(),
            IdDepartamento: $('#editCategoriaDepartamento').val()
        };
        $.ajax({
            type: 'PUT',
            url: '/Capturista/UpdateCategoria?id=' + data.IdCategoria,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    closeModal('#editModalCategoria');
                    showToastMessage('Categoría actualizada con éxito');
                    refreshPage('Categoría actualizada con éxito');
                } else {
                    alert(response.message);
                }
            }
        });
    }

    window.updateSubcategoria = function () {
        var data = {
            IdSubcategoria: $('#editSubcategoriaId').val(),
            Nombre: $('#editSubcategoriaNombre').val(),
            Descripcion: $('#editSubcategoriaDescripcion').val(),
            IdCategoria: $('#editSubcategoriaCategoria').val()
        };
        $.ajax({
            type: 'PUT',
            url: '/Capturista/UpdateSubcategoria?id=' + data.IdSubcategoria,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    closeModal('#editModalSubcategoria');
                    showToastMessage('Subcategoría actualizada con éxito');
                    refreshPage('Subcategoría actualizada con éxito');
                } else {
                    alert(response.message);
                }
            }
        });
    }

    window.updateProducto = function () {
        var formData = new FormData($('#editFormProducto')[0]);
        console.log("Updating product with ID:", formData.get('IdProductos'));

        $.ajax({
            type: 'POST',
            url: '/Capturista/UpdateProducto?id=' + formData.get('IdProductos'),
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                console.log("Response from server:", response);
                if (response.success) {
                    closeModal('#editModalProducto');
                    showToastMessage('Producto actualizado con éxito');
                    refreshPage('Producto actualizado con éxito');
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                console.error('Error al actualizar el producto:', xhr);
                alert('Error al actualizar el producto: ' + xhr.responseText);
            }
        });
    }


    // Mostrar modales de edición
    $('#editModalDepartamento, #editModalProveedor, #editModalCategoria, #editModalSubcategoria, #editModalProducto').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var modalId = '#' + $(this).attr('id');
        var id = button.data('id');
        var urlMap = {
            '#editModalDepartamento': '/Capturista/GetDepartamentoById?id=' + id,
            '#editModalProveedor': '/Capturista/GetProveedorById?id=' + id,
            '#editModalCategoria': '/Capturista/GetCategoriaById?id=' + id,
            '#editModalSubcategoria': '/Capturista/GetSubcategoriaById?id=' + id,
            '#editModalProducto': '/Capturista/GetProductoById?id=' + id
        };
        var fieldMap = {
            '#editModalDepartamento': {
                '#editDepartamentoId': 'idDepartamento',
                '#editDepartamentoNombre': 'nombre'
            },
            '#editModalProveedor': {
                '#editProveedorId': 'idProveedor',
                '#editProveedorNombre': 'nombre',
                '#editProveedorTelefono': 'telefono',
                '#editProveedorDireccion': 'direccion'
            },
            '#editModalCategoria': {
                '#editCategoriaId': 'idCategoria',
                '#editCategoriaNombre': 'nombre',
                '#editCategoriaDescripcion': 'descripcion',
                '#editCategoriaDepartamento': 'idDepartamento'
            },
            '#editModalSubcategoria': {
                '#editSubcategoriaId': 'idSubcategoria',
                '#editSubcategoriaNombre': 'nombre',
                '#editSubcategoriaDescripcion': 'descripcion',
                '#editSubcategoriaCategoria': 'idCategoria'
            },
            '#editModalProducto': {
                '#editProductoId': 'idProductos',
                '#editProductoNombre': 'nombre',
                '#editProductoDescripcion': 'descripcionDelProducto',
                '#editProductoPrecio': 'precio',
                '#editProductoCantidad': 'cantidad',
                '#editProductoDepartamento': 'idDepartamento',
                '#editProductoCategoria': 'idCategoria',
                '#editProductoSubcategoria': 'idSubcategoria',
                '#editProductoProveedor': 'idProveedor'
            }
        };

        $.getJSON(urlMap[modalId], function (response) {
            if (response) {
                var fields = fieldMap[modalId];
                for (const [key, value] of Object.entries(fields)) {
                    $(modalId).find(key).val(response[value]);
                }
            } else {
                alert('Error al cargar los datos.');
            }
        }).fail(function () {
            alert('Error al cargar los datos.');
        });
    });

    // Funciones para eliminar registros
    function ajaxDelete(url, successMessage) {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (response) {
                if (response.success) {
                    showToastMessage(successMessage);
                    refreshPage(successMessage);
                } else {
                    alert(response.message);
                }
            }
        });
    }

    window.deleteDepartamento = function (id) {
        if (confirm('¿Estás seguro de que deseas eliminar este departamento?')) {
            ajaxDelete('/Capturista/DeleteDepartamento?id=' + id, 'Departamento eliminado con éxito');
        }
    };

    window.deleteProveedor = function (id) {
        if (confirm('¿Estás seguro de que deseas eliminar este proveedor?')) {
            ajaxDelete('/Capturista/DeleteProveedor?id=' + id, 'Proveedor eliminado con éxito');
        }
    };

    window.deleteCategoria = function (id) {
        if (confirm('¿Estás seguro de que deseas eliminar esta categoría?')) {
            ajaxDelete('/Capturista/DeleteCategoria?id=' + id, 'Categoría eliminada con éxito');
        }
    };

    window.deleteSubcategoria = function (id) {
        if (confirm('¿Estás seguro de que deseas eliminar esta subcategoría?')) {
            ajaxDelete('/Capturista/DeleteSubcategoria?id=' + id, 'Subcategoría eliminada con éxito');
        }
    };

    window.deleteProducto = function (id) {
        if (confirm('¿Estás seguro de que deseas eliminar este producto?')) {
            ajaxDelete('/Capturista/DeleteProducto?id=' + id, 'Producto eliminado con éxito');
        }
    };

    // Event listeners for form submissions
    $('#createFormDepartamento').on('submit', function (e) {
        e.preventDefault();
        createDepartamento();
    });

    $('#editFormDepartamento').on('submit', function (e) {
        e.preventDefault();
        updateDepartamento();
    });

    $('#createFormProveedor').on('submit', function (e) {
        e.preventDefault();
        createProveedor();
    });

    $('#editFormProveedor').on('submit', function (e) {
        e.preventDefault();
        updateProveedor();
    });

    $('#createFormCategoria').on('submit', function (e) {
        e.preventDefault();
        createCategoria();
    });

    $('#editFormCategoria').on('submit', function (e) {
        e.preventDefault();
        updateCategoria();
    });

    $('#createFormSubcategoria').on('submit', function (e) {
        e.preventDefault();
        createSubcategoria();
    });

    $('#editFormSubcategoria').on('submit', function (e) {
        e.preventDefault();
        updateSubcategoria();
    });

    $('#createFormProducto').on('submit', function (e) {
        e.preventDefault();
        createProducto();
    });

    $('#editFormProducto').on('submit', function (e) {
        e.preventDefault();
        updateProducto();
    });

    // Check for any toast messages to show on page load
    checkForToastMessage();
});
