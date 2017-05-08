class HomeIndex extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            showCognizance: false,
            showEnterpriseSort: false,
            showSortResult: false,
            showDataProcessing: false,
            showSingleCognizance: false,
            showProcessingResult: false,
            showLogin: true
        };
        this._bindFuns(["closeSortResult"]);
    }

    closeSortResult() {
        this.setState({ showSortResult: false });
    }

    componentDidMount() {
        var refs = this.refs;
        var cTools = refs.cTools;

        cTools.on(cTools.eventList.cognizance, function () {
            this.setState({ showCognizance: true }, function () {
                var cCognizance = this.refs.cCognizance;
                cCognizance && cCognizance.on(cCognizance.eventList.closed, function () {
                    this.setState({ showCognizance: false });
                }.bind(this));
                cCognizance._bringToFront();
            }.bind(this));
        }.bind(this));

        cTools.on(cTools.eventList.enterpriseSort, function () {
            this.setState({ showEnterpriseSort: true }, function () {
                var cEnterpriseSort = this.refs.cEnterpriseSort;
                cEnterpriseSort && cEnterpriseSort.on(cEnterpriseSort.eventList.closed, function () {
                    this.setState({ showEnterpriseSort: false });
                }.bind(this));

                cEnterpriseSort && cEnterpriseSort.on(cEnterpriseSort.eventList.OKClick, function (e) {
                    var condition = e.data;
                    $.post(_bl_ + '/Home/UpSortCondition', condition, function (rt) {
                        if (rt.ErrorMessage) {
                            antd.message.error(rt.ErrorMessage);
                        }
                        else {
                            this.setState({ showSortResult: true });
                        }
                    }.bind(this));
                }.bind(this));

                cEnterpriseSort._bringToFront();
            }.bind(this));
        }.bind(this));

        cTools.on(cTools.eventList.dataProcessing, function (e) {
            var type = e.data.dataProcessingType;
            this.setState({ showDataProcessing: true }, function () {
                var cDataProcessing = this.refs.cDataProcessing;
                cDataProcessing.setState({ processType: type });
                cDataProcessing && cDataProcessing.on(cDataProcessing.eventList.closed, function () {
                    this.setState({ showDataProcessing: false });
                }.bind(this));
            });
        }.bind(this));

        cTools.on(cTools.eventList.singleCognizance, function () {
            this.setState({ showSingleCognizance: true }, function () {
                var cSingleCognizance = this.refs.cSingleCognizance;
                cSingleCognizance && cSingleCognizance.on(cSingleCognizance.eventList.closed, function () {
                    this.setState({ showSingleCognizance: false });
                }.bind(this));
            });
        }.bind(this));

        cTools.on(cTools.eventList.processingResult, function () {
            this.setState({ showProcessingResult: true }, function () {
                var cProcessingResult = this.refs.cProcessingResult;
                cProcessingResult && cProcessingResult.on(cProcessingResult.eventList.closed, function () {
                    this.setState({ showProcessingResult: false });
                }.bind(this));
            });
        }.bind(this));

        if (refs.cLogin) {
            var cLogin = refs.cLogin;
            cLogin.on(cLogin.eventList.login, function (rt) {
                if (rt.data) {
                    this.setState({ showLogin: false });
                }
            }, this);
        }
    }

    render() {
        var s = this.state;
        return (
        <div className="home-index">
            <div className="home-index-header"></div>
            <div className="home-index-body">
                <Tools ref='cTools' />
                <MainMap />
            </div>
            {s.showCognizance && (<Cognizance ref='cCognizance' />)}
            {s.showEnterpriseSort && (<EnterpriseSort ref="cEnterpriseSort" />)}
            {s.showDataProcessing && (<DataProcessing ref="cDataProcessing" />)}
            {s.showSingleCognizance && (<SingleCognizance ref="cSingleCognizance" />)}
            {s.showProcessingResult && (<ProcessingResult ref="cProcessingResult" />)}
            {s.showLogin && (<Login ref="cLogin" />)}
            <div className={"sort-result "+this._getClass(s.showSortResult)}>
                <antd.Icon type="close-circle" className="btn-close" onClick={this.closeSortResult} />
                {s.showSortResult ? (<iframe frameBorder="0" id="sortFrame" src={_bl_ + "/Home/EnterpriseSortPage" }></iframe>) : null}
            </div>
        </div>
       );
    }
}
$(function () {
    var app = ReactDOM.render(<HomeIndex />, document.getElementById('app'));
});
