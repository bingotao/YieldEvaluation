class EnterpriseSort extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            departments: []
        };
        this.eventList = {
            closed: 'closed',
            OKClick: 'OKClick'
        };
        this._bindFuns(['closed', 'OKClick']);
        this.tjnf = 2016;
        this.qygm = 9;

        var c =
        <div>
            <antd.Select.Option className="ct-option" value={1}>规&ensp;上</antd.Select.Option>
            <antd.Select.Option className="ct-option" value={2}>规&ensp;下</antd.Select.Option>
            <antd.Select.Option className="ct-option" value={0}>未确定</antd.Select.Option>
        </div>;
    }

    closed() {
        this.fire(this.eventList.closed, null, false);
    }

    OKClick() {
        var condition = this.getOptions();
        this.fire(this.eventList.OKClick, condition, false);
    }

    getOptions() {
        return {
            SSND: this.tjnf,
            ZGSWJ_DM: this.swbm,
            SZPQ: this.szpq,
            MJ_Min: this.mj_min,
            MJ_Max: this.mj_max,
            QYGM: this.qygm,
            HYDM: this.refs.industryTree.getIndustries()
        };
    }

    componentDidMount() {
        $('.enterprise-sort').draggable({
            handle: '#enterprise_sort_title'
        });

        $.post(_bl_ + '/Home/GetDepartments', function (rt) {
            if (rt.ErrorMessage) {
                antd.message.error(rt.ErrorMessage);
            } else {
                this.setState({ departments: rt.Data });
            }
        }.bind(this), 'json');
    }

    render() {
        var s = this.state;
        var cDepartments = s.departments.map(function (department, index) {
            return <antd.Select.Option className="ct-option" value={department.SWJG_DM }>{department.SWJGJC}</antd.Select.Option>;
        });
        return (
        <div className="enterprise-sort">
            <antd.Icon className="btn-close" type="close-circle" onClick={this.closed} />
            <div onClick={e=>this._bringToFront()} id="enterprise_sort_title" className="enterprise-sort-title">亩产税收排名</div>
            <div className="enterprise-sort-left">
                <IndustryTree ref="industryTree" />
            </div>
            <div className="enterprise-sort-right">
                <table>
                    <tr>
                        <th style={{width:'40%'}}>统计年份</th>
                        <td style={{width:'60%'}}>
                            <antd.Select onChange={e=>this.tjnf=e} size="large" defaultValue={2016} style={{ width: 180 }}>
                                  <antd.Select.Option className="ct-option" value={2016}>2016&nbsp;年</antd.Select.Option>
                                  <antd.Select.Option className="ct-option" value={2015}>2015&nbsp;年</antd.Select.Option>
                                  <antd.Select.Option className="ct-option" value={2014}>2014&nbsp;年</antd.Select.Option>
                                  <antd.Select.Option className="ct-option" value={2013}>2013&nbsp;年</antd.Select.Option>
                            </antd.Select>
                        </td>
                    </tr>
                    <tr>
                        <th>税务部门</th>
                        <td>
                            <antd.Select onChange={e=>this.swbm=e} size="large" defaultValue="all" style={{ width: 180 }}>
                                <antd.Select.Option className="ct-option" value="all">全&nbsp;部</antd.Select.Option>
                                {cDepartments}
                            </antd.Select>
                        </td>
                    </tr>
                    <tr>
                        <th>所在片区</th>
                        <td>
                            <antd.Input onChange={e=>this.szpq=e.target.value} size="large" />
                        </td>
                    </tr>
                    <tr>
                        <th>面&emsp;&emsp;积</th>
                        <td>
                            <antd.InputNumber min={0} onChange={e=>this.mj_min=e} style={{ width: 65 }} defaultValue={0} size="large"></antd.InputNumber>
                            &ensp;~&ensp;
                            <antd.InputNumber min={0} onChange={e=>this.mj_max=e} style={{ width: 65 }} defaultValue={0} size="large"></antd.InputNumber>
                            &ensp;亩
                        </td>
                    </tr>
                    <tr>
                        <th>企业规模</th>
                        <td>
                            <antd.Select onChange={e=>this.qygm=e} size="large" defaultValue={9} style={{ width: 180 }}>
                                  <antd.Select.Option className="ct-option" value={9}>全&ensp;部</antd.Select.Option>
                            </antd.Select>
                        </td>
                    </tr>
                </table>
                    <div>
                        <antd.Button onClick={this.OKClick} size="large" type="primary">确定</antd.Button>
                        &emsp;
                        <antd.Button onClick={this.closed} size="large">取消</antd.Button>
                    </div>
            </div>
        </div>);
    }
}