class Tools extends React.Component {
    constructor(props) {
        super(props);
        this.state = {};
        this.eventList = {
            cognizance: 'cognizance',
            enterpriseSort: 'enterpriseSort',
            dataProcessing: 'dataProcessing',
            singleCognizance: 'singleCognizance',
            processingResult: 'processingResult'
        };

        this._bindFuns(['cognizance', 'enterpriseSort', 'dataProcessing', 'singleCognizance', 'processingResult']);

        this.dataProcessingType = {
            dh: '单户',
            fj: '税务部门',
            qk: '全库'
        };
    }

    cognizance() {
        this.fire(this.eventList.cognizance, null, false);
    }

    enterpriseSort() {
        this.fire(this.eventList.enterpriseSort, null, false);
    }

    dataProcessing(type) {
        this.fire(this.eventList.dataProcessing, { dataProcessingType: type }, false);
    }

    singleCognizance() {
        this.fire(this.eventList.singleCognizance, null, false);
    }

    processingResult() {
        this.fire(this.eventList.processingResult, null, false);
    }

    render() {
        var s = this.state;

        return (
        <div className="tools">
            <ul>
                <li>
                    <span>
                        <antd.Icon type="book" />企业清册
                    </span>
                    <ul>
                        <li>所有正常企业清册</li>
                        <li>参与排名企业清册</li>
                        <li className="split"></li>
                        <li>按地税编码查询</li>
                        <li>按企业名称查询</li>
                        <li>按所属行业查询</li>
                        <li>按管理分局查询</li>
                        <li className="split"></li>
                        <li>按土地坐落查询</li>
                        <li>按土地面积查询</li>
                        <li>按土地等级查询</li>
                        <li>按企业名称查询</li>
                    </ul>
                </li>
                <li>
                    <span>
                        <antd.Icon type="area-chart" />亩产排名
                    </span>
                    <ul>
                        <li onClick={this.singleCognizance}>单户排名企业认定</li>
                        <li onClick={this.cognizance}>分局排名企业认定</li>
                        <li onClick={this.cognizance}>全部排名企业认定</li>
                        <li className="split"></li>
                        <li onClick={e=>this.dataProcessing(this.dataProcessingType.dh)}>单户企业数据加工</li>
                        <li onClick={e=>this.dataProcessing(this.dataProcessingType.fj)}>分局企业数据加工</li>
                        <li onClick={e=>this.dataProcessing(this.dataProcessingType.qk)}>全部企业数据加工</li>
                        <li onClick={this.processingResult}>加工进度查看</li>
                        <li className="split"></li>
                        <li onClick={this.enterpriseSort}>亩产税收排名查询</li>
                        <li>亩产税收图表生成</li>
                    </ul>
                </li>
                <li>
                    <span>
                        <antd.Icon type="environment" />地图定位
                    </span>
                    <ul>
                        <li>企业地址定位</li>
                        <li>土地地址定位</li>
                        <li>门牌地址查询</li>
                        <li>企业地址定位历史</li>
                    </ul>
                </li>
                <li>
                    <span>
                        <antd.Icon type="global" />
                        地图管理
                    </span>
                    <ul>
                        <li>企业迁址模拟定点</li>
                        <li>税收同比图警示指标设置</li>
                        <li className="split"></li>
                        <li>距离测量</li>
                        <li>面积测量</li>
                        <li>地图打印</li>
                        <li>清除图册</li>
                        <li>书签管理</li>
                        <li>地图标注绘制管理</li>
                    </ul>
                </li>
                <li>
                    <span>
                        <antd.Icon type="user" />
                        用户管理
                    </span>
                    <ul>
                        <li>用户账号管理</li>
                        <li>用户权限管理</li>
                        <li>龙版用户同步</li>
                        <li className="split"></li>
                        <li>重新登录</li>
                    </ul>
                </li>
            </ul>
        </div>);
    }
}