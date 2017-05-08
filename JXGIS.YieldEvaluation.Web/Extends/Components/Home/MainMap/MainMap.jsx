class MainMap extends React.Component {
    constructor(props) {
        super(props);

        this.state = {};
    }

    initMap() {

        var baseUrl = "http://144.32.16.71:9001/JXPDServerCore/rest/services/DTMapService/MapServer";
        var annoUrl = "http://144.32.16.71:9001/JXPDServerCore/rest/services/JJMapService/MapServer";

        require([
            "esri/map",
            "esri/layers/ArcGISTiledMapServiceLayer",
            "esri/dijit/Scalebar"
        ],
            function (Map, ArcGISTiledMapServiceLayer, Scalebar) {

                map = new Map("map", {
                    logo: false,
                    sliderPosition: 'top-right',
                    center: [120.75013, 30.76367],
                    zoom: 1
                });

                var scalebar = new Scalebar({
                    map: map,
                    attachTo: "bottom-left",
                    scalebarStyle: "line",
                    scalebarUnit: "metric"
                });

                var vecBase = new ArcGISTiledMapServiceLayer(baseUrl);
                var vecAnno = new ArcGISTiledMapServiceLayer(annoUrl);
                map.addLayers([vecBase, vecAnno]);
            });
    }

    componentDidMount() {
        this.initMap();
    }

    render() {

        return (
        <div id="map">


        </div>);
    }
}