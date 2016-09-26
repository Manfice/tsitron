var customerUrl = "/api/webseller/";

var RouteLincks = {
    getCustomer: customerUrl + "GetCustomer/",
    saveCustomerData: customerUrl + "SaveCustomerData/",
    saveReqData: customerUrl + "SaveRequisites",
    saveShopData: customerUrl + "SaveShopData"

}



var getCustomer = function (cust) {
    sendRequest(RouteLincks.getCustomer + cust, "GET", null, function (data) {
        CustomerModel.id(data.id);
        CustomerModel.title(data.title);
        CustomerModel.email(data.email);
        CustomerModel.telephone (data.telephone);
        CustomerModel.location (data.location);
        CustomerModel.ranck (data.ranck);
        var date = new Date(data.register);
        CustomerModel.register(date.toDateString());
        ReqModel.id(data.requisites.id);
        ReqModel.title(data.requisites.title);
        ReqModel.urName(data.requisites.urName);
        ReqModel.inn(data.requisites.inn);
        ReqModel.kpp(data.requisites.kpp);
        ReqModel.address(data.requisites.address);
        ReqModel.bank(data.requisites.bank);
        ReqModel.account(data.requisites.account);
        ReqModel.korrAccount(data.requisites.korrAccount);
        ReqModel.bik(data.requisites.bik);
        Shop.id(data.myShop.id);
        if (data.myShop.title != null) { Shop.title(data.myShop.title) };
        if (data.myShop.startWork != null) { Shop.startWork(data.myShop.startWork) };
        if (data.myShop.endWork != null) { Shop.endWork(data.myShop.endWork) };
        Shop.aroundTheClock(data.myShop.aroundTheClock);
    });
};

var saveCustData = function() {
    sendRequest(RouteLincks.saveCustomerData, "Post", CustomerModel);
    ViewModel.editCust(false);
}

var saveReqData = function () {
    sendRequest(RouteLincks.saveReqData, "Post", ReqModel);
    ViewModel.editReq(false);
}

var saveShopData = function() {
    sendRequest(RouteLincks.saveShopData, "Post", Shop);
    ViewModel.editShop(false);
}

