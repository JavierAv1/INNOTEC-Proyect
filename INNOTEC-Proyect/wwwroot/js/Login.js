document.addEventListener('DOMContentLoaded', function () {
    const loginForm = document.getElementById('loginForm');
    const submitButton = document.getElementById('submitButton');
    const toggleButton = document.getElementById('toggleButton');
    const userNameOrEmailDiv = document.getElementById('userNameOrEmailDiv');
    const extraFields = document.getElementById('extraFields');
    const passwordDiv = document.getElementById('passwordDiv');

    toggleButton.addEventListener('click', function () {
        const isLogin = submitButton.textContent === 'Log in';
        document.getElementById('formTitle').textContent = isLogin ? 'Please create a new account' : 'Please login to your account';
        submitButton.textContent = isLogin ? 'Register' : 'Log in';
        document.getElementById('toggleText').textContent = isLogin ? 'Already have an account?' : "Don't have an account?";
        toggleButton.textContent = isLogin ? 'Login' : 'Create new';

        userNameOrEmailDiv.style.display = isLogin ? 'none' : 'block';
        passwordDiv.style.display = 'block';
        extraFields.style.display = isLogin ? 'block' : 'none';

        document.getElementById('userNameOrEmail').disabled = !isLogin;
        document.getElementById('userNameOrEmail').required = !isLogin;
        document.getElementById('userName').required = isLogin;
        document.getElementById('email').required = isLogin;
        document.getElementById('firstName').required = isLogin;
        document.getElementById('lastName').required = isLogin;
        document.getElementById('dob').required = isLogin;
        document.getElementById('gender').required = isLogin;

        loginForm.action = isLogin ? '/Login/Register' : '/Login/Login';
    });

    loginForm.addEventListener('submit', function () {
        submitButton.disabled = true;
    });

    loginForm.addEventListener('keypress', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault();
            submitButton.click();
        }
    });

    // Abrir modal para "Forgot password?"
    document.querySelector('a.text-muted').addEventListener('click', function (event) {
        event.preventDefault();
        $('#forgotPasswordModal').modal('show');
    });
});

function updatePassword() {
    var userName = $('#forgotUsuarioId').val();
    var password = $('#newPassword').val();

    $.ajax({
        url: `/Login/UpdateUsuarioByUserName?userName=${encodeURIComponent(userName)}&password=${encodeURIComponent(password)}`,
        type: 'PUT',
        success: function (response) {
            if (response.success) {
                alert('Contraseña actualizada correctamente.');
                $('#forgotPasswordModal').modal('hide');
            } else {
                alert('Error al actualizar la contraseña: ' + response.message);
            }
        },
        error: function (xhr) {
            alert('Error al actualizar la contraseña: ' + xhr.responseText);
        }
    });
}
