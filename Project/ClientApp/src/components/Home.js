import { useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";

export const Home = () => {
  const navigate = useNavigate();
  const token = localStorage.getItem("loginToken");

  useEffect(() => {
    if (token) {
      navigate("/Subjects");
    } else {
      navigate("/Login");
    }
  }, []);
};
