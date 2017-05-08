class Cognizance extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            dataState: false,
            hasError: false,
            activeResult: 'review',//error
            uploading: false,
            cognizancing: false,
            errors: []
        };

        this.eventList = {
            closed: 'closed'
        };

        this._bindFuns(['beforeUpload', 'onChange', 'closed', 'downLoadTemplate', 'cognizance']);

        this.paginationOpts = {
            layout: ['sep', 'first', 'prev', 'sep', 'manual', 'sep', 'next', 'last', 'info'],
            onSelectPage: function (pageNumber, pageSize) {
                this.$ExcelImportResult.datagrid('loading');
                $.post(_bl_ + '/Home/GetPageCompanies',
                    {
                        pageNumber: pageNumber,
                        pageSize: pageSize
                    },
                    function (rt) {
                        if (rt.ErrorMessage) {
                            antd.message.error(rt.ErrorMessage);
                        }
                        else {
                            this.$ExcelImportResult.datagrid('loadData', {
                                rows: rt.Data.companies,
                                total: rt.Data.count
                            });
                        }
                        this.$ExcelImportResult.datagrid('loaded');
                    }.bind(this), 'json');
            }.bind(this)
        };
    }

    cognizance() {
        antd.message.info("正在认定中...");
        this.setState({ cognizancing: true });

        $.post(_bl_ + '/Home/ImportQYGM', function (rt) {
            if (rt.ErrorMessage) {
                antd.message.error(rt.ErrorMessage);
            } else {
                antd.message.success("导入成功！");
                this.setState({
                    cognizancing: false,
                    dataState: false,
                    hasError: false,
                    activeResult: 'review',//error
                    uploading: false,
                    errors: []
                });

                this.$ExcelImportResult.datagrid({ pageNumber: 1 }).datagrid('getPager').pagination(this.paginationOpts);
                this.$ExcelImportResult.datagrid('loadData', {
                    total: 0,
                    rows: []
                });
            }
        }.bind(this), 'json');
    }

    beforeUpload(file) {
        var splits = file.name.split('.');
        var bExcel = splits[1] === 'xls' || splits[1] === 'xlsx';
        if (!bExcel) {
            antd.message.error("只能上传Excel格式的数据！");
            this.setState({ fileName: '' })
        } else {
            this.setState({
                fileName: file.name,
                uploading: true
            });
        }
        return bExcel;
    }

    onChange(e) {
        this.$ExcelImportResult.datagrid('loading');
        var file = e.file;
        if (file.status === "uploading") {
            antd.message.info("文件上传中...");
        } else if (file.status === "done") {
            antd.message.success("文件上传成功！");
            var rt = file.response;
            if (rt.ErrorMessage) {
                antd.message.error(rt.ErrorMessage);
            } else {
                var companies = rt.Data.companies;
                var errors = rt.Data.errors;
                this.setState({ dataState: errors.length === 0, hasError: errors.length !== 0, errors: errors, uploading: false });
                this.$ExcelImportResult.datagrid({ pageNumber: 1 }).datagrid('getPager').pagination(this.paginationOpts);
                this.$ExcelImportResult.datagrid('loadData', {
                    total: rt.Data.count,
                    rows: companies
                });
            }
            this.$ExcelImportResult.datagrid('loaded');
        } else if (file.status === "error") {
            antd.message.error("文件上传失败...");
            this.$ExcelImportResult.datagrid('loaded');
            this.setState({ dataState: false, hasError: false, uploading: false });
        }
    }

    getErrorResult() {
        var errors = this.state.errors;
        var cErrors = errors.map(function (er, i) {
            return (
                <div className="error-item">
                    <div dangerouslySetInnerHTML={{ __html: (i + 1) + "、" + er.Title}} className="error-item-header">
                    </div>
                    <div className="error-item-errordetails">
                        {er.ErrorMessages.map(function (c, j) {
                            return <div dangerouslySetInnerHTML={{ __html: "（" + (j + 1) + "） " + c }} className="error-item-errordetail"></div>
                        })}
                    </div>
                </div>
                );
        });

        return cErrors;
    }

    downLoadTemplate() {
        window.open(_bl_ + '/Documents/企业规模认定导入模板.xls');
    }

    componentDidMount() {
        this.$ExcelImportResult = $('#excel_import_result').datagrid(
            {
                loadMsg: '正在上传并验证数据，请稍后...',
                pagination: true,
                pageSize: 10,
                singleSelect: true,
                frozenColumns: [[
                    { field: 'INDEX', title: '', align: 'center' },
                    { field: 'NSRSBH', halign: 'center', title: '纳税人识别号' },
                    { field: 'NSRMC', halign: 'center', title: '纳税人名称' }
                ]],
                columns: [[
                    { field: 'SHXYDM', halign: 'center', title: '社会信用代码' },
                    { field: 'ZGSWJ_DM', halign: 'center', title: '主管税务机构代码' },
                    { field: 'SSND', halign: 'center', title: '税收年度' },
                    { field: 'QYGMMC', halign: 'center', title: '企业规模' }
                ]]
            }).datagrid('loadData', []);

        this.$ExcelImportResult.datagrid('getPager').pagination(this.paginationOpts);

        $('.cognizance').draggable({
            handle: '#cognizance_title'
        });
    }

    closed() {
        this.fire(this.eventList.closed, null, false);
        $.post(_bl_ + "/Home/ClearUploadDatas", function (rt) {
            if (rt.ErrorMessage) {
                antd.message.error(rt.ErrorMessage);
            }
        }, 'json');
    }

    render() {
        var s = this.state;
        var cErrors = this.getErrorResult();

        return (
            <div className="cognizance">
                <antd.Icon className="btn-close" type="close-circle" onClick={this.closed} />
                <div onClick={e=>this._bringToFront()} id="cognizance_title" className="cognizance-title">企业规模认定</div>
                <table>
                    <tr>
                        <th style={{width:"30%"}}>认定模式</th>
                        <td style={{ width: "70%" }}>
                            <antd.Select onChange={e=>this.rdms=e} size="large" defaultValue="import" style={{ width: 200 }}>
                                  <antd.Select.Option className="ct-option" value="import">外部导入</antd.Select.Option>
                            </antd.Select>
                        </td>
                    </tr>
                    <tr>
                        <th>文件选择</th>
                        <td>
                            <antd.Input size='large' style={{ width: '200px' }} disabled value={s.fileName || "尚未选择文件"} />
                            <antd.Upload disabled={s.uploading} onChange={this.onChange} beforeUpload={this.beforeUpload} action={_bl_+"/Home/UploadExcel"} multiple={false} showUploadList={false}>
                                <antd.Button size='large' type="primary" loading={s.uploading}>
                                    点击上传
                                </antd.Button>
                            </antd.Upload>
                        </td>
                    </tr>
                    <tr>
                        <th>数据状态</th>
                        <td>
                            {(s.dataState === false ? (<span className="unverified"><antd.Icon type="close-circle" />尚未验证</span>) : (<span className="verified"><antd.Icon type="check-circle" />已验证</span>))}
                        </td>
                    </tr>
                    <tr>
                        <td colSpan={2} style={{ "text-align": "center" }}>
                        <antd.Button onClick={this.downLoadTemplate} size="large" type="primary">模板下载</antd.Button>
                            &ensp;
                            &emsp;
                        <antd.Button loading={s.cognizancing} onClick={this.cognizance} disabled={!s.dataState} size="large" type="primary">开始认证</antd.Button>
                        </td>
                    </tr>
                </table>
                <div className="table-panel">
                    <ul className="result-nav">
                        <li onClick={e=>this.setState({activeResult:"review"})} className={this._getClass(s.activeResult==="review")}>数据预览</li>
                        <li onClick={e=>this.setState({activeResult:"error"})} className={this._getClass(s.activeResult === "error")}>错误信息</li>
                    </ul>
                    <div className={this._getClass(s.activeResult==="review")}>
                        <table id="excel_import_result" style={{ height: "260px" }}></table>
                    </div>
                    <div className={this._getClass(s.activeResult === "error")}>
                        {s.hasError ? '' : (<div className="none-error-tip">无错误信息</div>)}
                        {cErrors}
                    </div>
                </div>
            </div>
            );
    }
}