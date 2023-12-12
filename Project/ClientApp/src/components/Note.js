import React, { useState, useEffect, useRef } from "react";
// import { Document, Page } from 'react-pdf';
import { useParams } from "react-router-dom";
import DocViewer, {
  DocViewerRenderers,
  PDFRenderer,
} from "@cyntler/react-doc-viewer";
import Markdown from "react-markdown";
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
  const [filePath, setFilePath] = useState(null);
  const [fileContent, setFileContent] = useState("");
  const [fileName, setFileName] = useState("");
  const [fileExtension, setFileExtension] = useState("");
  const [index, setIndex] = useState(0);

  const extraFileTypes = ["md", "txt"];
  const documentFileTypes = [
    "pdf",
    "docx",
    "doc",
    "odt",
    "ppt",
    "pptx",
    "xls",
    "xlsx",
  ];
  const imageFileTypes = ["png", "jpg", "jpeg"];
  const unsupportedFileTypes = [
    "docx",
    "doc",
    "odt",
    "ppt",
    "pptx",
    "xls",
    "xlsx",
  ];

  let newWindow = null;

  const onFileChange = (event) => {
    setFileInput(event.target.files[0]);
  };
  const onFileUpload = async () => {
    if (!fileInput) {
      alert("Please select a file to upload");
      return;
    }

    const formData = new FormData();
    formData.append("title", fileInput.name.replace(/\.[^/.]+$/, ""));
    formData.append("authorId", localStorage.getItem("loginToken"));
    formData.append("subjectId", subjectId);
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

  async function fetchConspectPath(topicId, index) {
    await fetch(
      `https://localhost:7015/conspect/get-conspect-file-path/${topicId}/${index}`
    )
      .then((response) => response.text())
      .then((data) => {
        setFilePath(data);
        setFileName(data.split("/")[data.split("/").length - 1]);
        setFileExtension(getExtension(data));
        return data;
      })
      .catch((error) => {
        console.error("Error fetching conspect path:", error);
      });
  }

  function deleteConspect(topicId, index, authorId) {
    fetch(
      `https://localhost:7015/conspect/delete/${topicId}/${index}/${authorId}`,
      {
        method: "DELETE",
      }
    )
      .then((response) => {
        if (!response.ok) {
          throw new Error(response.statusText);
        }
        setSeed(seed + 1);
        window.location.reload();
      })
      .catch((error) => {
        alert(error);
      });
  }

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Fetch topic name
        const topicResponse = await fetch(
          "https://localhost:7015/topic/get/" + topicId
        );
        const topicData = await topicResponse.json();
        setTopicName(topicData.name);

        // Fetch conspects
        const conspectsResponse = await fetch(
          "https://localhost:7015/conspect/get-conspects-list-by-id/" + topicId
        );
        if (conspectsResponse.ok) {
          const conspectsData = await conspectsResponse.json();
          setConspects(conspectsData);
        } else {
          console.error(
            "Failed to fetch conspects:",
            conspectsResponse.status,
            conspectsResponse.statusText
          );
        }

        // Fetch content when filePath is not null
        if (extraFileTypes.includes(getExtension(filePath))) {
          const content = await fetchFileContent(filePath);
          setFileContent(content);
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, [filePath]);

  const OpenedComments = () => {
    setShowComments(true);
    console.log("Set show comments to true");
  };

  const ClosedComments = () => {
    setShowComments(false);
    console.log("Set show comments set to" + showComments);
  };

  function getExtension(path) {
    if (!path) {
      return null;
    }

    const pathArray = path.split("/");
    const nameArray = pathArray[pathArray.length - 1].split(".");
    return nameArray[nameArray.length - 1];
  }

  const onOpenButtonClick = async (conspect) => {
    try {
      setIndex(conspect.index);
      await fetchConspectPath(topicId, conspect.index);
      await fetchFileContent(filePath);
      setSeed(seed + 1);
    } catch (error) {
      console.error("Error opening conspect:", error);
    }
  };

  const fetchFileContent = async (path) => {
    const response = await fetch("https://localhost:44465/" + path);
    const text = await response.text();
    return text;
  };

  return (
    <div className="default-page-background">
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
            key={uploadStatus}
          >
            <input
              id="file-input"
              type="file"
              onChange={onFileChange}
              accept=".pdf,.docx,.doc,.odt,.ppt,.pptx,.xls,.xlsx,.png,.jpg,.jpeg,.txt,.md"
            />
            <p className="file-type-text">
              Supported file types:{" "}
              <i>
                .pdf, .docx, .doc, .odt, .ppt, .pptx, .xls, .xlsx, .png, .jpg,
                .jpeg, .txt, .md
              </i>
            </p>
            <p style={{ color: uploadSuccess ? "green" : "red" }}>
              {uploadStatus}
            </p>
          </div>
          <h3>
            Available conspects:
          </h3>
          <div className="conspect-list-panel">
            <ul key={seed}>
              {conspects.map((conspect) => (
                <li key={`${conspect.topicId}-${conspect.index}`}>
                  <a>{conspect.fileName}</a>
                  <div className="conspect-buttons">
                    <button
                      className="open-conspect-button"
                      onClick={async () => await onOpenButtonClick(conspect)}
                    >
                      {!unsupportedFileTypes.includes(getExtension(conspect.fileName)) ? "Open" : "Download"}
                    </button>
                    {localStorage.getItem("loginToken") === conspect.authorId && (
                      <button
                        className="delete-conspect-button"
                        onClick={() => {
                          deleteConspect(conspect.topicId, conspect.index, conspect.authorId);
                        }}
                      >
                        Delete
                      </button>
                    )}
                  </div>
                </li>
              ))}
            </ul>
          </div>
          <Comment show={showComments} onClose={ClosedComments} topicId={topicId} className="comment-panel" />
        </div>
        {filePath != null ? (
          <div className="document-viewer" key={filePath}>
            <h2 className="conspect-name">Conspect name: {fileName}</h2>
            {fileExtension === "md" ? (
              <Markdown className="doc-viewer">{fileContent}</Markdown>
            ) : fileExtension === "txt" ? (
              <p className="doc-viewer">{fileContent}</p>
            ) : documentFileTypes.includes(fileExtension) ? (
              <iframe
                className="doc-viewer"
                src={"https://localhost:44465/" + filePath}
              ></iframe>
            ) : imageFileTypes.includes(fileExtension) ? (
              <img
                className="doc-viewer"
                src={"https://localhost:44465/" + filePath}
                style={{
                  maxWidth: "100%",
                  maxHeight: "100%",
                  objectFit: "contain",
                }}
              ></img>
            ) : (
              <DocViewer
                className="doc-viewer"
                documents={[{ uri: "https://localhost:44465/" + filePath }]}
              />
            )}
          </div>
        ) : null}
      </div>
    </div>
  );
};
