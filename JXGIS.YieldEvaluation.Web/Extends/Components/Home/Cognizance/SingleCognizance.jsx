class SingleCognizance extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            showPanel: false,
            entity: {},
            departments: []
        };

        this.eventList = { closed: 'closed' };
        this._bindFuns(['closed', 'newRecord', 'save', 'getQYGM']);

        this.newEntity = this.getNewEntity();
        this.ssnd = 2016;
        this.zgswj_dm = 'all';
    }

    getConditions() {
        return {
            ssnd: this.ssnd,
            zgswj_dm: this.zgswj_dm,
            nsrmc: this.nsrmc,
            nsrsbh: this.nsrsbh
        };
    }

    save() {
        var newEntity = this.newEntity;
        var oldEntity = this.state.entity;
        $.post(_bl_ + "/Home/SaveQYGM", { oldEntity: oldEntity, newEntity: newEntity }, function (rt) {
            if (rt.ErrorMessage) {
                antd.message.error(rt.ErrorMessage);
            }
            else {
                antd.message.success("保存成功！");
                this.setState({ showPanel: false });
                this.getQYGM();
            }
        }.bind(this), 'json');
    }

    closed() {
        this.fire(this.eventList.closed, null, false);
    }

    getDepartments(bDefault) {
        var departments = this.state.departments;

        var cOptions = departments.map(function (d) {
            return <antd.Select.Option className="ct-option" value={d.SWJG_DM }>{d.SWJGMC}</antd.Select.Option>;
        });

        var cSelect =
            <antd.Select size="large" onSelect={e=>this.zgswj_dm=e} defaultValue={this.zgswj_dm} style={{ width: 200 }}>
                <antd.Select.Option className="ct-option" value={'all' }>{'全部'}</antd.Select.Option>
                {cOptions}
            </antd.Select>;
        return cSelect;
                }

    getDepartments2() {
                    var departments = this.state.departments;
                    var entity = this.state.entity;
                    this.newEntity.ZGSWJ_DM = entity.ZGSWJ_DM ? entity.ZGSWJ_DM : '';
                    var cOptions = departments.map(function (d) {
                        return <antd.Select.Option className="ct-option" value={d.SWJG_DM }>{d.SWJGMC}</antd.Select.Option>;
                    });

                    var cSelect =
                        <antd.Select onSelect={e=>this.newEntity.ZGSWJ_DM = e} size="large" defaultValue={this.newEntity.ZGSWJ_DM} style={{ width: 200 }}>
    {cOptions}
</antd.Select>;
return cSelect;
    }

