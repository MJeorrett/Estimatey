import { Route, Routes } from "react-router-dom"
import HomePage from "./pages/Home"
import ProjectFeatures from "./pages/ProjectFeatures"
import appPaths from "./appPaths"

const AppRoutes = () => {
    return (
        <Routes>
            <Route path={appPaths.home} element={<HomePage />} />
            <Route path={appPaths.projectFeatures(":projectId")} element={<ProjectFeatures />} />
        </Routes>
    )
}

export default AppRoutes;
