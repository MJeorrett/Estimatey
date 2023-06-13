import { Link, BrowserRouter as Router } from "react-router-dom";
import AppRoutes from "./Routes";
import appPaths from "./appPaths";

const App = () => {
  return (
    <div className="p-4">
      <Router>
        <Link className="text-2xl underline" to={appPaths.home}>Home</Link>
        <AppRoutes />
      </Router>
    </div>
  )
}

export default App
