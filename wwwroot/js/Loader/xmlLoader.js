document.addEventListener("DOMContentLoaded", function () {
    function send(file) {

        function showError() {
            console.log("showError");
            $(".toast.fail").toast('show');
        };

        function showOK() {
            console.log("showOK");
            $(".toast.ok").toast('show');
        };

        function sendForm(file) {

            var iframe = $("<iframe>").appendTo("body").css("display", "none").load(function (e) {
                var response = $(this).contents().find("body").html();
                (response != "ok") ? showError() : showOK();
                $(this).remove();
            });

            var frameBody = iframe.contents().find("body");

            var form = $("<form>").appendTo(frameBody).attr({
                "action": "/Loader/Upload",
                "method": "POST",
                "target": "_self",
                "enctype": "multipart/form-data"
            });

            $("<input>").appendTo(form).attr("name", "uploads").val(file);
            form.submit();
        };

        function sendData(file) {

            var formData = new FormData()
            formData.append("upload", file);

            return $.ajax({
                url: "/Loader/Upload",
                method: "POST",
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                beforeSend: function (xhr) {

                },
                success: function (data) {
                    showOK();
                },
                error: function () {
                    showError();
                }
            });
        };

        (window.FormData) ? sendData(file) : sendForm(file);
    };

    var fileSelector = $("<input>").appendTo('body')
        .css("display", "none")
        .attr({
            type: "file",
            multiple: false,
            accept: "text/xml"
        })
        .change(function () {
            for (var key in this.files) {
                if (key == "length") return;
                var file = this.files[key];
                if (file) send(file);
            };
        });

    $("#myFileLoader").css("cursor", 'pointer').click(function () {
        fileSelector.click();
    });

});
