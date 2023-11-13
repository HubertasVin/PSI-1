import React, { useState } from "react";
import { useUserContext } from "../userContext";

const Login = () => {
  const { userEmail, setUserEmail, username, setUsername } = useUserContext();
  const [loginInputEmail, setLoginInputEmail] = useState("");
  const [loginInputPassword, setLoginInputPassword] = useState("");
  const [registerInputEmail, setRegisterInputEmail] = useState("");
  const [registerInputName, setRegisterInputName] = useState("");
  const [registerInputSurname, setRegisterInputSurname] = useState("");
  const [registerInputPassword, setRegisterInputPassword] = useState("");

  const requestBody = {
    userEmail: loginInputEmail,
    userPassword: loginInputPassword,
  };

  const handleLogin = async () => {
    if (!loginInputEmail || !loginInputPassword) {
      return;
    } else {
      const response = await fetch("https://localhost:7015/user/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(requestBody),
      });
  
      if (!response.ok) {
        alert("Incorrect email or password");
        return;
      }
  
      const loginToken = await response.text();
      localStorage.setItem("loginToken", loginToken);
      const userInfo = await fetch("https://localhost:7015/user/get/" + loginToken, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });
      const userData = await userInfo.json();
      console.log(userData.name);

      window.location.href = "/";
    }
  };

  const validateEmail = (email) => {
    return String(email)
      .toLowerCase()
      .match(
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      );
  };

  const handleRegister = async () => {
    if (!registerInputEmail || !registerInputName || !registerInputSurname || !registerInputPassword) {
      return;
    } else {
      if (!validateEmail(registerInputEmail)) {
        alert("Invalid email");
        return;
      }

      // Check if the email is already registered
      const checkEmailResponse = await fetch("https://localhost:7015/user/checkEmail", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ userEmail: registerInputEmail }),
      });
      
      if (!checkEmailResponse.ok) {
        alert("Email is already registered");
        return;
      }

      const requestBody = {
        userEmail: registerInputEmail,
        userName: registerInputName,
        userSurname: registerInputSurname,
        userPassword: registerInputPassword,
      };
      const response = await fetch("https://localhost:7015/user/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(requestBody),
      });
  
      if (!response.ok) {
        alert("Incorrect email or password");
        return;
      }

      const userInfo = await fetch("https://localhost:7015/user/get/" + localStorage.getItem("loginToken"), {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });
      const userData = await userInfo.json();
      console.log(userData.name);
    }
  }

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
      <h5 style={{ color: 'green' }}>{username ? "Logged in as " + username : ""}</h5>
    </div>
  );
};

export default Login;
