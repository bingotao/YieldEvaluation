var s,ns,r,start;constructor(props);n:bus;return this.eventList={startEndToggle:"startEndToggle",planningTypeSelected:"planningTypeSelected",startPointSetted:"startPointSetted",endPointSetted:"endPointSetted",quickSearchPanelToggled:"quickSearchPanelToggled",quickSearchResultsSetted:"quickSearchResultsSetted",resultPanelToggled:"resultPanelToggled",allCleared:"allCleared",tripModeSelected:"tripModeSelected",routeResultSetted:"routeResultSetted",buslineClick:"buslineClick",busSublineClick:"busSublineClick"},this.tripMode={lessTime:1,lessTransfer:2,lessWalk:4,noSubway:8,lessTime2:0,lessDistance:1,lessHighway:2,onlyWalk:3},this.planning={icons:{start:L.divIcon({className:"rt-icons-start",iconSize:[26,42],iconAnchor:[13,40],popupAnchor:[0,-40]}),end:L.divIcon({className:"rt-icons-end",iconSize:[26,42],iconAnchor:[13,40],popupAnchor:[0,-40]}),busStation:L.divIcon({className:"rt-icons-station",iconSize:[22,21],popupAnchor:[0,-15]})},lineStyle:{lineDefault:{color:"#f26161",weight:4},lineWalk:{color:"#47a2fd",weight:5,dashArray:"5,8"},activeLine:{color:"#1eb739",weight:5}},map:null,start:null,end:null,busLines:[],driveLine:null,stations:[],activeLine:null},this.currentFocus=null,this.state={planningType:this.planningType.drive,tripMode:0,start:{point:null,name:_const_.ept,focus:!1},end:{point:null,name:_const_.ept,focus:!1},quickSearchPanel:{show:!1,results:null},resultPanel:{show:!1,results:null}},this._funsBind(["startEndToggle","quickSearchResultItemClick","getRoutePlanning","buslineClick","setMapStart","setMapEnd","busSublineClick"]),setMap(map),this.planning.map=map,startEndToggle(callback),this.setState((s,p)),{start:s.end,end:s.start};var s,args,r