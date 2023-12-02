var express = require('express');
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var shortID = require('shortid');

app.use("/public/TemplateData" , express.static(__dirname + "/public/TemplateData"));
app.use("/public/Build" , express.static(__dirname + "/public/Build"));
app.use(express.static(__dirname+'/public'));

var clients = [];
var clientLookup = {};
var sockets = {};


io.on('connection', function(socket)
{
    console.log('A user ready for connection');

    var currentUser;

    socket.on('PING', function (_pack)
    {
        console.log("Ping")
        var pack = JSON.parse(_pack)

        console.log('message# '+socket.id+" : "+pack.msg);
        socket.emit('PONG' , socket.id, pack.msg);
    });

    socket.on('LOGIN' , function(_data){

    
        console.log('INFO JOIN received');

        var data = JSON.parse(_data);

        currentUser = {
            name:data.name,
            
            position:data.position,
            rotation:'0',
            id:socket.id,
            socketID:socket.id,
            isTrabant:data.isTrabant,
        };
        
        console.log('player logged');
        

        clients.push(currentUser);

        clientLookup[currentUser.id] = currentUser;

        socket.emit("LOGIN_SUCCESS",currentUser.id,currentUser.name,currentUser.position);

        clients.forEach(function(i){
            if(i.id!= currentUser.id)
            {
                socket.emit('SPAWN_PLAYER', i.id, i.name, i.position, i.isTrabant);
            }
        });
        socket.broadcast.emit('SPAWN_PLAYER', currentUser.id,currentUser.name,currentUser.position,currentUser.isTrabant);
    });

    socket.on('MOVE_AND_ROTATE', function(_data){
       
        var data = JSON.parse(_data);
        if(currentUser)
        {

        currentUser.position = data.position;

        currentUser.rotation = data.position;
        socket.broadcast.emit('UPDATE_MOVE_AND_ROTATE', currentUser.id,currentUser.position,currentUser.rotation);

        }
    });
    socket.on('disconnect', function (){

        if(currentUser)
        {
            currentUser.isDead = true;

            socket.broadcast.emit('USER_DISCONNECTED', currentUser.id);
        }

        for(var i = 0; i < clients.length; i++)
        {
            if(clients[i].name == currentUser.name && clients[i].id == currentUser.id)
            {
                console.log("User "+clients[i].name + " has disconnected");
            };
        };
    });
});

app.get('/api/greet', (req, res) => {
    res.send('Hello, World!');
  });


http.listen(process.env.PORT ||3000, function(){
    console.log('listening on *:3000');
});



console.log("running");