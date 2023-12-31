import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./Subject.css";
import "./common.css";

export const Subject = () => {
  const [subjects, setSubjects] = useState([]);
  const [subjectNames, setSubjectNames] = useState([]);
  const [refreshSubjects, setRefreshSubjects] = useState(false);
  const [newSubject, setNewSubject] = useState("");
  const [showDialog, setShowDialog] = useState(false);
  const [keepDialog, setKeepDialog] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    console.log("Fetching subjects");
    const fetchData = async () => {
      const response = await fetch("https://localhost:7015/subject/list");
      console.log("Response called");
      if (response.ok) {
        console.log("Response is ok");
        // console.log("Response from server: ", response)
        const data = await response.json();
        console.warn("Huh?");
        console.log("Data from server: ", data);

        const subjectData = data.map((subject) => {
          return {
            id: subject.id,
            name: subject.name,
          };
        });

        console.log("Subject data: ", subjectData);

        setSubjects(subjectData);

        console.log("Subjects: ", subjects);

        const names = subjectData.map((subject) => subject.name);
        console.log("Subject names: ", names);

        setSubjectNames(names);
      } else {
        console.error("Something went wrong fetching data from server");
      }
    };

    fetchData();
  }, [refreshSubjects]);

  const handleAddSubject = async () => {
    if (newSubject) {
      const requestBody = {
        name: newSubject,
      };
      console.log(requestBody);
      const response = await fetch("https://localhost:7015/subject/upload", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(requestBody),
      });
      if (response.ok) {
        const data = await response.json();

        subjects.push({
          name: data.name,
          id: data.id,
        });

        setSubjects(subjects);
        setNewSubject("");
        setShowDialog(false);
        console.log("Subject added successfully");
        setRefreshSubjects(!refreshSubjects);
      } else {
        console.error("Something went wrong adding subject");
      }
    }
  };

  const toggleDialog = () => {
    if (localStorage.getItem("loginToken") === null) {
      alert("You must be logged in to add a subject");
      return;
    }
    setShowDialog(!showDialog);
    setKeepDialog(true);
  };

  return (
    <div className="default-page-background">
      <div className="subjects-top-container">
        <h1>Subjects</h1>
        <div className="button-container">
          <div className="add-subject" onClick={toggleDialog}>
            <span className="plus-icon">+</span>
          </div>
        </div>
      </div>
      <div className="subjects-container">
        <div className="subjects-grid">
          {subjects.map((subject, index) => (
            <div
              key={index}
              className="subject-grid-item"
              title={subject.name}
              onClick={() => navigate("/Subjects/" + subject.id)}
            >
              <h2>{subjectNames[index]}</h2>
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
              placeholder="Subject Name"
              value={newSubject}
              onChange={(e) => setNewSubject(e.target.value)}
            />
            <div className="dialog-buttons">
              <button onClick={handleAddSubject}>Create</button>
              <button onClick={() => setShowDialog(false)}>Cancel</button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
