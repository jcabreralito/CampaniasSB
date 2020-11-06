//function cambiar() {
//    var pdrs = document.getElementById('ImagenFile').files[0].name;
//    document.getElementById('info').innerHTML = pdrs;
//}

//$("#ImagenFile").change(function (event) {
//    var ev = event.target;

//    if (this.files && this.files[0]) {
//        var reader = new FileReader();
//        reader.onload = imageIsLoaded;
//        reader.readAsDataURL(this.files[0]);
//    }
//    function imageIsLoaded(e) {
//        $(ev).parent().siblings('#matImage').attr("src", e.target.result);
//    };
//});


function viewPageIndex() {
    document.getElementById('contenedor_carga').style.visibility = 'visible';
    document.getElementById('contenedor_carga').style.opacity = '1';
};

function viewPage() {
    document.getElementById('contenedor_carga').style.visibility = 'visible';
    document.getElementById('contenedor_carga').style.opacity = '1';

    document.getElementById('contenedor_carga').style.zIndex = '0';
};

function hidePage() {

    document.getElementById('contenedor_carga').style.visibility = 'visible';
    document.getElementById('contenedor_carga').style.opacity = '1';
    //document.getElementById('Aceptar').disabled = 'disabled';

    document.getElementById('contenedor_carga').style.zIndex = '10000';

};

function hidePages() {

    document.getElementById('contenedor_carga').style.visibility = 'hidden';
    document.getElementById('contenedor_carga').style.opacity = '0';

    document.getElementById('contenedor_carga').style.zIndex = '10000';

};

$(function () {
    $(document).tooltip({
        position: {
            my: "center top+6",
            at: "center bottom+6",
            using: function (position, feedback) {
                $(this).css(position);
                $("<div>")
                    .addClass("arrow")
                    .addClass(feedback.vertical)
                    .addClass(feedback.horizontal)
                    .appendTo(this);
            }
        }
    });
});

$(document).ready(function () {
    $('.sidenav').sidenav();
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#blah')
                .attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);

        var pdrs = document.getElementById('ImagenFile').files[0].name;
        document.getElementById('info').innerHTML = pdrs;
    }
}

function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octec-stream;base64," + bytesBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}