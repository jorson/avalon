(function ($) {
    var store = {
        //创建
        create: function (data, success) {
            $.ajax({
                url: "/curd/create",
                type: "post",
                dateType: "json",
                data: data,
                success: success
            });
        },
        //更新
        update: function (data, success) {
            $.ajax({
                url: "/curd/update",
                type: "post",
                dataType: "json",
                data: data,
                success: success
            });
        },
        //删除
        del: function (id, success) {
            $.ajax({
                url: "/curd/delete",
                type: "get",
                dataType: "json",
                cache: false,
                data: {id: id},
                success: success
            });
        },
        //获取单个
        get: function (id, success) {
            $.ajax({
                url: "/curd/get",
                type: "get",
                dataType: "json",
                cache: false,
                data: { id: id },
                success: success
            });
        },
        //获取列表
        list: function (search, success) {
            $.ajax({
                url: "/curd/list",
                type: "post",
                dateType: "json",
                cache: false,
                data: search,
                success: success
            });
        }
    };

    var viewModel = {
        model: {
            searcher: {
                userName: "",
                enumValue: -1
            },
            editor: {

            },
            list: []
        },
        //页面初始化函数
        init: function () {
            //将Model进行KO映射
            this.model = ko.mapping.fromJS(this.model);
            ko.applyBindings(this);
            //载入初始化的列表
            this.list();
        },
        list: function () {
            var self = this;
            var search = ko.mapping.toJS(self.model.searcher);
            store.list(search, function (data) {
                self.model.list(data);
            });
        },
        showEditor: function (id) {
            var self = this;
            store.get(id, function (data) {
                if (data == null) {

                }
            });
        },
        deleteItem: function (id) {
            var self = this;
            if (confirm("是否删除数据?")) {
                store.get(id, function (data) {
                    if (data == null) {
                        alert("用户已经不存在了");
                        self.list();
                    } else {
                        store.del(id, function (data) {
                            self.list();
                        })
                    }
                });
            }
        }
    };

    $(function () {
        viewModel.init();
    })
})(jQuery);