import { useState, useEffect, useCallback } from "react";
import { useUserContext } from "../userContext";
import {
  HttpTransportType,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";

export const Comment = ({ show, onClose, topicId }) => {
  const { userEmail, setUserEmail } = useUserContext();
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
    console.log("Setting userEmail if not set");
    if (!userEmail) {
      console.log("Setting userEmail");
      setUserEmail(localStorage.getItem("userEmail"));
      // console.log("User email: " + userEmail);
      console.log("User email set");
    } else {
      console.log("User email already set");
    }

    console.log("Fetching comments");
    fetch("https://localhost:7015/comment/get/" + topicId)
      .then((response) => response.json())
      .then((data) => {
        console.log("Comments: ", data);
        const commentPromises = data.map((comment) => {
          return fetch("https://localhost:7015/user/get/" + comment.userId)
            .then((response) => response.json())
            .then((user) => {
              console.log("User: ", user);
              return {
                id: comment.id,
                userId: comment.userId,
                name: user.name,
                surname: user.surname,
                text: comment.message,
                email: user.email,
                isReal: true,
              };
            });
        });
        Promise.all(commentPromises).then((commentData) => {
          setComments(commentData);
          setPrevComments(commentData);
        });
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
        <h3>Comments</h3>
        <button onClick={onClose}>Close</button>
      </div>
      <div className="comment-content">
        <ul className="comment-list">
          {comments?.map((comment, index) => (
            <li key={index} className="comment">
              <a className="comment-author">
                {comment.userId === localStorage.getItem("loginToken")
                  ? "You - "
                  : comment.name + " - "}
              </a>
              <div className="comment-text-content">{comment.text}</div>
              {comment.userId === localStorage.getItem("loginToken") && (
                <button onClick={() => handleDelete(comment.id)}>Delete</button>
              )}
            </li>
          ))}
        </ul>
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
