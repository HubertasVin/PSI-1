import { useState, useEffect} from 'react';
import {useParams} from "react-router-dom";
import {Comment} from "./Comment";

export const Note = () => {
    const {topicId} = useParams();
    const [showComments, setShowComments] = useState(false);
    const [topicName, setTopicName] = useState('');

    useEffect(() => {
        // console.log(showComments)
        fetch("https://localhost:7015/topic/get/"+topicId)
            .then(response => response.json())
            .then(data => {
                setTopicName(data.name);
            })
    }, []);
    
    const OpenedComments = () => {
        setShowComments(true);
        console.log("Set show comments to true")
    };

    const ClosedComments = () => {
        setShowComments(false);
        console.log("Set show comments set to" + showComments)
    };
    
    return(
        <div>
            <div className="comment-panel">
                <p>{topicName}</p>
                <button onClick={() => setShowComments(true)}>Comments</button>
            </div>
            <Comment
                show={showComments}
                onClose={ClosedComments}
                topicId={topicId}
            />
        </div>
      
        
    );
}