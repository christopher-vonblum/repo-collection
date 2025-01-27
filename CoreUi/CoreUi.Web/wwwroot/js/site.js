(function(){
    // emulation method when resizing by remote event
    // this adds a reziseTo method to the rezisable prototype
    // when called it emulates a a resize by triggering the mouse events
    // this is needed to keep all window sizes in sync
    $.widget("ui.resizable", $.ui.resizable, {
        resizeTo: function(newSize) {
            var start = new $.Event("mousedown", { pageX: 0, pageY: 0 });
            this._mouseStart(start);
            this.axis = 'se';
            var end = new $.Event("mouseup", {
                pageX: newSize.Width - this.originalSize.width,
                pageY: newSize.Height - this.originalSize.height,
                IAMADUCK : true
            });
            this._mouseDrag(end);
            this._mouseStop(end);
        }
    });

    // everything starts here...
    $( document ).ready(async function () {
        
        // create a client id
        // this will be used to tell the difference between different tabs/sessions
        clientId = uuidv4();
        
        // keeps polling and deserializing arrays of events on a seperate thread
        poller = new Worker('/js/websocket-event-poller.js');

        poller.onmessage = function (m) {
            // receiving and deserialization is handled by the worker
            // dom manipulation needs to happen on the main thread
            processEvent(m.data);
        };

        // create the proxy so we can do controller request
        serviceProxy = createInteractionServiceProxy("https://localhost:5001/DesktopApi/", clientId);

        // js does not know clr types.
        // this returns a simple array of type names.
        primitiveTypes = serviceProxy.GetPrimitiveTypes();

        var bodySize = serviceProxy.GetMinimumBodySize();
        
        var body = $("body");
        var html = $("html");
        
        if(html.width() > bodySize.width){
            bodySize.width = html.width()
        }

        if(html.height() > bodySize.height){
            bodySize.height = html.height()
        }
        
        $("body").css({ width : bodySize.width, height : bodySize.height});
        
        // get all open dialogs synchronously
        // the server will set a timestamp  for this client id now to ensure that only events that occured after the dialogs were retrieved will be processed.
        var dialogResults = serviceProxy.GetOpenDialogs();

        for (var i = 0; i < dialogResults.length; i++){
            var dialog = dialogResults[i];
            createInputDialog(dialog);
        }
        
        $("body *").focus(e => {
            serviceProxy.FocusSelector("#" + e.target.id);
        });
        
        // start processing events
        poller.postMessage({endpoint:"localhost:5001", clientId});
                
        function showMessageDialog(e)
        {
            var desktop = $(".desktop");
            desktop.append('<div id="'+e.ResponseToken+'"><h3>'+e.DialogTitle+'</h3>'+e.Message+'</div>');
            var dialog = $("#" + e.ResponseToken);

            dialog.dialog(createBaseDialog(e.ResponseToken));
            uploadDialogState(e.ResponseToken);
            dialog.parent().click(() => uploadDialogState(e.ResponseToken));
        }

        function processEvent(e) {
            
            if(e.type === undefined){
                return;
            }
            
            // a generic dialog for a set of properties was requested
            if (e.type.includes("DialogRequestedEvent")) {
                createInputDialog(e.data);
                return;
            }
            
            // another client sent new position and size data for this dialog
            if (e.type.includes("SyncDialogViewEvent")) {
                applyDialogState(e.data);
                return;
            }
            
            // a dialog diplaying a message was requested
            if(e.type.includes("ShowMessageDialogEvent")){
                showMessageDialog(e.data);
                return;
            }

            // a dialog should be shoould be closed now because another client interacted
            if(e.type.includes("DialogDestroyedEvent")){
                $("#" + e.data.ResponseToken).remove();
                return;
            }

            // another dialog was focused by the user in a different client
            if(e.type.includes("FocusSelectorEvent")){
                $(e.data).focus();
                return;
            }

            // the server requested to render all active dialogs again
            // also happens on restart... (just to make sure)
            if(e.type.includes("ReloadClientEvent")){
                var allDialogs = $(".dialog");
                allDialogs.remove();
                return;
            }
        }

        // Let the user select a activity to run
        $(".runActionButton").click( b =>
        {
            serviceProxy.InvokeActivityInput({"type":"Toolbox.Activities.RunActivityActivity, Toolbox"});
        })
    });
    
    // creates a simple dialog realted to a response token
    // all buttons report to the service proxy
    function createBaseDialog(responseToken){
        return {
            buttons : {
                Ok: function() {
                    serviceProxy.ConfirmResponse({
                            ResponseToken : responseToken
                        },
                        true);
                }
            },
            close : (e, d)=> { 
                serviceProxy.CancelResponse(e.target.id, true); 
                $(this).remove();
            },
            dragStop: (e, d) =>
            {
                uploadDialogState(e.target.id)
            },
            resizeStop: (e, d) =>
            {
                if(e.IAMADUCK)return;
                uploadDialogState(e.target.id)
            }
        };
    }
    
    //
    function createInputDialog(e) {
        var desktop = $(".desktop");
        desktop.append('<div id="'+e.ResponseToken+'" class="dialog"><h3>'+e.DialogTitle+'</h3></div>');
        var dialog =$("#" + e.ResponseToken);
        renderFields(e.ResponseToken, dialog, e.Properties);
        
        var baseDialog = createBaseDialog(e.ResponseToken);
        
        Object.assign(baseDialog, {
            buttons : {
                Ok: function(ev) {
                    var res = readFieldValues(e.ResponseToken, dialog, e.Properties);
                    serviceProxy.ConfirmResponse(
                        {
                            ResponseToken : e.ResponseToken,
                            Data : res
                        }, true);
                    dialog.remove();
                },
                Cancel: function() {
                    serviceProxy.CancelResponse($(this).attr("id"), true);
                    dialog.remove();
                }
            }});
        
        dialog.dialog(baseDialog);
        uploadDialogState( e.ResponseToken);
        dialog.parent().click(() => uploadDialogState( e.ResponseToken));
    }

    function uploadDialogState(id) {

        var dialog = $("#" + id);
        
        var model = {
            DialogId : id,
            X : parseInt(dialog.dialog("widget").css("left"),10),
            Y : parseInt(dialog.dialog("widget").css("top"),10),
            Z : dialog.dialog("widget").css("z-index"),
            Width : dialog.dialog("widget").width(),
            Height : dialog.dialog("widget").height()
        };
        
        serviceProxy.SyncDialog(model, true);
    }
    
    function applyDialogState(data){
        var dialogModel = data.Dialog;
        
        var dialog = $("#" + dialogModel.DialogId);

        dialog.dialog("widget").css({
            'top': dialogModel.Y, 
            'left' : dialogModel.X,
            'z-index' : dialogModel.Z
        });
        
        dialog.dialog("widget").data("uiResizable").resizeTo(dialogModel);
    }
    
    function renderFields(parentId, parentEl, props){
        for(var i = 0; i < props.length; i++){
            var prop = props[i];
            
            if(primitiveTypes.includes(prop.ClrType)){
                parentEl.append('<span>'+prop.Name+'<span>');
                parentEl.append('<input type="text" id="i'+parentId+'-'+prop.Name+'"><br>');
            }
        }
    }

    function readFieldValues(parentId, parentEl, props) {
        var model = {};
        
        for(var i = 0; i < props.length; i++){
            var prop = props[i];
            
            var propId = '#i' + parentId + '-' + prop.Name;
            
            if(primitiveTypes.includes(prop.ClrType)){
                model[prop.Name] = $(propId).val();
            }
        }
        
        return model;
    }
    
    function uuidv4() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

})();