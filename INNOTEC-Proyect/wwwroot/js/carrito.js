$(document).ready(function () {
    // Cuando cambia la cantidad de un producto específico
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
            FechaDeCompra: new Date().toISOString().split('T')[0], // Fecha actual
            FechaVencimiento: new Date(new Date().setMonth(new Date().getMonth() + 1)).toISOString().split('T')[0] // Un mes después
        };

        $.ajax({
            url: '/Carrito/Update',
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(compra),
            success: function (response) {
                if (response.success) {
                    location.reload(); // Recarga la página para actualizar los datos
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

    // Cuando se elimina un producto del carrito
    $('.eliminar-btn').on('click', function () {
        var id = $(this).data('id');

        $.ajax({
            url: '/Carrito/Delete/' + id,
            type: 'DELETE',
            success: function (response) {
                if (response.success) {
                    location.reload(); // Recarga la página para actualizar la lista
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

    // Comprar un solo producto
    $('.comprar-btn').on('click', function () {
        var idCompra = $(this).data('id');
        var total = $(this).data('total');
        var cantidad = $(this).closest('.d-flex').find('.cantidad').val(); // Captura la cantidad seleccionada
        console.log("Cantidad seleccionada para comprar ahora: ", cantidad); // Verifica la cantidad capturada

        var url = '/Envio/Index?idCompra=' + encodeURIComponent(idCompra) + '&total=' + encodeURIComponent(total) + '&cantidad=' + encodeURIComponent(cantidad);
        window.location.href = url;
    });

    // Continuar con la compra de todos los productos
    $('#continuar-compra').on('click', function () {
        var idsCompra = [];
        var totalCompra = 0;
        var totalCantidad = 0;

        // Recorre cada producto para obtener la cantidad y el total
        $('.cantidad').each(function () {
            totalCantidad += parseInt($(this).val());
        });

        $('.idcompra').each(function () {
            idsCompra.push($(this).val());
        });

        totalCompra = parseFloat($('#total-compra').text());

        var url = '/Envio/Index?idsCompra=' + encodeURIComponent(idsCompra.join(',')) + '&total=' + encodeURIComponent(totalCompra) + '&cantidad=' + encodeURIComponent(totalCantidad);
        window.location.href = url;
    });
});
