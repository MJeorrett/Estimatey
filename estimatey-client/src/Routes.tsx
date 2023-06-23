import { Route, Routes } from "react-router-dom"
import HomePage from "./pages/Home"
import ProjectFeatures from "./pages/ProjectFeatures"
import appPaths from "./appPaths"
import LoggedTimePage from "./pages/LoggedTime"

const AppRoutes = () => {
    return (
        <Routes>
            <Route path={appPaths.home} element={<HomePage />} />
            <Route path={appPaths.projectFeatures(":projectId")} element={<ProjectFeatures />} />
            <Route path={appPaths.projectLoggedTime(":projectId")} element={<LoggedTimePage />} />
        </Routes>
    )
}

export default AppRoutes;
