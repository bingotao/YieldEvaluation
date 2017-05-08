class ProcessingResult extends React.Component {
    constructor(props) {
        super(props);
        this.state = {};

        this.eventList = {
            closed: 'closed'
        };
        this._bindFuns(['closed']);
    }

    componentDidMount() {

        $('.processingresult').draggable({
            handle: '#processingresult_header'
        });

        var dgConfig = {
            //url: _bl_ + "/Home/GetPLOCK",
            url: _bl_ + "/Home/GetSJJGLS",

            rownumbers: true,
            //columns: [
            //    [
            //        { field: "PNAME", title: "过程名称", halign: "center", width: 100 },
            //        { field: "LOCKSTATE", title: "锁定状态", halign: "center", width: 100 },
            //        { field: "MEMO", title: "明细", halign: "center" }
            //    ]
            //],
            columns: [
               [
                   { field: "LOGDATE", title: "时间", halign: "center" },
                   { field: "LOGTYPE", title: "类型", halign: "center" },
                   { field: "LOGCONTENT", title: "内容", halign: "center" }
               ]
            ]
        };

        $('#processing_result').datagrid(dgConfig);
    }

    closed() {
        this.fire(this.eventList.closed, null, false);
    }

    render() {
        var s = this.state;

        return (
            <div className="processingresult">
                <antd.Icon className="btn-close" type="close-circle" onClick={this.closed} />
                <div id="processingresult_header">加工进度查看</div>
                <table id="processing_result" style={{ height: '250px' }}></table>
            </div>
            );
    }
}