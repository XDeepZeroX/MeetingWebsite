function sendForm(form, event, then, onStart) {
    event.preventDefault();
    if (onStart != undefined)
        onStart();

    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        complete: (response) => {
            if (then != undefined)
                then(response);
        },
        data: $(form).serialize()
    });
}