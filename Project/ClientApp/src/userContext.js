import { createContext, useContext, useState } from 'react';

const UserContext = createContext();

export const useUserContext = () => {
    return useContext(UserContext);
};

export const UserProvider = ({ children }) => {
    const [userEmail, setUserEmail] = useState('');
    const [username, setUsername] = useState('');

    return (
        <UserContext.Provider value={{ userEmail, setUserEmail, username, setUsername }}>
            {children}
        </UserContext.Provider>
    );
};