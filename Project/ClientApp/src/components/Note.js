import React, { useState, useEffect, useRef } from "react";
// import { Document, Page } from 'react-pdf';
import { useParams } from "react-router-dom";
import { Comment } from "./Comment";
import "./Note.css";
import "./common.css";

export const Note = () => {
  const { subjectId, topicId } = useParams();
  const [showComments, setShowComments] = useState(false);
  const [topicName, setTopicName] = useState("");
  const [fileInput, setFileInput] = useState(null);
  const [uploadStatus, setUploadStatus] = useState(null);
  const [uploadSuccess, setUploadSuccess] = useState(null);
  const [conspects, setConspects] = useState([]);
  const [seed, setSeed] = useState(0); // Used to force re-rendering of the conspect's list
  const fileRef = useRef(null);

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
    formData.append("subjectId", subjectId)
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
      setSeed(seed + 1);
    } catch (e) {
      console.error(e);
      setUploadStatus(`Upload failed ${e}.`);
    }
  };

  const fetchConspects = async () => {
    try {
      const response = await fetch(
        "https://localhost:7015/conspect/get-conspects-list-by-id/" + topicId
      );
      if (response.ok) {
        const data = await response.json();
        setConspects(data);
      } else {
        console.error(
          "Failed to fetch conspects:",
          response.status,
          response.statusText
        );
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  function fetchConspect(topicId, index) {
    console.log(index);
    fetch(
      `https://localhost:7015/conspect/get-conspect-file/${topicId}/${index}`
    )
      .then((response) => response.blob())
      .then((blob) => {
        const url = URL.createObjectURL(blob);
        if (fileRef.current) {
          fileRef.current.src = url;
        }
      });
  }

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

    fetchConspects();
  }, [seed]);

  const OpenedComments = () => {
    setShowComments(true);
    console.log("Set show comments to true");
  };

  const ClosedComments = () => {
    setShowComments(false);
    console.log("Set show comments set to" + showComments);
  };

  return (
    <div className="gradient-background">
      <div className="header">
        <h1>Topic: {topicName}</h1>
      </div>
      <div className="main-content">
        <div className="conspect-panel">
          <div className="button-panel">
            <button onClick={() => setShowComments(true)}>Comments</button>
            <button onClick={onFileUpload}>Upload</button>
          </div>
          <div
            className="upload-conspect-panel"
            style={{ display: "inline-block", width: "100%" }}
          >
            <input
              id="file-input"
              type="file"
              onChange={onFileChange}
              accept="application/pdf"
            />
            <p className="file-type-text">
              Supported file types: <i>.pdf</i>
            </p>
            <p style={{ color: uploadSuccess ? "green" : "red" }}>
              {uploadStatus}
            </p>
          </div>
          <div className="conspect-list-panel">
            <p>
              Available conspects:
              <ul key={seed}>
                {conspects.map((conspect) => (
                  <li key={conspect.topicId}>
                    <a>{conspect.title}</a>
                    <button
                      className="open-pdf-button"
                      onClick={() =>
                        fetchConspect(conspect.topicId, conspect.index)
                      }
                    >
                      Open
                    </button>
                    {localStorage.getItem("loginToken") ===
                      conspect.authorId && (
                      <button
                        className="delete-conspect-button"
                        onClick={() => {
                          fetch(
                            `https://localhost:7015/conspect/delete/${conspect.topicId}/${conspect.index}/${conspect.authorId}`,
                            {
                              method: "DELETE",
                            }
                          )
                            .then((response) => {
                              if (!response.ok) {
                                throw new Error(response.statusText);
                              }
                              setSeed(seed + 1);
                              return response;
                            })
                            .catch((error) => {
                              alert(error);
                            });
                          setSeed(seed + 1);
                        }}
                      >
                        Delete
                      </button>
                    )}
                  </li>
                ))}
              </ul>
            </p>
          </div>
        </div>
        <div className="pdf-viewer">
          <iframe ref={fileRef} type="application/pdf" />
        </div>
      </div>
      <Comment show={showComments} onClose={ClosedComments} topicId={topicId} />
    </div>
  );
};
