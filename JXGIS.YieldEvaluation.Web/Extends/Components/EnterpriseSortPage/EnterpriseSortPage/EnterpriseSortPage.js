$(function () {
    var $app = $("#app");
    var height = $app.height();
    var $sortGrid = $('#sortGrid');
    $sortGrid.height(height);

    var currentRows = 50;
    var sortField = "MCSS";
    var desc = true;

    var dgConfig = {
        title: '亩产税收详情',
        loadMsg: '正在排名，请稍后...',
        rownumbers: false,
        singleSelect: true,
        pagination: true,
        pageList: [50, 100, 200, 300],
        showFooter: true,
        onSortColumn: function (sort, order) {
            sortField = sort;
            desc = order === 'desc';
            initGrid();
            $sortGrid.datagrid('loading');
            $.post(_bl_ + '/Home/GetSortData', { page: 1, rows: currentRows, sortField: sortField, desc: desc, newCondition: false }, loadData, 'json');
        },
        toolbar:
             [{
                 text: '导出Excel',
                 iconCls: 'anticon anticon-file-excel',
                 handler: function () {
                     window.open('ExportSortExcel');
                 }
             }]
        ,
        frozenColumns: [
            [
                {
                    field: 'RN', title: '排名', halign: 'center', align: 'center', width: 50, styler: function (value, row, index) {
                        return 'font-weight:700;';
                    }
                },
                { field: 'NSRMC', title: '企业名称', halign: 'center', align: 'center', width: 260 }
            ]
        ],
        columns: [
            [
                { field: 'SZPQ', title: '所在片区', halign: 'center', width: 200 },
                { field: 'SWJG_DM', title: '管理分局代码', halign: 'center', align: 'center', width: 120 },
                { field: 'SWBMMC', title: '管理分局', halign: 'center', width: 200 },
                { field: 'DHY', title: '大行业', halign: 'center', width: 200 },
                { field: 'HYMC', title: '行业名称', halign: 'center', width: 200 },
                { field: 'NSRSBH', title: '纳税人识别号', halign: 'center', align: "center", width: 160 },
                { field: 'SHXYDM', title: '社会信用代码', halign: 'center', align: "center", width: 160 },
                { field: 'RKND', title: '入库年度', halign: 'center', align: "center", width: 80 },
                //{ field: '', title: '企业规模', halign: 'center' },//无企业规模
                { field: 'ZYYWSR', title: '主营业务收入', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'SSZE', title: '税收总金额', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'YNSSZE', title: '应纳税收总额', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                {
                    field: 'MCSS', title: '亩产税收值', halign: 'center', align: "right", width: 120, styler: function (value, row, index) {
                        return 'color:red;';
                    },
                    sortable: true, order: 'desc'
                },
                {
                    field: 'MCJZ', title: '亩产均值', halign: 'center', align: "right", width: 120, styler: function (value, row, index) {
                        return 'color:red;';
                    },
                    sortable: true, order: 'desc'
                },
                {
                    field: 'MCJZB', title: '亩产均值比', halign: 'center', align: "right", width: 120, styler: function (value, row, index) {
                        return 'color:red;';
                    },
                    sortable: true, order: 'desc'
                },
                { field: 'GS_QYSDS', title: '国税企业所得税', halign: 'center', width: 120, align: "right", sortable: true, order: 'desc' },
                { field: 'GS_ZZS', title: '国税增值税', halign: 'center', width: 120, width: 120, align: "right", sortable: true, order: 'desc' },
                { field: 'MDT', title: '国税免抵调', halign: 'center', width: 120, width: 120, align: "right", sortable: true, order: 'desc' },
                { field: 'ZZS', title: '增值税', halign: 'center', width: 120, width: 120, align: "right", sortable: true, order: 'desc' },
                { field: 'TDSYS', title: '土地使用税', halign: 'center', width: 120, align: "right", sortable: true, order: 'desc' },
                { field: 'FCS', title: '房产税', halign: 'center', width: 120, align: "right", sortable: true, order: 'desc' },
                { field: 'YYS', title: '营业税', halign: 'center', width: 120, align: "right", sortable: true, order: 'desc' },
                { field: 'QYSDS', title: '企业所得税(地税)', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'CJS', title: '城建税', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'YHS', title: '印花税', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'TDZZS', title: '土地增值税', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'JYFFJ', title: '教育费附加', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'DFJYFFJ', title: '地方教育费附加', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'SLJSJJ', title: '水利建设基金', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'WHSYF', title: '文化事业费', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'ZYTDMJ', title: '自用土地面积', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'CUZTDMJ', title: '出租土地面积', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' },
                { field: 'CENGZTDMJ', title: '承租土地面积', halign: 'center', align: "right", width: 120, sortable: true, order: 'desc' }
            ]
        ]
    };


    function initGrid() {
        dgConfig.pageSize = currentRows;
        $sortGrid.datagrid(dgConfig);
        $sortGrid.datagrid('getPager').pagination({
            'onSelectPage': function (page, rows) {
                currentRows = rows;
                $sortGrid.datagrid('loading');
                $.post(_bl_ + '/Home/GetSortData', { page: page, rows: currentRows, sortField: sortField, desc: desc, newCondition: false }, loadData, 'json');
            }
        });
    }

    initGrid();

    $sortGrid.datagrid('loading');
    $.post(_bl_ + '/Home/GetSortData', { page: 1, rows: currentRows, newCondition: true }, loadData, 'json');
    function loadData(rt) {
        if (rt.ErrorMessage) {
            $.messager.alert('错误', rt.ErrorMessage);
        } else {
            $sortGrid.datagrid('loadData', rt.Data);
        }
        $sortGrid.datagrid('loaded');
    }
});