var CustomerModel =  {
    id : ko.observable(),
    title : ko.observable(''),
    email : ko.observable(''),
    telephone : ko.observable(''),
    location : ko.observable(''),
    ranck : ko.observable(''),
    register: ko.observable('')
}

var ReqModel = {
    id: ko.observable(),
    title: ko.observable('не указан'),
    urName: ko.observable('не указан'),
    inn: ko.observable('не указан'),
    kpp: ko.observable('не указан'),
    address: ko.observable('не указан'),
    bank: ko.observable('не указан'),
    account: ko.observable('не указан'),
    korrAccount: ko.observable('не указан'),
    bik: ko.observable('не указан')
}

var Shop = {
    id: ko.observable(),
    title: ko.observable('не указано'),
    startWork: ko.observable('0'),
    endWork: ko.observable('24'),
    aroundTheClock: ko.observable(false)
}

var ViewModel = {
    editCust: ko.observable(false),
    editShop: ko.observable(false),
    editReq:ko.observable(false)
}
