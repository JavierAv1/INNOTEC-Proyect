document.addEventListener("DOMContentLoaded", () => {
    const mp = new MercadoPago(publicKey, {
        locale: "es-MX",
        sandbox: true
    });

    document.getElementById("paymentBtn").addEventListener("click", async () => {
        try {
            const selectedAddressId = $('input[name="selectedAddress"]:checked').val();
            if (!selectedAddressId) {
                alert("Por favor selecciona una dirección antes de proceder al pago.");
                return;
            }

            // Construye la lista de pedidos con el IdEnvio incluido
            const pedidos = [];
            $('.idcompra').each(function () {
                pedidos.push({
                    IdCompra: parseInt($(this).val()),
                    FechaPedido: new Date().toISOString(),
                    EstadoPedido: "Entrante",
                    UsuarioId: 1, // Cambia esto si lo necesitas
                    Envios: [{
                        IdEnvio: parseInt(selectedAddressId)
                    }]
                });
            });

            const pedidoData = {
                Pedidos: pedidos
            };

            const insertPedidoResponse = await fetch("/Envio/InsertPedido", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(pedidoData),
            });

            const insertPedidoResult = await insertPedidoResponse.json();

            if (insertPedidoResult.success) {
  
                const orderData = {
                    title: "Products",
                    quantity: 0,
                    price: 100, 
                };

                const response = await fetch("/Envio/create_Preference", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(orderData),
                });

                const preference = await response.json();

                if (preference.success && preference.id) {
                    createCheckoutButton(preference.id);
                } else {
                    alert("Error al crear la preferencia de pago. Por favor, inténtalo de nuevo.");
                }
            } else {
                alert("Error al insertar el pedido. Por favor, inténtalo de nuevo.");
            }
        } catch (error) {
            alert("Error :(");
        }
    });

    const createCheckoutButton = (preferenceId) => {
        const bricksBuilder = mp.bricks();

        const renderComponent = async () => {
            if (window.checkoutButton) window.checkoutButton.unmount();
            window.checkoutButton = await bricksBuilder.create("wallet", "wallet_container", {
                initialization: {
                    preferenceId: preferenceId,
                },
                customization: {
                    texts: {
                        valueProp: 'smart_option',
                    },
                },
            });
        };
        renderComponent();
    };

    $(document).ready(function () {
        loadSavedAddresses();

   
        $('#addressList').on('change', 'input[name="selectedAddress"]', function () {
            disableTextBoxes();
            $('#paymentBtn').show();
        });
    });

    function loadSavedAddresses() {
        $.ajax({
            type: 'GET',
            url: '/Envio/GetEnviosByUserId',
            success: function (response) {
                if (response && response.length > 0) {
                    var addressList = $('#addressList');
                    addressList.empty();
                    $.each(response, function (index, address) {
                        addressList.append(
                            `<li class="list-group-item">
                                <input type="radio" name="selectedAddress" value="${address.idEnvio}" />
                                ${address.codigoPostal}, ${address.estado}, ${address.calle}, ${address.colonia}, ${address.municipio}, ${address.numero}
                            </li>`
                        );
                    });
                } else {
                    $('#savedAddresses').html('<p>No tienes direcciones guardadas.</p>');
                }
            },
            error: function () {
                toastr.error('Error al cargar las direcciones guardadas.', 'Error', {
                    positionClass: 'toast-bottom-right'
                });
            }
        });
    }

    function disableTextBoxes() {
        $('#CodigoPostal').prop('disabled', true);
        $('#Estado').prop('disabled', true);
        $('#Calle').prop('disabled', true);
        $('#Colonia').prop('disabled', true);
        $('#Municipio').prop('disabled', true);
        $('#Numero').prop('disabled', true);
    }

    function enableTextBoxes() {
        $('#CodigoPostal').prop('disabled', false);
        $('#Estado').prop('disabled', false);
        $('#Calle').prop('disabled', false);
        $('#Colonia').prop('disabled', false);
        $('#Municipio').prop('disabled', false);
        $('#Numero').prop('disabled', false);
        $('input[name="selectedAddress"]').prop('checked', false);
        $('#paymentBtn').hide();
    }

    function deselectSavedAddresses() {
        $('input[name="selectedAddress"]').prop('checked', false);
        $('#paymentBtn').hide();
        enableTextBoxes();
    }
});
