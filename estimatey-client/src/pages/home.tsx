import { Link } from "react-router-dom";
import { useProjects } from "../api/projects";
import appPaths from "../appPaths";

const HomePage = () => {
    const { projects } = useProjects();

    return (
        <div className="p-8">
            <h1 className="text-5xl">Features</h1>
            {projects.map(project => (
                <Link to={appPaths.projectFeatures(project.id)}>
                    <span>{project.title}</span>
                </Link>
            ))}
        </div>
    )
}

export default HomePage;
