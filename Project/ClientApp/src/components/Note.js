import React, { useState, useEffect } from "react";
// import { Document, Page } from 'react-pdf';
import { useParams } from "react-router-dom";
import { Comment } from "./Comment";
 
export const Note = () => {
  const { topicId } = useParams();
  const [showComments, setShowComments] = useState(false);
  const [topicName, setTopicName] = useState("");
  const [fileInput, setFileInput] = useState(null);
  const [uploadStatus, setUploadStatus] = useState(null);
  const [uploadSuccess, setUploadSuccess] = useState(null);
 
  const onFileChange = (event) => {
    setFileInput(event.target.files[0]);
  };
  console.log(topicId);
  const onFileUpload = async () => {
    if (!fileInput) {
      alert("Please select a file to upload");
      return;
    }
    console.log(fileInput.name.replace(/\.[^/.]+$/, ""));
 
    const formData = new FormData();
    formData.append("title", fileInput.name.replace(/\.[^/.]+$/, ""));
    formData.append("authorId", localStorage.getItem("loginToken"));
    formData.append("topicId", topicId);
    formData.append("fileName", fileInput.name);
    formData.append("file", fileInput);
 
    try {
      setUploadStatus("Uploading...");
      const response = await fetch("https://localhost:7015/conspect/upload", {
        method: "POST",
        body: formData, // Use FormData instead of JSON.stringify
      });
 
      if (response.ok) {
        setUploadStatus("Upload successful");
        setUploadSuccess(true);
      } else {
        const errorMessage = await response.text();
        setUploadStatus(`Upload failed: ${errorMessage}.`);
        setUploadSuccess(false);
      }
    } catch (e) {
      console.error(e);
      setUploadStatus(`Upload failed ${e}.`);
    }
  };
 
  useEffect(() => {
    try {
      fetch("https://localhost:7015/topic/get/" + topicId)
        .then((response) => response.json())
        .then((data) => {
          setTopicName(data.name);
        });
    } catch (e) {
      console.log(e);
    }
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
        <p style={{ color: uploadSuccess ? "green" : "red" }}>
          {uploadStatus}
        </p>
      </div>
      <div className="comment-panel">
        <button onClick={() => setShowComments(true)}>Comments</button>
      </div>
      <Comment show={showComments} onClose={ClosedComments} topicId={topicId} />
    </div>
  );
};