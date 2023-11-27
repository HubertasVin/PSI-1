import React, {useEffect, useState} from "react";
import {useNavigate} from "react-router-dom";
import {useUserContext} from "../userContext";
import "./Login.css";

const Login = () => {
    const {setUserEmail, setUserId, setUsername, userEmail} = useUserContext();
    const [loginInputEmail, setLoginInputEmail] = useState("");
    const [loginInputPassword, setLoginInputPassword] = useState("");
    const [registerInputEmail, setRegisterInputEmail] = useState("");
    const [registerInputName, setRegisterInputName] = useState("");
    const [registerInputSurname, setRegisterInputSurname] = useState("");
    const [registerInputPassword, setRegisterInputPassword] = useState("");
    const [registerInputRepeatPassword, setRegisterInputRepeatPassword] = useState("");
    const [passwordsMatch, setPasswordsMatch] = useState(false);
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
    
    const passwordClass = passwordsMatch ? '' : 'passwordMismatch';
    
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
        navigate("/Subjects");
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
        } else {
            const errorMessage = await response.text();
            alert(`Registration failed: ${errorMessage}` + ".");
        }
    };

    if (localStorage.getItem("loginToken")) {
        window.location.href = "/Subjects";
        return;
    }

    return (
        <div className="container">
            <div className="project-name">
                <h1>NoteBlend</h1>
            </div>
            <div className="authContainer">
                <div className="authForm">
                    <h2 className="login">Login</h2>
                    {/*<form onSubmit={handleLogin}>*/}
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
                    {/*</form>*/}
                </div>
                <div className="authForm">
                    <h2 className="login">Register</h2>
                    {/*<form onSubmit={handleRegister}>*/}
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
                            className={passwordClass}
                            type="password"
                            placeholder="Enter password"
                            value={registerInputPassword}
                            onChange={(e) => setRegisterInputPassword(e.target.value)}
                        />
                        <input
                            className={passwordClass}
                            type="password"
                            placeholder="Repeat password"
                            value={registerInputRepeatPassword}
                            onChange={(e) => setRegisterInputRepeatPassword(e.target.value)}
                        />
                        <button onClick={handleRegister} disabled={!passwordsMatch}>Register</button>
                        {!passwordsMatch && (
                            <p className="error-message">Passwords do not match.</p>
                        )}
                    {/*</form>*/}
                </div>
            </div>
        </div>
    );
};

export default Login;
