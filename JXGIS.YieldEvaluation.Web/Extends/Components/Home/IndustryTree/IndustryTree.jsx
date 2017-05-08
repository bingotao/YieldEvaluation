class IndustryTree extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            treeData: [],
            checkedAll: false
        };
        this._bindFuns(["onNodeChecked", "onSelectAllChange"]);
    }

    onNodeChecked(checkedKeys, info) {
        this.checkedKeys = checkedKeys;
    }

    getIndustries() {
        return this.checkedKeys;
    }

    getTree() {
        var s = this.state;
        var nodes = s.treeData;
        var nodeIDs = [];
        function getNodes(nodes, nodeIDs) {
            return nodes.map(function (node, index) {
                nodeIDs.push(node.HY_DM);
                return node.Children && node.Children.length > 0 ?
                    <antd.Tree.TreeNode title={node.HY_DM==='all'? node.HYMC:("(" + node.HY_DM + ") " + node.HYMC)} key={node.HY_DM}>
                        {getNodes(node.Children, nodeIDs)}
                    </antd.Tree.TreeNode> :
                    <antd.Tree.TreeNode title={"(" + node.HY_DM + ") " + node.HYMC} key={node.HY_DM}>
                    </antd.Tree.TreeNode>;
            });
        }
        var cNodes = getNodes(nodes, nodeIDs);
        if (!this.nodeIDs) this.nodeIDs = nodeIDs;
        return <antd.Tree defaultExpandedKeys={['all']} checkable={true} onCheck={this.onNodeChecked }>{cNodes}</antd.Tree>
    }

    componentDidMount() {
        $.post(_bl_ + '/Home/GetIndustryTree', function (rt) {
            if (rt.ErrorMessage) {
                antd.message.error(rt.ErrorMessage);
            } else {
                var treeData = rt.Data;
                this.setState(
                    this._getUpdateStateFun({
                        treeData: [{
                            HY_DM: 'all',
                            HYMC: '全选',
                            Children: treeData
                        }]
                    }));
            }
        }.bind(this), 'json');
    }

    onSelectAllChange(e) {
        this.setState(this._getUpdateStateFun({ checkedAll: e.target.checked }));
    }

    render() {
        var s = this.state;
        var cTree = this.getTree();
        return (
            <div className="industry-tree">
                <div className="industry-tree-header">所属行业</div>
                <div className="industry-tree-body">{cTree}</div>
            </div>
            );
    }
}