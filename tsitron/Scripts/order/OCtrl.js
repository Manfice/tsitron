$(function () {
    if (!Modernizr.inputtypes.date) {
        $(function () {
            $("input[type='date']")
                .datepicker({ dateFormat: 'dd/mm/yy' })
                .get(0).setAttribute("type", "text");
        });
        $.datepicker.regional['ru'] = {
            closeText: 'Закрыть',
            prevText: 'Пред',
            nextText: 'След',
            currentText: 'Сегодня',
            monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
            'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
            monthNamesShort: ['Янв', 'Фев', 'Мар', 'Апр', 'Май', 'Июн',
            'Июл', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
            dayNames: ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'],
            dayNamesShort: ['вск', 'пнд', 'втр', 'срд', 'чтв', 'птн', 'сбт'],
            dayNamesMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
            weekHeader: 'Не',
            dateFormat: 'dd.mm.yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['ru']);
    };
});

function getOrder() {
    $.ajax({
        url: "/Home/getOrder",
        type: "get",
        success: function (data) {
            order.recipient.date(data.WishDate);
            order.bouquet.id(data.Marker);
            order.bouquet.avatar.path(data.Avatar.Path);
            order.bouquet.avatar.alt(data.Avatar.Alt);
            order.bouquet.price(data.Bouquet.Price);
            order.bouquet.quantity(data.Bouquet.Quantity);
            order.bouquet.title(data.Bouquet.Title);
        }
    });
};

var acceptOrder = function () {
    $.ajax({
        data: order,
        url: "/api/WHome/MakeOrder/",
        type: "POST",
        success: function(data) {
            window.location = "/security/thankyou";
        }
});
};

$(document).ready(function () {
    ko.applyBindings();
    getOrder();
});