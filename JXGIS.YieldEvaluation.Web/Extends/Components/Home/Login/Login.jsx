class Login extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            userActive: false,
            passwordActive: false
        };
        this.eventList = { login: 'login' };
        this._bindFuns(['login']);
    }

    login() {
        var userName = this.refs.user.value;
        var password = this.refs.password.value;
        if (!userName || !password) {
            antd.message.error("请填写用户名密码！");
        } else {
            userName = userName.trim();
            password = password.trim();

            $.post(_bl_ + "/Common/Login",
                {
                    userName: userName,
                    password: password
                }, function (rt) {
                    if (rt.ErrorMessage || !rt.Data) {
                        antd.message.error("登录失败，用户名密码错误！");
                    } else {
                        antd.message.info("登录成功");
                    }
                    this.fire(this.eventList.login, rt.Data, false);
                }.bind(this), 'json')
        }
    }

    componentDidMount() {
        $.post(_bl_ + "/Common/Login",
                function (rt) {
                    this.fire(this.eventList.login, rt.Data, false);
                }.bind(this), 'json');
    }

    render() {
        var s = this.state;

        return (
        <div className="login">
            <form name="login">
                <div className="login-content">
                    <div>亩产税收管理系统</div>
                    <div className={"group " + this._getClass(s.userActive)}>
                        <antd.Icon type="user" />
                        <input ref="user" onFocus={e=>this.setState({ userActive: true })} onBlur={e=>this.setState({ userActive: false })} name="UserName" type="text" placeholder="请输入用户名" />
                    </div>
                    <div className={"group " + this._getClass(s.passwordActive)}>
                        <antd.Icon type="key" />
                        <input ref="password" onFocus={e=>this.setState({passwordActive:true})} onBlur={e=>this.setState({passwordActive:false})} name="Password" type="password" placeholder="请输入密码" />
                    </div>
                    <span onClick={this.login} className="bb-comment-btn"><span className="glyphicon glyphicon-log-in"></span>&nbsp;&nbsp;&nbsp;&nbsp;登&nbsp;&nbsp;录</span>
                </div>
            </form>
        </div>
            );
    }
}