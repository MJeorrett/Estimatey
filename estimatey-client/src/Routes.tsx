import { Route, Routes } from "react-router-dom"
import HomePage from "./pages/home"

export const appPaths = {
    home: "/",
}

const AppRoutes = () => {
    return (
        <Routes>
            <Route path={appPaths.home} element={<HomePage />} />
        </Routes>
    )
}

export default AppRoutes;
