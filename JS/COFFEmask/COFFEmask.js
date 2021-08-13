const ws = new WebSocket('ws://127.0.0.1:28000');
ws.addEventListener('open', function (event) {
	$('#result_socket').html('WSS connected')
	var data = { req:true, action:'connected' };
    ws.send(JSON.stringify(data), { binary: false });
});

let session = make_session();
function make_session(){
   var result           = '';
   var characters       = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789!?@%';
   var charactersLength = characters.length;
   for ( var i = 0; i < 24; i++){
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
   }
   return result;
}

function auth(desktop){
    console.log('PAGE send auth');
    var data = { req:true, session:$('#session').val(), action:'auth' };
	if(desktop){
		ws.send(JSON.stringify(data), { binary: false });
	}else{
		window.postMessage(data, '*');
	}
};

function transaction(desktop){
    console.log('PAGE send transaction');
    var data = { req:true, session:$('#session').val(), action:'transaction', data:JSON.parse($('#transaction').val()) };
	if(desktop){
		ws.send(JSON.stringify(data), { binary: false });
	}else{
		window.postMessage(data, '*');
	}
};

function transaction_smart(desktop){
    console.log('PAGE send transaction_smart');
    var data = { req:true, session:$('#session').val(), action:'transaction', data:JSON.parse($('#transaction_smart').val()) };
	if(desktop){
		ws.send(JSON.stringify(data), { binary: false });
	}else{
		window.postMessage(data, '*');
	}
};

window.addEventListener('message', readMessage);
ws.addEventListener('message', readMessage);
function readMessage(event){
	console.log('readMessage', event.data)
	event = event.data
	try{
		if(typeof event == 'string'){
			event = JSON.parse(event)
		}
	}catch(e){
		console.log('readMessage', e)
	}
	if(event.session == $('#session').val() && event.res){
		console.log('PAGE receive message:', event);
		$('#result').html(JSON.stringify(event))
	}
}