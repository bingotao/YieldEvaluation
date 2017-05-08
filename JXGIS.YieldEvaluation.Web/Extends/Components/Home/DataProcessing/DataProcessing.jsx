class DataProcessing extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            processType: '全库',
            departments: []
        };

        this.eventList = {
            closed: 'closed'
        };
        this._bindFuns(['closed', 'process']);

        this.sjnf = 2016;
    }

    process() {
        antd.message.info("数据正在加工中，请稍后...");
        var condition = this.getCondition();
        $.post(_bl_ + '/Home/DataProcess', condition, function (rt) {
            if (rt.ErrorMessage) {
                antd.message.error(rt.ErrorMessage);
            } else {
                antd.message.info(rt.Data);
                this.fire(this.eventList.closed);
            }
        }.bind(this), 'json');
    }

    getCondition() {
        return {
            sjnf: this.sjnf,
            jgms: this.state.processType,
            swbm: this.swbm,
            dsbm: this.dsbm
        };
    }

    getDepartments() {
        var departments = this.state.departments;
        this.swbm = departments.length ? departments[0].SWJG_DM : null;

        var cOptions = departments.map(function (d) {
            return <antd.Select.Option className="ct-option" value={d.SWJG_DM }>{d.SWJGMC}</antd.Select.Option>;
        });

        var cSelect =
            <antd.Select onChange={e=>this.swbm = e} size="large" defaultValue={this.swbm} style={{ width: 200 }}>
                {cOptions}
            </antd.Select>;
        return cSelect;
    }

    componentDidMount() {
        $('.dataprocessing').draggable({
            handle: '#dataprocessing_header'
        });

        $.post(_bl_ + '/Home/GetDepartments', function (rt) {
            if (rt.ErrorMessage) {
                antd.message.error(rt.ErrorMessage);
            }
            else {
                var departments = rt.Data;
                this.setState({ departments: departments });
            }
        }.bind(this), 'json');
    }

    closed() {
        this.fire(this.eventList.closed, null, false);
    }

    render() {
        var s = this.state;
        var cDepartments = this.getDepartments();

        return (
        <div className="dataprocessing">
            <antd.Icon className="btn-close" type="close-circle" onClick={this.closed} />
            <div id="dataprocessing_header">亩产数据加工</div>
            <div>
                <table>
                    <tr>
                        <th>数据年份</th>
                        <td>
                            <antd.Select onChange={e=>this.sjnf = e} size="large" defaultValue={this.sjnf} style={{ width: 200 }}>
                                <antd.Select.Option className="ct-option" value="2016">2016年</antd.Select.Option>
                            </antd.Select>
                        </td>
                    </tr>
                    <tr>
                        <th>加工模式</th>
                        <td>
                            <antd.Select value={s.processType} onChange={e=>this.setState({ processType: e })} size="large" defaultValue={s.processType} style={{ width: 200 }}>
                                <antd.Select.Option className="ct-option" value="全库">全库</antd.Select.Option>
                                <antd.Select.Option className="ct-option" value="单户">单户</antd.Select.Option>
                                <antd.Select.Option className="ct-option" value="税务部门">税务部门</antd.Select.Option>
                            </antd.Select>
                        </td>
                    </tr>
                    {
                        s.processType === '单户' ?
                            (
                            <tr>
                                 <th>地税编码</th>
                                 <td>
                                     <antd.Input size="large" style={{ width:200 }} onChange={e => this.dsbm = e.target.value} />
                                 </td>
                            </tr>)
                            : null
                    }
                    {
                        s.processType === '税务部门' ?
                        (
                        <tr>
                         <th>税务部门</th>
                         <td>
                             { cDepartments}
                         </td>
                        </tr>)
                        : null
                    }
                    <tr>
                        <td colSpan="2">
                            <antd.Button size="large" type="primary" onClick={this.process}>加工</antd.Button>
                        </td>
                    </tr>
                </table>
            </div>
        </div>);
    }
}