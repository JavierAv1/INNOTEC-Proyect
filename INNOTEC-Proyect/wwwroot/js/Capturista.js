function setActiveTab(tabName) {
    localStorage.setItem('activeTab', tabName);
}

function getActiveTab() {
    return localStorage.getItem('activeTab') || '#departamentos-tab';
}

$(document).ready(function () {
    var activeTab = getActiveTab();
    $('[href="' + activeTab + '"]').tab('show');

    $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        var tabName = $(e.target).attr('href');
        setActiveTab(tabName);
    });

    $('#editModalDepartamento').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        console.log(`Fetching data for Departamento ID: ${id}`);
        $.getJSON(`/Capturista/GetDepartamentoById?id=${id}`, function (response) {
            console.log('Response:', response);
            if (response) {
                $('#editDepartamentoId').val(response.idDepartamento);
                $('#editDepartamentoNombre').val(response.nombre);
            } else {
                alert('Error al cargar los datos del departamento.');
            }
        }).fail(function () {
            alert('Error al cargar los datos del departamento.');
        });
    });

    $('#editModalProveedor').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        console.log(`Fetching data for Proveedor ID: ${id}`);
        $.getJSON(`/Capturista/GetProveedorById?id=${id}`, function (response) {
            console.log('Response:', response);
            if (response) {
                $('#editProveedorId').val(response.idProveedor);
                $('#editProveedorNombre').val(response.nombre);
                $('#editProveedorTelefono').val(response.telefono);
                $('#editProveedorDireccion').val(response.direccion);
            } else {
                alert('Error al cargar los datos del proveedor.');
            }
        }).fail(function () {
            alert('Error al cargar los datos del proveedor.');
        });
    });

    $('#editModalCategoria').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        console.log(`Fetching data for Categoria ID: ${id}`);
        $.getJSON(`/Capturista/GetCategoriaById?id=${id}`, function (response) {
            console.log('Response:', response);
            if (response) {
                $('#editCategoriaId').val(response.idCategoria);
                $('#editCategoriaNombre').val(response.nombre);
                $('#editCategoriaDescripcion').val(response.descripcion);
                $('#editCategoriaDepartamento').val(response.idDepartamento);
            } else {
                alert('Error al cargar los datos de la categoría.');
            }
        }).fail(function () {
            alert('Error al cargar los datos de la categoría.');
        });
    });

    $('#editModalSubcategoria').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        console.log(`Fetching data for Subcategoria ID: ${id}`);
        $.getJSON(`/Capturista/GetSubcategoriaById?id=${id}`, function (response) {
            console.log('Response:', response);
            if (response) {
                $('#editSubcategoriaId').val(response.idSubcategoria);
                $('#editSubcategoriaNombre').val(response.nombre);
                $('#editSubcategoriaDescripcion').val(response.descripcion);
                $('#editSubcategoriaCategoria').val(response.idCategoria);
            } else {
                alert('Error al cargar los datos de la subcategoría.');
            }
        }).fail(function () {
            alert('Error al cargar los datos de la subcategoría.');
        });
    });

    $('#editModalProducto').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        console.log(`Fetching data for Producto ID: ${id}`);
        $.getJSON(`/Capturista/GetProductoById?id=${id}`, function (response) {
            console.log('Response:', response);
            if (response) {
                $('#editProductoId').val(response.idProductos);
                $('#editProductoNombre').val(response.nombre);
                $('#editProductoDescripcion').val(response.descripcionDelProducto);
                $('#editProductoPrecio').val(response.precio);
                $('#editProductoCantidad').val(response.cantidad);
                $('#editProductoDepartamento').val(response.idDepartamento);
                $('#editProductoCategoria').val(response.idCategoria);
                $('#editProductoSubcategoria').val(response.idSubcategoria);
                $('#editProductoProveedor').val(response.idProveedor);
            } else {
                alert('Error al cargar los datos del producto.');
            }
        }).fail(function () {
            alert('Error al cargar los datos del producto.');
        });
    });

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
});

// CRUD functions for each entity

// Departamento
function createDepartamento() {
    var formData = new FormData($('#createFormDepartamento')[0]);
    $.ajax({
        url: '/Capturista/CreateDepartamento',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert('Error al agregar el departamento: ' + xhr.responseText);
        }
    });
}

function updateDepartamento() {
    var formData = new FormData($('#editFormDepartamento')[0]);
    var idDepa = $('#editDepartamentoId').val();
    $.ajax({
        url: `/Capturista/UpdateDepartamento?id=${idDepa}`,
        type: 'PUT',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert('Error al actualizar el departamento: ' + xhr.responseText);
        }
    });
}

function deleteDepartamento(id) {
    if (confirm('¿Estás seguro de que deseas eliminar este departamento?')) {
        $.ajax({
            url: `/Capturista/DeleteDepartamento?id=${id}`,
            type: 'DELETE',
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                alert('Error al eliminar el departamento: ' + xhr.responseText);
            }
        });
    }
}

