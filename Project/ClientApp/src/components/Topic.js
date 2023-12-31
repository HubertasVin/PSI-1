﻿import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "./Topic.css";
import "./common.css";

export const Topic = () => {
  const { id } = useParams();
  const [subjectName, setSubjectName] = useState("");
  const [topics, setTopics] = useState([]);
  const [topicNames, setTopicNames] = useState([]);
  const [newTopic, setNewTopic] = useState("");
  const [refreshTopics, setRefreshTopics] = useState(false);
  const [showDialog, setShowDialog] = useState(false);
  const [keepDialog, setKeepDialog] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    fetch("https://localhost:7015/subject/get/" + id)
      .then((response) => response.json())
      .then((data) => setSubjectName(data.name))
      .catch((error) => console.error("Error fetching subject name: ", error));
    // console.log("Fetching subject name");
    // const fetchData = async () => {
    //     const response = await fetch('https://localhost:7015/subject/get/${id}');
    //     console.log("Response called");
    //     if (response.ok) {
    //         console.log("Response is ok");
    //         const data = await response.json();
    //         console.log("Data from server: ", data);
    //         setSubjectName(data.name);
    //     } else {
    //         console.error("Something went wrong fetching data from server");
    //     }
    // }
  }, []);

  useEffect(() => {
    const fetchTopics = async () => {
      console.log("Fetching topics");
      const response = await fetch("https://localhost:7015/topic/list/" + id);
      console.log("Response called");
      if (response.ok) {
        console.log("Response is ok");
        const data = await response.json();
        console.log("Data from server: ", data);

        const topicData = data.map((topic) => {
          return {
            id: topic.id,
            name: topic.name,
          };
        });

        setTopics(topicData);

        console.log("Topics: ", topics);
        const names = topicData.map((topic) => topic.name);
        console.log("Topic names: ", names);

        setTopicNames(names);
      } else {
        console.error("Something went wrong fetching data from server");
      }
    };

    fetchTopics();
  }, [refreshTopics, id]);

  const handleAddTopic = async () => {
    if (newTopic) {
      const requestBody = {
        topicName: newTopic,
        subjectId: id,
      };
      console.log(requestBody);
      const response = await fetch("https://localhost:7015/topic/upload", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(requestBody),
      });
      if (response.ok) {
        const data = await response.json();

        topics.push({
          id: data.id,
          name: data.name,
        });
        setTopics(topics);
        setNewTopic("");
        setShowDialog(false);
        setRefreshTopics(!refreshTopics);
      } else {
        console.error("Something went wrong fetching data from server");
      }
    }
  };

  const toggleDialog = () => {
    if (localStorage.getItem("loginToken") === null) {
      alert("You must be logged in to add a topic");
      return;
    }
    setShowDialog(!showDialog);
    setKeepDialog(true);
  };

  const goBack = () => {
    navigate(-1);
  };

  return (
    <div className="default-page-background">
      <div className="subjects-top-container">
        <h1>Topics in subject: {subjectName}</h1>
        <div className="button-container">
          <div className="add-subject" onClick={goBack}>
            <span className="plus-icon">Back</span>
          </div>
          <div className="add-subject" onClick={toggleDialog}>
            <span className="plus-icon">+</span>
          </div>
        </div>
      </div>
      <div className="subjects-container">
        <div className="subjects-grid">
          {topics.map((topic, index) => (
            <div
              key={index}
              className="subject-grid-item"
              title={topic.name}
              onClick={() => navigate("/Subjects/" + id + "/" + topic.id)}
            >
              <h2>{topicNames[index]}</h2>
            </div>
          ))}
        </div>
        {keepDialog && (
          <div
            className={`dialog ${showDialog ? "" : "closing"}`}
            style={{
              animation: showDialog
                ? "slideInCenter 0.2s forwards"
                : "slideOutCenter 0.2s forwards",
            }}
          >
            <input
              type="text"
              placeholder="Topic Name"
              value={newTopic}
              onChange={(e) => setNewTopic(e.target.value)}
            />
            <div className="dialog-buttons">
              <button onClick={handleAddTopic}>Create</button>
              <button onClick={() => setShowDialog(false)}>Cancel</button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
