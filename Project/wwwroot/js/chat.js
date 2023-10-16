"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, timestamp) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user}: ${message} (${timestamp})`;
});

connection.start().then(function () {
    console.log("connection successful!");
    document.getElementById("sendButton").disabled = false;
    connection.invoke("LoadMessage").catch(function (err) {
        return console.error(err.toString());
    })
    console.log("connection invoked?");
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("messageInput").addEventListener("keypress", function(e) {
    if (e.code === "Enter") {
        SendMessage(e);
    }
});

document.getElementById("sendButton").addEventListener("click", function (e) {
    SendMessage(e);
});

function SendMessage(e) {
    console.log("This works");
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    e.preventDefault();
}