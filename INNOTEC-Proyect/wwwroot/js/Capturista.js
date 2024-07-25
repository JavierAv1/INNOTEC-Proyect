$(document).ready(function () {
    function setActiveTab(tabName) {
        localStorage.setItem('activeTab', tabName);
    }

    function getActiveTab() {
        return localStorage.getItem('activeTab') || '#departamentos-tab';
    }

    var activeTab = getActiveTab();
    $('[href="' + activeTab + '"]').tab('show');

    $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        var tabName = $(e.target).attr('href');
        setActiveTab(tabName);
    });

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

    $('#editModalDepartamento').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var url = `/Capturista/GetDepartamentoById?id=${id}`;
        var fields = {
            '#editDepartamentoId': 'idDepartamento',
            '#editDepartamentoNombre': 'nombre'
        };
        fetchDataAndPopulateModal(url, fields, '#editModalDepartamento');
    });

    $('#editModalProveedor').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var url = `/Capturista/GetProveedorById?id=${id}`;
        var fields = {
            '#editProveedorId': 'idProveedor',
            '#editProveedorNombre': 'nombre',
            '#editProveedorTelefono': 'telefono',
            '#editProveedorDireccion': 'direccion'
        };
        fetchDataAndPopulateModal(url, fields, '#editModalProveedor');
    });

    $('#editModalCategoria').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var url = `/Capturista/GetCategoriaById?id=${id}`;
        var fields = {
            '#editCategoriaId': 'idCategoria',
            '#editCategoriaNombre': 'nombre',
            '#editCategoriaDescripcion': 'descripcion',
            '#editCategoriaDepartamento': 'idDepartamento'
        };
        fetchDataAndPopulateModal(url, fields, '#editModalCategoria');
    });

    $('#editModalSubcategoria').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var url = `/Capturista/GetSubcategoriaById?id=${id}`;
        var fields = {
            '#editSubcategoriaId': 'idSubcategoria',
            '#editSubcategoriaNombre': 'nombre',
            '#editSubcategoriaDescripcion': 'descripcion',
            '#editSubcategoriaCategoria': 'idCategoria'
        };
        fetchDataAndPopulateModal(url, fields, '#editModalSubcategoria');
    });

    $('#editModalProducto').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var url = `/Capturista/GetProductoById?id=${id}`;
        $.getJSON(url, function (response) {
            if (response) {
                var fields = {
                    '#editProductoId': 'idProductos',
                    '#editProductoNombre': 'nombre',
                    '#editProductoDescripcion': 'descripcionDelProducto',
                    '#editProductoPrecio': 'precio',
                    '#editProductoCantidad': 'cantidad'
                };
                populateModalFields(response, fields, '#editModalProducto');
                $('#editProductoDepartamento').val(response.departamento?.idDepartamento || null);
                $('#editProductoCategoria').val(response.categoria?.idCategoria || null);
                $('#editProductoSubcategoria').val(response.subcategoria?.idSubcategoria || null);
                $('#editProductoProveedor').val(response.proveedor?.idProveedor || null);
            } else {
                alert('Error al cargar los datos.');
            }
        }).fail(function () {
            alert('Error al cargar los datos.');
        });
    });

    function populateModalFields(data, fields, modalId) {
        for (const [key, value] of Object.entries(fields)) {
            $(modalId).find(key).val(data[value]);
        }
    }

    function showToastMessage(message) {
        var toastElement = $('#successToast');
        toastElement.find('.toast-body').text(message);
        var toast = new bootstrap.Toast(toastElement);
        toast.show();
    }

    function refreshTabContent(tab) {
        var tabUrlMap = {
            '#departamentos-tab': '/Capturista/GetDepartamentos',
            '#proveedores-tab': '/Capturista/GetProveedores',
            '#categorias-tab': '/Capturista/GetCategorias',
            '#subcategorias-tab': '/Capturista/GetSubcategorias',
            '#productos-tab': '/Capturista/GetProductos'
        };

        var url = tabUrlMap[tab];
        if (url) {
            $.get(url, function (data) {
                var contentId = tab.replace('-tab', '-content');
                $(contentId).html(data);
                showToastMessage('Guardado con éxito.');
            });
        }
    }

    function closeModal(modalId) {
        $(modalId).modal('hide');
    }

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
                    refreshTabContent(getActiveTab());
                } else {
                    alert(response.message);
                }
            }
        });
    }



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

    // Functions to handle CRUD operations
    window.createDepartamento = function () {
        var data = {
            Nombre: $('#createDepartamentoNombre').val()
        };
        $.ajax({
            type: 'POST',
            url: '/Capturista/CreateDepartamento',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    $('#createModalDepartamento').modal('hide');
                    showToastMessage('Departamento agregado con éxito');
                    refreshPage();
                } else {
                    alert(response.message);
                }
            }
        });
    };

    window.createProveedor = function () {
        var data = {
            Nombre: $('#createProveedorNombre').val(),
            Telefono: $('#createProveedorTelefono').val(),
            Direccion: $('#createProveedorDireccion').val()
        };
        $.ajax({
            type: 'POST',
            url: '/Capturista/CreateProveedor',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    $('#createModalProveedor').modal('hide');
                    showToastMessage('Proveedor agregado con éxito');
                    refreshPage();
                } else {
                    alert(response.message);
                }
            }
        });
    };

    window.createCategoria = function () {
        var data = {
            Nombre: $('#createCategoriaNombre').val(),
            Descripcion: $('#createCategoriaDescripcion').val(),
            IdDepartamento: $('#createCategoriaDepartamento').val()
        };
        $.ajax({
            type: 'POST',
            url: '/Capturista/CreateCategoria',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    $('#createModalCategoria').modal('hide');
                    showToastMessage('Categoría agregada con éxito');
                    refreshPage();
                } else {
                    alert(response.message);
                }
            }
        });
    };

    window.createSubcategoria = function () {
        var data = {
            Nombre: $('#createSubcategoriaNombre').val(),
            Descripcion: $('#createSubcategoriaDescripcion').val(),
            IdCategoria: $('#createSubcategoriaCategoria').val()
        };
        $.ajax({
            type: 'POST',
            url: '/Capturista/CreateSubcategoria',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    $('#createModalSubcategoria').modal('hide');
                    showToastMessage('Subcategoría agregada con éxito');
                    refreshPage();
                } else {
                    alert(response.message);
                }
            }
        });
    };

    window.createProducto = function () {
        var formData = new FormData($('#createFormProducto')[0]);
        $.ajax({
            type: 'POST',
            url: '/Capturista/CreateProducto',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    $('#createModalProducto').modal('hide');
                    showToastMessage('Producto agregado con éxito');
                    refreshPage();
                } else {
                    alert(response.message);
                }
            }
        });
    };

    // Update Functions
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
                    $('#editModalDepartamento').modal('hide');
                    showToastMessage('Departamento actualizado con éxito');
                    refreshPage();
                } else {
                    alert(response.message);
                }
            }
        });
    };

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
                    $('#editModalProveedor').modal('hide');
                    showToastMessage('Proveedor actualizado con éxito');
                    refreshPage();
                } else {
                    alert(response.message);
                }
            }
        });
    };

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
                    $('#editModalCategoria').modal('hide');
                    showToastMessage('Categoría actualizada con éxito');
                    refreshPage();
                } else {
                    alert(response.message);
                }
            }
        });
    };

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
                    $('#editModalSubcategoria').modal('hide');
                    showToastMessage('Subcategoría actualizada con éxito');
                    refreshPage();
                } else {
                    alert(response.message);
                }
            }
        });
    };

    window.updateProducto = function () {
        var formData = new FormData($('#editFormProducto')[0]);
        $.ajax({
            type: 'PUT',
            url: '/Capturista/UpdateProducto?id=' + formData.get('IdProductos'),
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    $('#editModalProducto').modal('hide');
                    showToastMessage('Producto actualizado con éxito');
                    refreshPage();
                } else {
                    alert(response.message);
                }
            }
        });
    };



    // Funciones para eliminar registros
    function ajaxDelete(url, successMessage) {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (response) {
                if (response.success) {
                    showToastMessage(successMessage);
                    refreshTabContent(getActiveTab());
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

    // Mostrar mensaje de éxito
    function showToastMessage(message) {
        $('#successToast .toast-body').text(message);
        $('#successToast').toast('show');
    }

    // Refrescar la página después de una operación exitosa
    function refreshPage() {
        location.reload();
    }

});
