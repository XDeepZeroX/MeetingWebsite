
//Обрабатывает отправку форм
function sendForm(form, event, then, options) {
    event.preventDefault();

    onStart = options?.onStart;
    if (onStart != undefined)
        onStart();

    send(
        $(form).attr('action'),
        $(form).attr('method'),
        then,
        options?.data ?? $(form).serialize()
    );
}

//Обрабатывает отправку запросов на сервер
function send(action, method, then, data) {
    $.ajax({
        url: action,
        method: method,
        complete: (response) => {
            if (then != undefined)
                then(response);
        },
        data: typeof (data) == typeof ('') ? data : JSON.stringify(data),
        contentType: "application/json; charset=UTF-8",
    });
}

//Возвращает время в формате HH:mm
function OnlyTime(date) {
    var minutes = date.getMinutes().toString();
    if (minutes.length == 1)
        minutes = '0' + minutes;

    return `${date.getHours()}:${minutes}`;
}

// Парсит JWT токен
function ParseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
};

//Возвращает информацию о пользователе из localStorage
function GetCurrentUserInfo() {
    return JSON.parse(localStorage.getItem('user-info'));
}

//Генерирует guid
function generateUUID() { // Public Domain/MIT
    var d = new Date().getTime();//Timestamp
    var d2 = (performance && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16;//random number between 0 and 16
        if (d > 0) {//Use timestamp until depleted
            r = (d + r) % 16 | 0;
            d = Math.floor(d / 16);
        } else {//Use microseconds since page-load if supported
            r = (d2 + r) % 16 | 0;
            d2 = Math.floor(d2 / 16);
        }
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
}

//Добавляет текущие параметры запроса к ссылке
function OpenLink(tagA, event) {
    event.preventDefault();
    var uri = $(tagA)[0].href += new URL(window.location.href).search;
    window.open(uri, '_self');
}

//Преобразование формы в JSON
function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}


// Получение информации о пользователе с сервера
function GetUserInfoFromServer() {
    var token = $.cookie("Authorization");
    var userId = ParseJwt(token).Id;

    $.ajax({
        url: `/api/Users/${userId}`,
        async: false,
        complete: response => {
            localStorage.setItem("user-info", JSON.stringify(response.responseJSON));
        }
    });
}

// Сортировка по полю
Array.prototype.sortBy = function (p) {
    return this.slice(0).sort(function (a, b) {
        return (a[p] > b[p]) ? 1 : (a[p] < b[p]) ? -1 : 0;
    });
}

//Object.prototype.get = function (key) {
//    return this[key] ?? '';
//}

function get(dict, keys) {
    var spl = keys.split('>');
    var res = dict[spl[0]] ?? '';
    for (var i = 1; i < spl.length; i++) {
        res = res[spl[i]] ?? '';
    }
    return res;
}

//Log out
function LogOut() {
    $.removeCookie("Authorization");
    window.location = '/Account/Login';
}