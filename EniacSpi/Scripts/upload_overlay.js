$(function () {
    var upload = function (files) {
        var formData = new FormData(),
            xhr = new XMLHttpRequest();

        for (i = 0; i < files.length; i++) {
            formData.append(files[i].name, files[i]);
        }

        xhr.open('post', $('#url').val() );
        xhr.setRequestHeader('__RequestVerificationToken', $('[name=__RequestVerificationToken]').val())
        xhr.send(formData);

        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                alert(files.length + " files uploaded");
            }
        }
    };

    document.getElementById("FileInput").onchange = function (e) {
        upload(this.files);
    };

    $('#addBtn').on('click', function (e) {
        $('#overlay').css({
            'display': 'flex'
        });
    });

    $('#overlay').on('click', function (e) {
        if (e.target !== this)
            return;
        $('#overlay').css({
            'display': 'none'
        });
    });

    $('#overlay').on('dragover drop dragdrop', function (e) {
        e.preventDefault();
    });

    $("div > div", "#overlay").on('drop dragdrop', function (event) {
        $(this).css({
            'color': '#ccc',
            'border-color': '#ccc'
        });

        upload(event.originalEvent.dataTransfer.files);
    });

    $("div > div", "#overlay").on('dragleave', function (event) {
        event.preventDefault();
        $(this).css({
            'color': '#ccc',
            'border-color': '#ccc'
        });
    });

    $("div > div", "#overlay").on('dragenter', function (event) {
        event.preventDefault();
        $(this).css({
            'color': '#000',
            'border-color': '#000'
        });
    });
});