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
                IsNew: true,
                UserId: -1,
                UserName: "",
                EnumValue: -1
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
            if (id == 0) {
                self.model.editor.IsNew(true);
                self.model.editor.UserName("");
                self.model.editor.UserId(-1);
                self.model.editor.EnumValue(-1);
            } else {
                store.get(id, function (data) {
                    if (data != null) {
                        ko.mapping.fromJS(data, {}, self.model.editor);
                        self.model.editor.IsNew(false);
                    }
                });
            }
            $("#myModal").modal("show");
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
        },
        save: function () {
            var data = ko.mapping.toJS(this.model.editor);
            delete data.IsNew;
            var self = this;

            if (data.UserId == -1) {
                store.create(data, function (result) {
                    if (result != null) {
                        self.list();
                    }
                });
            } else {
                store.update(data, function (result) {
                    if (result != null) {
                        self.list();
                    }
                });
            }
            $("#myModal").modal("hide");
        }
    };

    $(function () {
        viewModel.init();
    })
})(jQuery);