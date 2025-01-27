var $this = self;
var ws;

$this.onmessage = function(e) {
    ws = new WebSocket('wss://'+e.data.endpoint+'/ws', e.data.clientId);
    ws.onopen = function() {
        ws.send("test:1234");
    };
    ws.onerror = function() {
    };
    ws.onclose = function() {
    };
    ws.onmessage = function(msgevent) {
        var arr = JSON.parse(msgevent.data);

        for(i = 0; i < arr.length; i++){
            $this.postMessage(arr[i]);
        }
    };
};