import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { Subject } from "./components/Subject";
import Login from "./components/Login";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
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