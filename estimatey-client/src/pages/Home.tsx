import { Link } from "react-router-dom";
import { useProjects } from "../api/projects";
import appPaths from "../appPaths";
import PageTitle from "../components/PageTitle";

const HomePage = () => {
    const { projects } = useProjects();

    return (
        <div className="p-8">
            <PageTitle>Features</PageTitle>
            <div className="space-y-4">
                {projects.map(project => (
                    <Link className="block text-2xl border border-gray-500 py-2 px-4 rounded-l hover:bg-gray-100" key={project.id} to={appPaths.projectFeatures(project.id)}>
                        {project.title}
                    </Link>
                ))}
            </div>
        </div>
    )
}

export default HomePage;
