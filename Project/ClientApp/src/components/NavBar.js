import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import "./NavBar.css";

export default function NavBar({ onUpdateSeed }) {
  const [seed, setSeed] = useState(0); // Used to force re-rendering of the NavBar

  const handleLogout = () => {
    try {
      localStorage.clear();
    } catch (e) {
      console.error("Error clearing localStorage:", e);
    }
  };

  const updateSeed = () => {
    setSeed((prevSeed) => prevSeed + 1);
  };

  return (
    <nav className="nav" key={seed}>
      <a style={{fontWeight: "800", fontSize: 30}} href="/">Noteblend</a>
      <ul className="pd-0 mg-0 list-none flex gap-4">
        <li>
          <Link to="/Subjects" onClick={updateSeed}>
            Subjects
          </Link>
        </li>
        <li>
          {localStorage.getItem("loginToken") ? (
            <Link
              onClick={() => {
                handleLogout();
                updateSeed();
              }}
            >
              Logout
            </Link>
          ) : (
            <Link to="/Login" onClick={updateSeed}>
              Login
            </Link>
          )}
        </li>
        <li>
          <a className="welcome-msg">
            Welcome,&nbsp;
            {localStorage.getItem("username")
              ? localStorage.getItem("username")
              : "Guest"}
            !
          </a>
        </li>
      </ul>
    </nav>
  );
}
