﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user}: ${message}`;
});

// connection.on("LoadMessages", function (user, message) {
//     var li = document.createElement("li");
//     document.getElementById.appendChild(li);
//     li.textContent = `${user}: ${message}`;
// })

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

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});