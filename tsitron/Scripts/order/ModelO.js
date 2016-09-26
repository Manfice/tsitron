var order = {
    bouquet: {
        id: ko.observable(),
        title: ko.observable(),
        quantity: ko.observable(1),
        price: ko.observable(),
        avatar: {
            path: ko.observable(),
            alt:ko.observable('pic')
        }
    },
    customer: {
        id: ko.observable(),
        title:ko.observable('Вася'),
        telephone: ko.observable('+79034441616'),
        email: ko.observable('c592@ya.ru')
    },
    recipient: {
        id: ko.observable(),
        name: ko.observable('Ольга'),
        phone: ko.observable('+79886789966'),
        recipientMe: ko.observable(false),
        adres: ko.observable('г. Ставрополь, ул. Пирогова, 55а'),
        date: ko.observable(''),
        UnknownAddress: ko.observable(false),
        incognito: ko.observable(true),
        comment: ko.observable(''),
        delivery: ko.observable(0)
    },
    orderDate: ko.observable(),
    info: ko.observable('')
};

var Bm = function() {
    var self = this;
    self.bouquet = {
        id: ko.observable(),
        title: ko.observable(),
        quantity: ko.observable(1),
        price: ko.observable(),
        avatar: {
            path: ko.observable(),
            alt: ko.observable('pic')
        }
    };
    self.customer = {
        id: ko.observable(),
        title: ko.observable(),
        telephone: ko.observable(),
        email: ko.observable()
    };
    self.recipient = {
        id: ko.observable(),
        name: ko.observable(),
        phone: ko.observable(),
        recipientMe: ko.observable(false),
        adres: ko.observable(),
        date: ko.observable(),
        UnknownAddress: ko.observable(false),
        incognito: ko.observable(),
        comment: ko.observable(),
        delivery: ko.observable(0)
    };
    self.orderDate = ko.observable();
    self.info = ko.observable();
}
