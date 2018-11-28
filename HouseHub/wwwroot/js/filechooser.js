$('.custom-file-input').on('change', function (e) {
    var fileName = e.target.files[0].name;
    $(this).next('.custom-file-label').html(fileName);
})