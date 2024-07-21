$(document).ready(function () {
    $('.cantidad').on('change', function () {
        var id = $(this).data('id');
        var cantidad = $(this).val();
        var idusuario = $(this).closest('.d-flex').find('.idusuario').val();
        var idproducto = $(this).closest('.d-flex').find('.idproducto').val();
        var idcompra = $(this).closest('.d-flex').find('.idcompra').val();

        var compra = {
            IdCompra: parseInt(idcompra),
            Cantidad: parseInt(cantidad),
            Idusuario: parseInt(idusuario),
            Idproducto: parseInt(idproducto),
            FechaDeCompra: new Date().toISOString().split('T')[0],
            FechaVencimiento: new Date(new Date().setMonth(new Date().getMonth() + 1)).toISOString().split('T')[0]
        };

        $.ajax({
            url: updateUrl,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(compra),
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert('Error al actualizar la cantidad: ' + response.errorMessage);
                }
            },
            error: function (xhr) {
                var errorMessage = xhr.responseJSON ? xhr.responseJSON.message : 'Error desconocido';
                alert('Error al actualizar la cantidad: ' + errorMessage);
            }
        });
    });

    $('.eliminar-btn').on('click', function () {
        var id = $(this).data('id');

        $.ajax({
            url: deleteUrl + '/' + id,
            type: 'DELETE',
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert('Error al eliminar el producto: ' + response.errorMessage);
                }
            },
            error: function (xhr) {
                var errorMessage = xhr.responseJSON ? xhr.responseJSON.message : 'Error desconocido';
                alert('Error al eliminar el producto: ' + errorMessage);
            }
        });
    });

    $('.comprar-btn').on('click', function () {
        var id = $(this).data('id');
        window.location.href = comprarUrl + '?id=' + id;
    });
});
