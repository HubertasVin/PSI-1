import { useState, useEffect, useCallback } from "react";
import { useUserContext } from "../userContext";

export const PrivateNote = ({ show, onClose, topicId }) => {
    const { userEmail } = useUserContext();
    const [notes, setNotes] = useState([]);
    const [currentNote, setCurrentNote] = useState("");

    useEffect(() => {
        // Fetching notes when the component mounts
        fetch(`https://localhost:7015/note/get/${topicId}`)
            .then((response) => response.json())
            .then((data) => {
                setNotes(data);
            });
    }, [topicId]);

    const handleSendNote = async () => {
        // Sending a new note to the server
        const response = await fetch("https://localhost:7015/note/add", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                topicId: topicId,
                userId: userEmail,
                content: currentNote,
            }),
        });

        if (response.ok) {
            // If the note is successfully saved, update the local state
            const newNote = await response.json();
            setNotes((prevNotes) => [...prevNotes, newNote]);
            setCurrentNote("");
        }
    };

    if (!show) {
        return null;
    }

    return (
        <div className="note-panel">
            <div className="note-header">
                <h2>Notes</h2>
                <button onClick={onClose}>Close</button>
            </div>
            <div className="note-body">
                {notes.map((note, index) => (
                    <div key={index} className="note">
                        <div className="note-content">{note.content}</div>
                    </div>
                ))}
            </div>
            <div className="note-footer">
                <input
                    value={currentNote}
                    onChange={(e) => setCurrentNote(e.target.value)}
                    placeholder="Enter your note..."
                />
                <button onClick={handleSendNote}>Save</button>
            </div>
        </div>
    );
};