getNewEntity() {
        return { SSND: 2016, QYGM: 0 };
    }

    newRecord() {
        this.newEntity = this.getNewEntity();
        this.setState({ entity: {}, showPanel: true });
    }

    componentDidMount() {
        $('.singlecognizance').draggable({
            handle: '#singlecognizance_header'
        });

        $.post(_bl_ + '/Home/GetDepartments', function (rt) {
            if (rt.ErrorMessage) {
                antd.message.error(rt.ErrorMessage);
            } else {
                this.setState({ departments: rt.Data });
            }
        }.bind(this), 'json');

        var config = {
            singleSelect: true,
            rownumbers: true,
            pagination: true,
            pageSize: 10,
            pageList: [10],
            columns: [[
                { field: 'NSRSBH', title: '纳税人识别号', halign: 'center', width: 160 },
                { field: 'NSRMC', title: '纳税人名称', halign: 'center', width: 220 },
                { field: 'SHXYDM', title: '社会信用代码', halign: 'center', width: 160 },
                { field: 'SSND', title: '所属年度', halign: 'center', width: 80 },
                { field: 'ZGSWJ_DM', title: '主管税务局代码', halign: 'center', width: 160 },
                { field: 'QYGMMC', title: '企业规模', halign: 'center', width: 80 }
            ]],
            toolbar: [{
                text: '新增',
                iconCls: 'anticon anticon-plus',
                handler: this.newRecord
            }, '-', {
                text: '删除',
                iconCls: 'anticon anticon-minus',
                handler: function () {
                    var entity = this.$datagrid.datagrid('getSelected');
                    if (!entity) {
                        antd.message.error("请选择要删除的数据！");
                        return false;
                    }
                    $.messager.confirm('提醒', '确定删除该条记录？', function (r) {
                        if (r) {
                            $.post(_bl_ + '/Home/DeleteQYGM', entity, function (rt) {
                                if (rt.ErrorMessage) {
                                    antd.message.error(rt.ErrorMessage);
                                } else {
                                    antd.message.success("删除成功！");
                                    this.getQYGM();
                                }
                            }.bind(this));
                        }
                    }.bind(this));
                }.bind(this)
            }, '-', {
                text: '修改',
                iconCls: 'anticon anticon-edit',
                handler: function () {
                    var entity = this.$datagrid.datagrid('getSelected');
                    if (!entity) antd.message.error("请选择要编辑的数据！");
                    else {
                        this.newEntity={
                            NSRSBH:entity.NSRSBH,
                            NSRMC:entity.NSRMC,
                            SHXYDM:entity.SHXYDM,
                            ZGSWJ_DM:entity.ZGSWJ_DM,
                            SSND:entity.SSND,
                            QYGM:entity.QYGM
                        };
                        
                        this.setState({ entity: entity, showPanel: true });
                    }
                }.bind(this)
            }]
        };


        config.queryParams = this.getConditions();
        config.url = _bl_ + "/Home/GetQYGM";

        this.datagridConfig = config;
        var $datagrid = $('#singlecognizance_grid').datagrid(config);
        this.$datagrid = $datagrid;
    }

    getQYGM() {
        this.datagridConfig.queryParams = this.getConditions();
        this.datagridConfig.url = _bl_ + "/Home/GetQYGM";
        this.$datagrid.datagrid(this.datagridConfig);
    }

    render() {
        var s = this.state;
        var e = s.entity;

        return (
        <div className="singlecognizance">
            <antd.Icon className="btn-close" type="close-circle" onClick={this.closed} />
            <div id="singlecognizance_header">企业规模认定</div>
            <div className="singlecognizance-conditions">
                <table>
                    <tr>
                        <th>所属年度</th>
                        <td>
                            <antd.Select size="large" onSelect={e=>this.ssnd = e} defaultValue={this.ssnd} style={{ width: 200 }}>
                                <antd.Select.Option className="ct-option" value="">2016</antd.Select.Option>
                            </antd.Select>
                        </td>
                        <th>税务部门</th>
                        <td>{this.getDepartments()}</td>
                                    </tr>
                                    <tr>
                                        <th>纳税人识别号</th>
                                        <td>
                                            <antd.Input onChange={e=>this.nsrsbh=e.target.value} size="large" style={{ width: 200 }} />
                        </td>
                        <th>企业名称</th>
                        <td>
                            <antd.Input onChange={e=>this.nsrmc=e.target.value} size="large" style={{ width: 200 }} />
                        </td>
                    </tr>
                    <tr>
                        <td colSpan='4' style={{ 'text-align': 'center' }}>
                        <antd.Button onClick={this.getQYGM} type="primary" size='large'>查询</antd.Button>
                        </td>
                    </tr>
                </table>
            </div>
            <div className="singlecognizance-results">
                <table id="singlecognizance_grid" style={{ height: 315 }}></table>
            </div>
                        {
            s.showPanel
            &&
            <div className="singlecognizance-panel">
                <div>{e.NSRSBH ? '修改单户企业' : '新增单户企业'}</div>
                <table>
                    <tr>
                        <th>所属年度</th>
                        <td>
                            <antd.Select size="large" onSelect={e=>this.newEntity.SSND=e} defaultValue={e.SSND||2016} style={{ width: 200 }}>
                                <antd.Select.Option className="ct-option" value={2016}>2016年</antd.Select.Option>
                            </antd.Select>
                        </td>
                    </tr>
                    <tr>
                        <th>社会信用代码</th>
                        <td>
                            <antd.Input onChange={e=>this.newEntity.SHXYDM=e.target.value} style={{ width: 200 }} defaultValue={e.SHXYDM} />
                        </td>
                    </tr>
                    <tr>
                        <th>纳税人识别号</th>
                        <td>
                            <antd.Input onChange={e=>this.newEntity.NSRSBH = e.target.value} style={{ width: 200 }} defaultValue={e.NSRSBH} />
                        </td>
                    </tr>
                    <tr>
                        <th>企业名称</th>
                        <td>
                            <antd.Input onChange={e=>this.newEntity.NSRMC = e.target.value} style={{ width: 200 }} defaultValue={e.NSRMC} />
                        </td>
                    </tr>
                    <tr>
                        <th>税务部门编码</th>
                        <td>
                        {this.getDepartments2()}
                        </td>

                            </tr>
                            <tr>
                                <th>企业规模</th>
                                <td>
                                    <antd.Select onSelect={e=>this.newEntity.QYGM=e} size="large" defaultValue={e.QYGM?e.QYGM:0} style={{ width: 200 }}>
                                <antd.Select.Option className="ct-option" value={1}>规&ensp;上</antd.Select.Option>
                                <antd.Select.Option className="ct-option" value={2}>规&ensp;下</antd.Select.Option>
                                <antd.Select.Option className="ct-option" value={0}>未确定</antd.Select.Option>
                            </antd.Select>
                        </td>
                    </tr>
                    <tr>
                        <td colSpan='2' style={{ "text-align": "center" }}>
                            <antd.Button onClick={this.save} type="primary">保存</antd.Button>
                            &emsp;
                            &emsp;
                        <antd.Button onClick={e=>this.setState({ showPanel: false })} type="default">取消</antd.Button>
                        </td>
                    </tr>
                </table>
            </div>    }
        </div>);
                                }
}