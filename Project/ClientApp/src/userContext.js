import { createContext, useContext, useState, useEffect } from 'react';

export const UserContext = createContext(); // Add export here

export const useUserContext = () => {
    return useContext(UserContext);
};

export const UserProvider = ({ children }) => {
    const [userEmail, setUserEmail] = useState('');
    const [username, setUsername] = useState('');
    const [userID, setUserID] = useState('');

    useEffect(() => {
        // Fetch user data using localStorage.getItem("loginToken")
        // This could be an API call to get user information based on the login token

        // Example API call:
        fetch(`https://localhost:7015/user/get/${localStorage.getItem("loginToken")}`)
        .then(response => response.json())
        .then(data => {
            setUsername(data.name); // Update user data obtained from the API
            setUserEmail(data.email);
        })
        .catch(error => {
            console.error('Error fetching user data:', error);
        });
    }, []);

    return (
        <UserContext.Provider value={{ userID, setUserID, userEmail, setUserEmail, username, setUsername }}>
            {children}
        </UserContext.Provider>
    );
};