import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { Comment } from "./Comment";

export const Note = () => {
  const { topicId } = useParams();
  const [showComments, setShowComments] = useState(false);
  const [topicName, setTopicName] = useState("");
  const [fileInput, setFileInput] = useState(null);
  const [uploadStatus, setUploadStatus] = useState(null);

  const onFileChange = (event) => {
    setFileInput(event.target.files[0]);
  };
  console.log(topicId);
  const onFileUpload = () => {
    if (!fileInput) {
      alert("Please select a file to upload");
      return;
    }

    const requrestBody = {
      topicId: topicId,
      file: fileInput,
    };

    setUploadStatus("Uploading...");
    const response = fetch("https://localhost:7015/conspect/upload", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(requrestBody),
    })
    .then((response) => {
      if (response.ok) {
        setUploadStatus("Upload successful");
      } else {
        setUploadStatus("Upload failed");
      }
    });
  };

  useEffect(() => {
    // console.log(showComments)
    fetch("https://localhost:7015/topic/get/" + topicId)
      .then((response) => response.json())
      .then((data) => {
        setTopicName(data.name);
      });
  }, []);

  const OpenedComments = () => {
    setShowComments(true);
    console.log("Set show comments to true");
  };

  const ClosedComments = () => {
    setShowComments(false);
    console.log("Set show comments set to" + showComments);
  };

  return (
    <div>
      <h1>{topicName}</h1>
      <div
        className="upload-conspect-panel"
        style={{ display: "inline-block" }}
      >
        <input
          id="file-input"
          type="file"
          onChange={onFileChange}
          accept="application/pdf"
        />
        <button onClick={onFileUpload}>Upload</button>
        <p>
          Supported file types: <i>.pdf</i>
        </p>
        <p>{uploadStatus}</p>
      </div>
      <div className="comment-panel">
        <button onClick={() => setShowComments(true)}>Comments</button>
      </div>
      <Comment show={showComments} onClose={ClosedComments} topicId={topicId} />
    </div>
  );
};
