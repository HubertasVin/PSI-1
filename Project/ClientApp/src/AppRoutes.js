import { Home } from "./components/Home";
import { Subject } from "./components/Subject";
import Login from "./components/Login";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/subjects',
    element: <Subject/>
  },
  {
    path: '/login',
    element: <Login />
  }
];

export default AppRoutes;