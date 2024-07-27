$(document).ready(function () {
    $('#editModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');

        $.getJSON(`/Usuario/GetById?id=${id}`, function (response) {
            if (response.success) {
                var data = response.data;
                $('#editUsuarioId').val(data.usuarioId);
                $('#editNombre').val(data.nombre);
                $('#editApellidoPaterno').val(data.apellidoPaterno);
                $('#editApellidoMaterno').val(data.apellidoMaterno);
                $('#editFechaDeNacimiento').val(new Date(data.fechaDeNacimiento).toISOString().substr(0, 10));
                $('#editSexo').val(data.sexo);
                $('#editUserName').val(data.userName);
                $('#editCorreo').val(data.correo);
                $('#editTelefono').val(data.telefono);
                $('#editCelular').val(data.celular);
                $('#editTipoUsuario').val(data.tipoUsuarioIdTipousuario);
            } else {
                alert('Error al cargar los datos del usuario.');
            }
        }).fail(function () {
            alert('Error al cargar los datos del usuario.');
        });
    });
});

function createUsuario() {
    var formData = new FormData($('#createForm')[0]);

    $.ajax({
        url: '/Usuario/Insert',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert('Error al agregar el usuario.');
            }
        },
        error: function (xhr) {
            alert('Error al agregar el usuario: ' + xhr.responseText);
        }
    });
}

function updateUsuario() {
    var formData = new FormData($('#editForm')[0]);
    var id = $('#editUsuarioId').val();

    $.ajax({
        url: `/Usuario/Update?id=${id}`,
        type: 'PUT',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                location.reload();
            } else {
                alert('Error al actualizar el usuario.');
            }
        },
        error: function (xhr) {
            alert('Error al actualizar el usuario: ' + xhr.responseText);
        }
    });
}

function deleteUsuario(id) {
    if (confirm('¿Estás seguro de que deseas eliminar este usuario?')) {
        $.ajax({
            url: `/Usuario/Delete?id=${id}`,
            type: 'DELETE',
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert('Error al eliminar el usuario.');
                }
            },
            error: function (xhr) {
                alert('Error al eliminar el usuario: ' + xhr.responseText);
            }
        });
    }
}