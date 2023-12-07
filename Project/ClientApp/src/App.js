import React, { Component } from "react";
import { Route, Routes } from "react-router-dom";
import "./custom.css";
import "./userContext";
import { UserProvider } from "./userContext";
import { Home } from "./components/Home";
import { Subject } from "./components/Subject";
import { Topic } from "./components/Topic";
import { Note } from "./components/Note";
import Login from "./components/Login";
import NavBar from "./components/NavBar";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <UserProvider>
        <NavBar onUpdateSeed={() => this.forceUpdate()} />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/Subjects" element={<Subject />} />
          <Route path="/Subjects/:id" element={<Topic />} />
          <Route path="/Subjects/:subjectId/:topicId" element={<Note />} />
          <Route
            style={{ width: "100%" }}
            path="/Login"
            element={<Login onUpdateSeed={() => this.forceUpdate()} />}
          />
        </Routes>
      </UserProvider>
    );
  }
}
