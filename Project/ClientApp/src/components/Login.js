import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useUserContext } from "../userContext";

const Login = () => {
  const { setUserEmail, setUserId, setUsername, userEmail } = useUserContext();
  const [loginInputEmail, setLoginInputEmail] = useState("");
  const [loginInputPassword, setLoginInputPassword] = useState("");
  const [registerInputEmail, setRegisterInputEmail] = useState("");
  const [registerInputName, setRegisterInputName] = useState("");
  const [registerInputSurname, setRegisterInputSurname] = useState("");
  const [registerInputPassword, setRegisterInputPassword] = useState("");
  const navigate = useNavigate();

  const requestBody = {
    name: "login",
    surname: "login",
    email: loginInputEmail,
    password: loginInputPassword,
  };

  const handleLogin = async () => {
    if (!loginInputEmail || !loginInputPassword) {
      alert("Login failed: All login fields have to be filled.");
      return;
    }

    const response = await fetch("https://localhost:7015/user/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(requestBody),
    });

    if (!response.ok) {
      const errorMessage = await response.text();
      alert(`Login failed: ${errorMessage}` + ".");
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
    // localStorage.setItem("loginSurname", userDataJson['surname']);
    setUserEmail(loginInputEmail);
    setUsername(userDataJson["name"]);

    console.log("UserContext userEmail: ", userEmail);
    navigate("/");
  };

  const handleRegister = async () => {
    if (
      !registerInputEmail ||
      !registerInputName ||
      !registerInputSurname ||
      !registerInputPassword
    ) {
      alert("Registration failed: All register fields have to be filled.");
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
    } else {
      const errorMessage = await response.text();
      alert(`Registration failed: ${errorMessage}` + ".");
    }
  };

  if (localStorage.getItem("loginToken")) {
    window.location.href = "/";
    return;
  }

  return (
    <div>
      <h2>Login</h2>
      <input
        type="email"
        placeholder="Enter email"
        value={loginInputEmail}
        onChange={(e) => setLoginInputEmail(e.target.value)}
      />
      <input
        type="password"
        placeholder="Enter password"
        value={loginInputPassword}
        onChange={(e) => setLoginInputPassword(e.target.value)}
      />
      <button onClick={handleLogin}>Login</button>
      <h2>Register</h2>
      <input
        type="email"
        placeholder="Enter email"
        value={registerInputEmail}
        onChange={(e) => setRegisterInputEmail(e.target.value)}
      />
      <input
        type="text"
        placeholder="Enter name"
        value={registerInputName}
        onChange={(e) => setRegisterInputName(e.target.value)}
      />
      <input
        type="text"
        placeholder="Enter surname"
        value={registerInputSurname}
        onChange={(e) => setRegisterInputSurname(e.target.value)}
      />
      <input
        type="password"
        placeholder="Enter password"
        value={registerInputPassword}
        onChange={(e) => setRegisterInputPassword(e.target.value)}
      />
      <button onClick={handleRegister}>Register</button>
      {/*<h5 style={{color: "green"}}>*/}
      {/*{username ? "Logged in as " + username : ""}*/}
      {/*</h5>*/}
    </div>
  );
};

export default Login;