// Proveedor
function createProveedor() {
    var formData = {
        Nombre: $('#createProveedorNombre').val(),
        Telefono: $('#createProveedorTelefono').val(),
        Direccion: $('#createProveedorDireccion').val()
    };
    $.ajax({
        url: '/Capturista/CreateProveedor',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert('Error al agregar el proveedor: ' + xhr.responseText);
        }
    });
}

function updateProveedor() {
    var formData = {
        IdProveedor: $('#editProveedorId').val(),
        Nombre: $('#editProveedorNombre').val(),
        Telefono: $('#editProveedorTelefono').val(),
        Direccion: $('#editProveedorDireccion').val()
    };
    $.ajax({
        url: `/Capturista/UpdateProveedor?id=${formData.IdProveedor}`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert('Error al actualizar el proveedor: ' + xhr.responseText);
        }
    });
}

function deleteProveedor(id) {
    if (confirm('¿Estás seguro de que deseas eliminar este proveedor?')) {
        $.ajax({
            url: `/Capturista/DeleteProveedor?id=${id}`,
            type: 'DELETE',
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                alert('Error al eliminar el proveedor: ' + xhr.responseText);
            }
        });
    }
}

// Categoria
function createCategoria() {
    var formData = {
        Nombre: $('#createCategoriaNombre').val(),
        Descripcion: $('#createCategoriaDescripcion').val(),
        IdDepartamento: $('#createCategoriaDepartamento').val()
    };
    $.ajax({
        url: '/Capturista/CreateCategoria',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert('Error al agregar la categoría: ' + xhr.responseText);
        }
    });
}

function updateCategoria() {
    var formData = {
        IdCategoria: $('#editCategoriaId').val(),
        Nombre: $('#editCategoriaNombre').val(),
        Descripcion: $('#editCategoriaDescripcion').val(),
        IdDepartamento: $('#editCategoriaDepartamento').val()
    };
    $.ajax({
        url: `/Capturista/UpdateCategoria?id=${formData.IdCategoria}`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert('Error al actualizar la categoría: ' + xhr.responseText);
        }
    });
}

function deleteCategoria(id) {
    if (confirm('¿Estás seguro de que deseas eliminar esta categoría?')) {
        $.ajax({
            url: `/Capturista/DeleteCategoria?id=${id}`,
            type: 'DELETE',
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                alert('Error al eliminar la categoría: ' + xhr.responseText);
            }
        });
    }
}

// Subcategoria
function createSubcategoria() {
    var formData = {
        Nombre: $('#createSubcategoriaNombre').val(),
        Descripcion: $('#createSubcategoriaDescripcion').val(),
        IdCategoria: $('#createSubcategoriaCategoria').val()
    };
    $.ajax({
        url: '/Capturista/CreateSubcategoria',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert('Error al agregar la subcategoría: ' + xhr.responseText);
        }
    });
}

function updateSubcategoria() {
    var formData = {
        IdSubcategoria: $('#editSubcategoriaId').val(),
        Nombre: $('#editSubcategoriaNombre').val(),
        Descripcion: $('#editSubcategoriaDescripcion').val(),
        IdCategoria: $('#editSubcategoriaCategoria').val()
    };
    $.ajax({
        url: `/Capturista/UpdateSubcategoria?id=${formData.IdSubcategoria}`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert('Error al actualizar la subcategoría: ' + xhr.responseText);
        }
    });
}

function deleteSubcategoria(id) {
    if (confirm('¿Estás seguro de que deseas eliminar esta subcategoría?')) {
        $.ajax({
            url: `/Capturista/DeleteSubcategoria?id=${id}`,
            type: 'DELETE',
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                alert('Error al eliminar la subcategoría: ' + xhr.responseText);
            }
        });
    }
}

// Producto
function createProducto() {
    var formData = new FormData($('#createFormProducto')[0]);
    $.ajax({
        url: '/Capturista/CreateProducto',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert('Error al agregar el producto: ' + xhr.responseText);
        }
    });
}

function updateProducto() {
    var formData = new FormData($('#editFormProducto')[0]);
    var idProducto = $('#editProductoId').val();
    $.ajax({
        url: `/Capturista/UpdateProducto?id=${idProducto}`,
        type: 'PUT',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function (xhr) {
            alert('Error al actualizar el producto: ' + xhr.responseText);
        }
    });
}

function deleteProducto(id) {
    if (confirm('¿Estás seguro de que deseas eliminar este producto?')) {
        $.ajax({
            url: `/Capturista/DeleteProducto?id=${id}`,
            type: 'DELETE',
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr) {
                alert('Error al eliminar el producto: ' + xhr.responseText);
            }
        });
    }
}
