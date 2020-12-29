function errorMes(message, errorTitle) {
    toastr.error(message, errorTitle, toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    })
};

function sucMes(message, sucTitle) {

    toastr.success(message, sucTitle, toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "700",
        "hideDuration": "1000",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    })
};

function warningMes(message, warTitle) {
    toastr.warning(message, warTitle, toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    })
};

function LoginControlAdmin() {
    var data = {
        UserName: $("#UserName").val(),
        Password: $("#Password").val(),
    }
    if (data.UserName.trim() != "" && data.Password.trim() != "") {
        $.ajax({
            url: "/Login/LoginControl",
            type: "post",
            data: data,
            success: function (e) {
                console.log(e);
                if (e == "gameover") {
                    window.location.replace("/Shared/LicenceError");
                }
                else {
                    if (e != true) {
                        errorMes("Kullanıcı adı veya parola hatalı!", "Giriş Hatası!");
                        window.location("/Login/Index");
                    }
                  
                    else {
                        sucMes(data.UserName, "Hoşgeldin!")
                        setInterval(function () {
                            window.location.replace("/Home/Index");
                        }, 1000); //3 seconds
                    }
                }
            }
        });
    }
    else {
        warningMes("Kullanıcı adı ve Parola Boş Geçilemez", "Giriş Hatası!");
        window.location("/");
    }
}

$('#ekleKullanici').on('click', () => {
    var bosStr = "";

    data = {
        UserName: $('#kullaniciAdi').val(),
        Password: $('#kullaniciSifre').val(),
        IsActive: $("input[name='aktifMi']:checked").val(),
        Role: $("input[name='Role']:checked").val(),
    };

    if (data.UserName === bosStr || data.Password === bosStr) {
        if (data.UserName === bosStr) {
            $('#kullaniciAdErr').removeClass('hidden');
        } else {
            $('#kullaniciAdErr').addClass('hidden');
        }
        if (data.Password === bosStr) {
            $('#sifreErr').removeClass('hidden');
        } else {
            $('#sifreErr').addClass('hidden');
        }
    } else {
        if (data.Password != bosStr) {
            $('#sifreErr').addClass('hidden');
        };

        $.ajax({
            url: "/User/Insert",
            type: "post",
            data: data,
            success: function (e) {
                if (e == false) {
                    errorMes("Aynı Kullanıcı Sisteme Kayıtlı !", "Başarısız");
                }
                else {
                    sucMes(data.UserName, "Başarıyla Eklendi");
                    setInterval(function () {
                        window.location.replace("/Home/Index");
                    }, 1000); //3 seconds
                }
            }
        });
    }
});