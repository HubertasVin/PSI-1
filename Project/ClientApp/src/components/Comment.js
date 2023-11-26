import { useState, useEffect, useCallback } from "react";
import { useUserContext } from "../userContext";
import {
  HttpTransportType,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";

export const Comment = ({ show, onClose, topicId }) => {
  const { userEmail } = useUserContext();
  const [comments, setComments] = useState([]);
  const [currentComment, setCurrentComment] = useState("");
  const [connection, setConnection] = useState(null);
  const [prevComments, setPrevComments] = useState([]);

  const [isConnecting, setIsConnecting] = useState(false);

  const initConnection = useCallback(async () => {
    setIsConnecting(true);
    console.log("Creating connection");
    const newConnection = new HubConnectionBuilder()
      .withUrl("https://localhost:7015/chatHub", {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    newConnection.on(
      "ReceiveMessage",
      (messageId, topicId, senderId, messageContent) => {
        console.log(
          "Received: topicId: " +
            topicId +
            " senderId: " +
            senderId +
            " messageContent: " +
            messageContent +
            " messageId: " +
            messageId
        );
        setComments((prevComments) => {
          console.warn("Doing magic with comments");
          console.warn("Received messageContent: " + messageContent);
          return [
            ...prevComments,
            {
              id: messageId,
              userId: senderId,
              text: messageContent,
              isReal: true,
            },
          ];
        });
        setPrevComments(comments);
        // return prevComments.map(comment => {
        // console.warn("Doing magic with comments")
        // console.warn("Received messageContent: " + messageContent)
        // return { ...comment, id: comment.id, isReal: true };
        // return comment;
        // });

        console.warn(comments);
      }
    );

    newConnection.on("DeleteMessage", (messageId) => {
      setComments((prevComments) => {
        return prevComments.filter((comment) => comment.id !== messageId);
      });
    });

    // newConnection.stop()
    console.log("Starting connection");
    newConnection
      .start()
      .then(() => {
        console.log("Connection started");
        console.log(topicId);
        newConnection.invoke("JoinTopic", topicId);
        setConnection(newConnection);
      })
      .catch((err) => console.error("Connection failed", err.toString()));
    setIsConnecting(false);
  }, []);

  useEffect(() => {
    if (
      show &&
      !isConnecting &&
      (!connection || connection.state !== "Connected")
    ) {
      initConnection().catch((err) =>
        console.error("Connection failed", err.toString())
      );
    }

    return () => {
      if (connection) {
        connection
          .stop()
          .then(() => console.log("Connection has been stopped"))
          .catch((err) =>
            console.error("Unable to stop connection", err.toString())
          );
      }
    };
  }, [show, connection, initConnection, isConnecting, topicId]);

  useEffect(() => {
    //fetching comments
    // console.log("Current user email: " + userEmail);
    console.log("Fetching comments");
    fetch("https://localhost:7015/comment/get/" + topicId)
      .then((response) => response.json())
      .then((data) => {
        console.log("Comments: ", data);
        const commentData = data.map((comment) => {
          return {
            id: comment.id,
            userId: comment.userId,
            text: comment.message,
            isReal: true,
          };
        });
        setComments(commentData);
        setPrevComments(commentData);
      });
  }, []);

  const handleSend = async () => {
    console.log("Sending message");
    if (connection) {
      console.log("Connection exists");
      console.log("Getting user info with email " + userEmail);

      const response = await fetch(
        "https://localhost:7015/user/get-by-email/" + userEmail
      );
      if (response.ok) {
        const data = await response.json();
        const fetchedUserId = data.id;
        console.log("User id: " + fetchedUserId);

        const comment = {
          userId: fetchedUserId,
          topicId: topicId,
          message: currentComment,
        };

        try {
          console.error("Sending message: " + currentComment);
          connection.invoke(
            "SendMessage",
            topicId,
            fetchedUserId,
            currentComment
          );
          // await handleSaveComment(comment, fetchedUserId);
          setCurrentComment("");
        } catch (err) {
          console.error("Unable to send message", err.toString());
          // Handle the error appropriately
        }
      } else {
        console.error("Failed to fetch user data");
        // Handle the error appropriately
      }
    }
  };

  // const handleSaveComment = async (comment, fetchedUserId) => {
  //     try {
  //         const requestBody = {
  //             message: comment.message,
  //             userId: comment.userId,
  //             topicId: comment.topicId
  //         };
  //         console.warn("Saving comment: " + comment.userId + " " + comment.topicId + " " + comment.message)
  //         const response = await fetch('https://localhost:7015/comment/add', {
  //             method: 'POST',
  //             headers: {
  //                 'Content-Type': 'application/json',
  //             },
  //             body: JSON.stringify(requestBody),
  //         });
  //         console.error("Response: " + response);
  //         if (!response.ok) {
  //             throw new Error('Network response was not ok');
  //         }
  //         console.log("Is working?");
  //         const data = await response.text()
  //             .then(async data => {
  //                 console.error("Responseeeeeee: " + data.id);
  //                 await connection.invoke("SendMessage", data.id, topicId, fetchedUserId, currentComment);
  //             })
  //         // console.log("Responseeeeeee: " + data.id);
  //             // .then (async data => {
  //             //     await connection.invoke("SendMessage", data.id, topicId, fetchedUserId, currentComment);
  //             // });
  //
  //
  //     } catch (error) {
  //         console.error('Failed to save the comment:', error);
  //     }
  // };

  const handleDelete = async (commentId) => {
    try {
      connection.invoke("DeleteMessage", commentId, topicId);
      const response = await fetch(
        `https://localhost:7015/comment/delete/${commentId}`,
        {
          method: "DELETE",
        }
      );
      if (response.ok) {
        if (connection) {
          await connection.invoke("DeleteMessage", topicId, commentId);
        }
      } else {
        throw new Error("Failed to delete the comment");
      }
    } catch (error) {
      console.error("Error deleting comment:", error);
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
        {comments?.map((comment, index) => (
          <div key={index} className="comment">
            <div className="comment-text-content">{comment.text}</div>
            <button onClick={() => handleDelete(comment.id)}>Delete</button>
          </div>
        ))}
      </div>
      <div className="comment-footer">
        <input
          value={currentComment}
          onChange={(e) => setCurrentComment(e.target.value)}
          placeholder="Enter your comment..."
        />
        <button onClick={handleSend}>Send</button>
      </div>
    </div>
  );
};
