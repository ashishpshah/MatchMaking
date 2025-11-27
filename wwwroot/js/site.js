// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('body').on('keypress', '.isNumberKey', function (evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /^[0-9]*$/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
});

$('body').on('keypress', '.isNumberKey_Decimal', function (evt, obj) {

    var theEvent = evt || window.event;
    var charCode = (evt.which) ? evt.which : event.keyCode;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }

    var dotcontains = theEvent.target.value.indexOf(".") != -1;
    if (dotcontains)
        if (charCode == 46) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
            return false;
        };

    var regex = /^[0-9]*$/;
    if (!regex.test(key) && !(charCode == 46)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
        return false;
    }

});

$('body').on('click', '.btnSubmit', function (e) {
    e.preventDefault();

    if ($(this).parents('form').length > 0) fnSubmitForm($(this).parents('form').attr('id'));
    else fnSubmitForm($(this).attr('data-form'));
});

$('.modal').on('shown.bs.modal', function (e) {

    let $modal = $(this);
    let $form = $modal.find('form');

    // Reset normal HTML form fields
    if ($form.length > 0) {
        $form[0].reset();
    }
    /*$('body').append('<div class="modal-backdrop fade show" id="div-modal-backdrop"></div>')*/
    var button = $(e.relatedTarget);

    var forTarget = button.attr('data-for-target');
    if (typeof forTarget != 'undefined' && forTarget != null && forTarget.length > 0)
        $(this).find('form input[type="file"]').attr('name', 'file' + forTarget);

    if (typeof forTarget != 'undefined' && forTarget != null && forTarget.length > 0 && forTarget == 'Others')
        $(this).find('form input[type="file"]').attr('multiple', '');
    else
        $(this).find('form input[type="file"]').removeAttr('multiple');

    $(this).find('form input[type="file"]').imageuploadify();
});

$('.modal').on('hide.bs.modal', function (e) {
    let $modal = $(this);
    let $form = $modal.find('form');

    // Reset normal HTML form fields
    if ($form.length > 0) {
        $form[0].reset();
    }

    try {
        // Reset file inputs
        $modal.find('input[type="file"]').val('');

        // Clear textareas
        $modal.find('textarea').val('');

        // Uncheck radios & checkboxes
        $modal.find('input[type="checkbox"], input[type="radio"]').prop('checked', false);
    } catch { }

    try {
        $modal.find('div.imageuploadify').remove();
        $modal.find('input[type="file"]').val(null).trigger('change');
    } catch { }

    try {
        $modal.find('button.btn-close').trigger('blur');     // remove focus
        $modal.setAttribute("aria-hidden", "true");
    } catch { }

    //$('body div#div-modal-backdrop').remove();

    //$('div.loader-overlay').remove();
    //if (!($(document.activeElement)[0].type == 'button' && $($(document.activeElement)[0]).hasClass('close')))
    //    e.preventDefault();
});

function ShowLoader(isShow) {

    if (isShow == true) $('.preloader').removeClass('d-none');
    else $('.preloader').addClass('d-none');
}

function formValidate($id) {

    var IsValid = true;
    $.each($($id + ' input[data-required]'), function (key, input) {
        if ((typeof input.value == 'undefined' || input.value == null || input.value.length == 0) && !input.hasAttribute('disabled') && !$(input).hasClass('temp_fileUpload')) {
            Swal.fire({ icon: 'error', title: input.getAttribute('data-msg') });
            IsValid = false;
            input.focus();
            return IsValid;
        }
    });

    $.each($($id + ' select[data-required]'), function (key, input) {
        if ((typeof input.value == 'undefined' || input.value == null || input.value.length == 0 || input.value == "0") && !$(input)[0].hasAttribute('disabled')) {
            Swal.fire({ icon: 'error', title: input.getAttribute('data-msg') });
            IsValid = false;
            input.focus();
            return IsValid;
        }
    });


    var inputFiles = $($id + ' input[type="file"]');

    if (typeof inputFiles != 'undefined' && inputFiles != null && inputFiles.length > 0) {

        const totalMax = 5 * 1024 * 1024; // 500 MB
        let totalSize = 0;

        $.each(inputFiles, function (key, input) {
            if ((typeof input.value != 'undefined' && input.value != null && input.value.length > 0) && !input.hasAttribute('disabled') && !$(input).hasClass('temp') && !$(input).hasClass('temp_fileUpload')) {
                var file = document.getElementById('' + input.getAttribute('id')).files[0];
                totalSize += file.size;
            }
        });

        if (totalSize > totalMax) {
            Swal.fire({
                icon: 'error', title: `Total selected files exceed the limit.<br>` +
                    `Selected: ${(totalSize / (1024 * 1024)).toFixed(2)} MB<br>` +
                    `Max Allowed: ${totalMax / (1024 * 1024)} MB`
            });
            return false;
        }
    }


    return IsValid;
}

