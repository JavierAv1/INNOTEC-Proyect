document.addEventListener("DOMContentLoaded", () => {
    const mp = new MercadoPago(publicKey, {
        locale: "es-MX",
        sandbox: true 
    });

    document.getElementById("paymentBtn").addEventListener("click", async () => {
        try {
            const orderData = {
                title: "Products",
                quantity: 1,
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
            createCheckoutButton(preference.id);
        } catch (error) {
            alert("error :(");
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

    function saveAddress() {
        var postData = {
            CodigoPostal: $('#CodigoPostal').val(),
            Estado: $('#Estado').val(),
            Calle: $('#Calle').val(),
            Colonia: $('#Colonia').val(),
            Municipio: $('#Municipio').val(),
            Numero: $('#Numero').val()
        };

        $.ajax({
            type: 'POST',
            url: '/Envio/InsertEnvio',
            contentType: 'application/json',
            data: JSON.stringify(postData),
            success: function (response) {
                if (response.success) {
                    toastr.success('Dirección guardada con éxito!', 'Éxito', {
                        positionClass: 'toast-bottom-right'
                    });
                    $('#saveAddressBtn').prop('disabled', true);
                    $('#contactForm').addClass('reduced');
                    loadSavedAddresses(); // Reload saved addresses after saving a new one
                } else {
                    toastr.error('Error al guardar la dirección.', 'Error', {
                        positionClass: 'toast-bottom-right'
                    });
                }
            },
            error: function () {
                toastr.error('Error al guardar la dirección.', 'Error', {
                    positionClass: 'toast-bottom-right'
                });
            }
        });
    }

    function redirectToPayment() {
        var selectedAddressId = $('input[name="selectedAddress"]:checked').val();
        if (selectedAddressId) {
            // Redirigir a la página de pago (reemplazar la URL con la página de pago correspondiente)
            window.location.href = '/PaymentPage?addressId=' + selectedAddressId;
        } else {
            toastr.error('Por favor selecciona una dirección antes de proceder al pago.', 'Error', {
                positionClass: 'toast-bottom-right'
            });
        }
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
