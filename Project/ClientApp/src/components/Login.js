import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useUserContext } from "../userContext";
import "./Login.css";
import "./common.css";

const Login = ({ onUpdateSeed }) => {
  const { setUserEmail, setUserId, setUsername, userEmail } = useUserContext();
  const [loginInputEmail, setLoginInputEmail] = useState("");
  const [loginInputPassword, setLoginInputPassword] = useState("");
  const [registerInputEmail, setRegisterInputEmail] = useState("");
  const [registerInputName, setRegisterInputName] = useState("");
  const [registerInputSurname, setRegisterInputSurname] = useState("");
  const [registerInputPassword, setRegisterInputPassword] = useState("");
  const [registerInputRepeatPassword, setRegisterInputRepeatPassword] =
    useState("");
  const [isValidEmail, setIsValidEmail] = useState(null);
  const [passwordsMatch, setPasswordsMatch] = useState(false);
  const [loginFail, setLoginFail] = useState(null);
  const [registrationFail, setRegistrationFail] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    handlePasswordValidation();
  }, [registerInputPassword, registerInputRepeatPassword]);

  const handlePasswordValidation = () => {
    if (registerInputPassword !== registerInputRepeatPassword) {
      setPasswordsMatch(false);
    } else {
      setPasswordsMatch(true);
    }
  };

  const passwordClass = passwordsMatch ? "" : "passwordMismatch";

  const emailRegex = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g;

  const validateEmail = (e) => {
    if (e.target?.value == "" || e.target.value.match(emailRegex)) {
      setIsValidEmail(true);
    } else {
      setIsValidEmail(false);
    }
  };

  const handleLogin = async (loginEmail, loginPassword) => {
    if (!loginEmail || !loginPassword) {
      // alert("Login failed: All login fields have to be filled.");
      setLoginFail("All login fields have to be filled.");
      return;
    }

    const requestBody = {
      name: "login",
      surname: "login",
      email: loginEmail,
      password: loginPassword,
    };

    const response = await fetch("https://localhost:7015/user/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(requestBody),
    });

    if (!response.ok) {
      const errorMessage = await response.text();
      // alert(`Login failed: ${errorMessage}` + ".");
      setLoginFail(`${errorMessage}.`);
      return;
    }

    const loginToken = await response.text();
    const getUserDataResponse = await fetch(
      "https://localhost:7015/user/get/" + loginToken,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );
    const userDataJson = await getUserDataResponse.json();
    localStorage.setItem("loginToken", loginToken);
    localStorage.setItem("username", userDataJson["name"]);
    localStorage.setItem("userEmail", userDataJson["email"]);
    setUserEmail(loginEmail);
    setUsername(userDataJson["name"]);

    onUpdateSeed();
    navigate("/Subjects");
  };

  const handleRegister = async () => {
    if (
      !registerInputEmail ||
      !registerInputName ||
      !registerInputSurname ||
      !registerInputPassword
    ) {
      // alert("Registration failed: All register fields have to be filled.");
      setRegistrationFail("All register fields have to be filled.");
      return;
    }

    if (registerInputPassword !== registerInputRepeatPassword) {
      alert("Registration failed: Passwords do not match.");
      return;
    }

    const requestBody = {
      name: registerInputName,
      surname: registerInputSurname,
      email: registerInputEmail,
      password: registerInputPassword,
    };
    const response = await fetch("https://localhost:7015/user/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(requestBody),
    });

    if (response.ok) {
      alert("Registration successful, you can now log in.");

      handleLogin(registerInputEmail, registerInputPassword);
    } else {
      const errorMessage = await response.text();
      setRegistrationFail(`${errorMessage}.`);
      // alert(`Registration failed: ${errorMessage}` + ".");
    }
  };

  if (localStorage.getItem("loginToken")) {
    window.location.href = "/Subjects";
    return;
  }

  return (
    <div className="container">
      <div className="project-name">
        <h1>Login / Register</h1>
      </div>
      <div className="authContainer">
        <div className="authForm">
          <h2 className="login">Login</h2>
          <input
            type="email"
            placeholder="Enter email"
            value={loginInputEmail}
            onChange={(e) => setLoginInputEmail(e.target.value)}
            required
          />
          <input
            type="password"
            placeholder="Enter password"
            value={loginInputPassword}
            onChange={(e) => setLoginInputPassword(e.target.value)}
            required
          />
          <button
            onClick={() => handleLogin(loginInputEmail, loginInputPassword)}
          >
            Login
          </button>
          {loginFail !== null && <p style={{ color: "red" }}>{loginFail}</p>}
          {/*</form>*/}
        </div>
        <div className="authForm">
          <h2 className="login">Register</h2>
          {/*<form onSubmit={handleRegister}>*/}
          <input
            type="email"
            placeholder="Enter email"
            value={registerInputEmail}
            onChange={(e) => {
              setRegisterInputEmail(e.target.value);
              validateEmail(e);
            }}
            required
          />
          {isValidEmail === false && (
            <p style={{ color: "red" }}>Email is not valid.</p>
          )}
          <input
            type="text"
            placeholder="Enter name"
            value={registerInputName}
            onChange={(e) => setRegisterInputName(e.target.value)}
            required
          />
          <input
            type="text"
            placeholder="Enter surname"
            value={registerInputSurname}
            onChange={(e) => setRegisterInputSurname(e.target.value)}
            required
          />
          <input
            className={passwordClass}
            type="password"
            placeholder="Enter password"
            value={registerInputPassword}
            onChange={(e) => setRegisterInputPassword(e.target.value)}
            required
          />
          <input
            className={passwordClass}
            type="password"
            placeholder="Repeat password"
            value={registerInputRepeatPassword}
            onChange={(e) => setRegisterInputRepeatPassword(e.target.value)}
            required
          />
          {!passwordsMatch && (
            <p style={{ color: "red" }}>The passwords do not match.</p>
          )}
          <button onClick={handleRegister} disabled={!passwordsMatch}>
            Register
          </button>
          {registrationFail !== null && (
            <p style={{ color: "red" }}>{registrationFail}</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default Login;
