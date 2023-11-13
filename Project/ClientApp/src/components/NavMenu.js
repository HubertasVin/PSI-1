import React, { Component } from "react";
import {
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";
import { Link } from "react-router-dom";
import "./NavMenu.css";
import { UserContext } from "../userContext";

export class NavMenu extends Component {
  static displayName = NavMenu.name;
  static contextType = UserContext;

  constructor(props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed,
    });
  }

  handleLogout = () => {
    localStorage.removeItem("loginToken");
    window.location.reload();
  };

  render() {
    const { username } = this.context;
    console.log("Username: ", username);

    return (
      <UserContext.Consumer>
        {(userData) => (
          <header>
            <Navbar
              className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
              container
              light
            >
              <NavbarBrand tag={Link} to="/">
                Project
              </NavbarBrand>
              <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
              <Collapse
                className="d-sm-inline-flex flex-sm-row-reverse"
                isOpen={!this.state.collapsed}
                navbar
              >
                <ul className="navbar-nav flex-grow">
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/">
                      Home
                    </NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/counter">
                      Counter
                    </NavLink>
                  </NavItem>
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/fetch-data">
                      Fetch data
                    </NavLink>
                  </NavItem>
                  <NavItem>
                    {localStorage.getItem("loginToken") ? ( // Check if loginToken exists in localStorage
                      <button
                        onClick={this.handleLogout}
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
                    <p className="welcome-msg">Welcome, {username ? username : "Guest"}!</p>
                  </NavItem>
                </ul>
              </Collapse>
            </Navbar>
          </header>
        )}
      </UserContext.Consumer>
    );
  }
}
