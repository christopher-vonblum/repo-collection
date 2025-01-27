var $this = self;

$this.onmessage = function(e) {
    doWork(e.data);
   setInterval(function(){ doWork(e.data) }, 1000)
};

var inProgress=false;

var doWork = function(clientId) {
    if(inProgress){
        return;
    }
    inProgress = true;
    var xhr = new XMLHttpRequest();

    xhr.open('POST', 'https://localhost:5001/ClientEventPipe/PollEvents', true);

    xhr.setRequestHeader("content-type", "application/json");
    xhr.setRequestHeader("clientId", clientId);

    xhr.onload = function () {
        inProgress = false;
        var arr = JSON.parse(xhr.response);
        
        for(i = 0; i < arr.length; i++){
            $this.postMessage(arr[i]);
        }
        
        if(arr.length > 0){
            doWork(clientId);
        }

        xhr.finished = true;
    };

    xhr.onerror = function(){
        inProgress = false;
    };
    
    xhr.send(null);
};