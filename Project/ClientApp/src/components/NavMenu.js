import React, { Component, useEffect, useState } from "react";
import {
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";
import { Link, useNavigate } from "react-router-dom";
import "./NavMenu.css";
import { useUserContext } from "../userContext";

export const NavMenu = () => {
  const [collapsed, setCollapsed] = useState(true);
  const { username, userEmail, setUsername, setUserEmail } = useUserContext();
  const navigate = useNavigate();

  useEffect(() => {
    const setUserDetails = async () => {
      const cachedUsername = localStorage.getItem("username");
      const cachedUserEmail = localStorage.getItem("userEmail");
      if (cachedUsername) {
        setUsername(cachedUsername);
      }

      if (cachedUserEmail) {
        setUserEmail(cachedUserEmail);
      }
    };
    console.log("Username: ", username);
    console.log("UserEmail: ", userEmail);
    setUserDetails();
  }, [username, userEmail]);

  const handleToggleNavbar = () => {
    setCollapsed(!collapsed);
  };

  const handleLogout = () => {
    try {
      localStorage.clear();
    } catch (e) {
      console.error("Error clearing localStorage:", e);
    }
    setUsername("");

    navigate("/login");
    // window.location.reload();
  };

  return (
    <header>
      <Navbar
        className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
        container
        light
      >
        <NavbarBrand tag={Link} to="/">
          NoteBlend
        </NavbarBrand>
        <NavbarToggler onClick={handleToggleNavbar} className="mr-2" />
        <Collapse
          className="d-sm-inline-flex flex-sm-row-reverse"
          isOpen={!collapsed}
          navbar
        >
          <ul className="navbar-nav flex-grow">
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/Subjects">
                Subjects
              </NavLink>
            </NavItem>
            <NavItem>
              {localStorage.getItem("loginToken") ? ( // Check if loginToken exists in localStorage
                <button
                  onClick={handleLogout}
                  className="btn btn-link text-dark"
                  // style={{ textDecoration: "none" }}
                >
                  Logout
                </button>
              ) : (
                <NavLink tag={Link} className="text-dark" to="/login">
                  Login/Register
                </NavLink>
              )}
            </NavItem>
            <NavItem>
              <p className="welcome-msg">
                Welcome,{" "}
                {localStorage.getItem("username")
                  ? localStorage.getItem("username")
                  : "Guest"}
                !
              </p>
            </NavItem>
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
};
