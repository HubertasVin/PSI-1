﻿import { useState, useEffect } from 'react';
import {useUserContext} from "../userContext";
import {HttpTransportType, HubConnectionBuilder, LogLevel} from "@microsoft/signalr";

export const Comment = ({show, onClose, topicId}) => {
    const {userID} = useUserContext();
    const [comments, setComments] = useState([]);
    const [currentComment, setCurrentComment] = useState('');
    const [connection, setConnection] = useState(null);
    const [prevComments, setPrevComments] = useState([]);

    useEffect(() => {
        console.log("Initializing connection");
        console.log("Show state: " + show)
        const initConnection = async () => {
            console.log("Creating connection")
            const newConnection = new HubConnectionBuilder()
                .withUrl("https://localhost:7015/chatHub", {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets
                })
                .configureLogging(LogLevel.Information)
                .withAutomaticReconnect()
                .build();

            newConnection.on("ReceiveMessage", (topicId, senderId, messageContent) => {
                setComments(prevComments => {
                    return prevComments.map(comment => {
                        console.warn("Doing magic with comments")
                        console.warn("Received messageContent: " + messageContent)
                        return { ...comment, id: comment.id, isReal: true };
                        // return comment;
                    });
                });
                console.warn(comments)
            });

            newConnection.on("DeleteMessage", (messageId) => {
                setComments(prevComments => {
                    return prevComments.filter(comment => comment.id !== messageId);
                });
            });

            console.log("Starting connection")
            newConnection.start()
                .then(() => {
                    console.log("Connection started");
                    console.log(topicId);
                    newConnection.invoke("JoinTopic", topicId);
                    setConnection(newConnection);
                })
                .catch(err => console.error("Connection failed", err.toString()));

            // TODO need to fetch all comments for the topic
        }

        console.log("Show state near if: " + show)
        console.log("Connection: " + connection)
        if (show && (!connection || connection.state !== "Connected")) {
            initConnection()
                .then(newConnection => {
                    setConnection(newConnection);
                })
                .catch(err => console.error("Connection failed", err.toString()));
        }
        console.log("Connection: " + connection)

        return() => {
            if (connection) {
                connection.stop()
                    .then(() => {
                        console.log("Connection has been stopped");
                        setConnection(null);
                    })
                    .catch(err => console.error("Unable to stop connection", err.toString()));
            }
        };

    }, [show, connection, topicId]);
    
    const handleSend = async () => {
        console.log("Sending message")
        if (connection) {
            console.log("Connection exists")
            const commentId = Date.now();
            const comment = {
                id: commentId,
                userId: userID,
                text: currentComment,
                isReal: false
            }

            setComments(prevComments => {
                return [...prevComments, comment];
            });

            try {
                console.error("Sending message:  " + currentComment)
                await connection.invoke("SendMessage", topicId, userID, currentComment);
                setCurrentComment('');
            } catch (err) {
                console.error("Unable to send message", err.toString());
                setComments(prevComments => prevComments.filter(comment => comment.id !== commentId));
            }
        }
    };

    if (!show) {
        return null;
    }
    
    return (
        <div className="comment-panel">
            <div className="comment-header">
                <h2>Comments</h2>
                <button onClick={onClose}>Close</button>
            </div>
            <div className="comment-body">
                {comments.map((comment, index) => (
                    <div key={index} className="comment">
                        <div className="comment-text-content">
                            {comment.text}
                        </div>
                    </div>
                ))}
            </div>
            <div className="comment-footer">
                <input
                    value = {currentComment}
                    onChange = {(e) => setCurrentComment(e.target.value)}
                    placeholder="Enter your comment..."
                />
                <button onClick={handleSend}>Send</button>
            </div>
        </div>
    );
}
    