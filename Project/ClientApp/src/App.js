import React, { Component } from "react";
import { Route, Routes } from "react-router-dom";
import AppRoutes from "./AppRoutes";
import { Layout } from "./components/Layout";
import "./custom.css";
import "./userContext";
import { UserProvider } from "./userContext";
import { Home } from "./components/Home";
import { Subject } from "./components/Subject";
import { Topic } from "./components/Topic";
import {Note} from "./components/Note";
import Login from "./components/Login";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
    <UserProvider>
      <Layout>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/Subjects" element={<Subject />} />
          <Route path="/Subjects/:id" element={<Topic />} />
          <Route path="/Subjects/:id/:topicId" element={<Note />} />
          <Route path="/Login" element={<Login />} />
          {/*{AppRoutes.map((route, index) => {*/}
          {/*  const { element, ...rest } = route;*/}
          {/*  return <Route key={index} {...rest} element={element} />;*/}
          {/*})}*/}
        </Routes>
      </Layout>
    </UserProvider>
    );
  }
}