function fnSubmitForm($id) {
    ShowLoader(true);

    var $form = $('#' + $id);

    if (formValidate('#' + $id)) {

        let formData = new FormData();

        const array = $form.serializeArray();

        $.each($('#' + $id + ' select'), function (key, input) {
            if ((typeof input.value != 'undefined' && input.value != null && input.value.length > 0) && !input.hasAttribute('disabled') && !$(input).hasClass('temp'))
                array.filter(function (obj) {
                    if (obj['name'] == $(input).attr('name')) {
                        obj['value'] = $('#' + $(input).attr('id') + ' option:selected').val()
                    }
                })
        });

        $.each($('#' + $id + ' input[type="checkbox"]'), function (key, input) {
            if ((typeof input.value != 'undefined' && input.value != null && input.value.length > 0) && !input.hasAttribute('disabled') && !$(input).hasClass('temp'))
                array.filter(function (obj) {
                    if (obj['name'] == $(input).attr('name')) {
                        obj['value'] = $(input).is(':checked')
                    }
                })
        });

        $.each($('#' + $id + ' input[type="radio"]'), function (key, input) {
            if ((typeof input.value != 'undefined' && input.value != null && input.value.length > 0) && !input.hasAttribute('disabled') && !$(input).hasClass('temp'))
                array.filter(function (obj) {
                    if (obj['name'] == $(input).attr('name')) {
                        obj['value'] = $('input[name="' + $(input).attr('name') + '"]:checked').val()
                    }
                })
        });

        $.each(array, function (key, input) {
            if (typeof input.name != 'undefined' && input.name != null && input.name.length > 0 && input.name != "__RequestVerificationToken")
                formData.append(input.name, input.value);
        });

        var inputFiles = $('#' + $id + ' input[type="file"]');

        if (typeof inputFiles != 'undefined' && inputFiles != null && inputFiles.length > 0)
            $.each(inputFiles, function (key, input) {
                if ((typeof input.value != 'undefined' && input.value != null && input.value.length > 0) && !input.hasAttribute('disabled') && !$(input).hasClass('temp') && !$(input).hasClass('temp_fileUpload')) {
                    var file = document.getElementById('' + input.getAttribute('id')).files[0];
                    if (typeof input.getAttribute('name') == 'undefined' || input.getAttribute('name') == null || input.getAttribute('name').length == 0) {
                        formData.append("files", file);
                    } else {
                        formData.append(input.getAttribute('name'), file);
                    }
                }
            });

        $.ajax({
            type: 'POST',
            url: $form.attr('action'),
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            dataType: "json",
            success: function (response) {
                debugger;
                try {
                    ShowLoader(false);
                    if (response.IsSuccess == true) {
                        if (typeof response.IsConfirm != 'undefined' && response.IsConfirm != null && response.IsConfirm != '' && response.IsConfirm == true)
                            if (typeof response.RedirectURL != 'undefined' && response.RedirectURL != null && response.RedirectURL != '')
                                CommonConfirmed_Success(response.Message, response.RedirectURL, null);
                            else
                                CommonConfirmed_Success(response.Message, fnSubmitForm_Success, [response, $id]);
                        else
                            if (typeof response.RedirectURL != 'undefined' && response.RedirectURL != null && response.RedirectURL != '')
                                window.location = response.RedirectURL;
                            else
                                fnSubmitForm_Success(response, $id);
                    }
                    else {
                        CommonAlert_Error(response.Message, null)
                    }
                } catch {
                    window.location.reload();
                }
            },
            //xhr: function () {
            //    var fileXhr = $.ajaxSettings.xhr();
            //    if (fileXhr.upload) {
            //        $("progress").show();
            //        fileXhr.upload.addEventListener("progress", function (e) {
            //            if (e.lengthComputable) {
            //                $("#fileProgress").attr({
            //                    value: e.loaded,
            //                    max: e.total
            //                });
            //            }
            //        }, false);
            //    }
            //    return fileXhr;
            //},
            failure: function (response) { ShowLoader(false); CommonAlert_Error(null) },
            error: function (response) { ShowLoader(false); CommonAlert_Error(null) }
        });

    }
    else { ShowLoader(false); return false; }

}

function CommonAlert_Error(msg, redirectUrl) {

    if (msg == null || msg == "")
        msg = "Oops...! Something went wrong!";

    Swal.fire({
        icon: 'error',
        title: msg,
        showDenyButton: false,
        showCancelButton: false,
        confirmButtonText: 'OK'
    }).then((result) => {

        if (typeof redirectUrl != 'undefined' && redirectUrl != null && redirectUrl != '')
            window.location = redirectUrl;

        ShowLoader(false);
    })
}

function CommonAlert_Success(msg) {

    if (msg == null || msg == "")
        msg = "Data Successfully saved.";

    Swal.fire({
        icon: 'success',
        title: msg,
        showDenyButton: false,
        showCancelButton: false,
        confirmButtonText: 'OK',
    });
}

function CommonConfirmed_Success(msg, functionName, functionParams) { //params = [2, 3, 'xyz'];

    if (msg == null || msg == "")
        msg = "Data Successfully saved.";

    Swal.fire({
        icon: 'success',
        title: msg,
        showDenyButton: false,
        showCancelButton: false,
        confirmButtonText: 'OK'
    }).then((result) => {

        if (result.isConfirmed && typeof functionName != 'undefined' && functionName != null && functionName != '')
            if (typeof functionParams != 'undefined' && functionParams != null)
                if (Array.isArray(functionParams) && functionParams.length > -1) {
                    this.callback = functionName;
                    this.callback.apply(this, functionParams);
                }
                else this.callback = functionName;
            else {
                ShowLoader(true);
                window.location = functionName;
            }

        /* Read more about isConfirmed, isDenied below
        if (result.isConfirmed) {
            Swal.fire('Saved!', '', 'success')
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        } */

    })
}

function CallBack(fn, data, Id) {
    return fn(data, Id);
}
