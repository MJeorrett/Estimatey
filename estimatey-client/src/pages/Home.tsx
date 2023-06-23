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
                    <div className="flex gap-4 text-2xl border border-gray-500 py-2 px-4 rounded-l hover:bg-gray-100" key={project.id}>
                        {project.title}
                        <Link className="underline ml-auto" to={appPaths.projectFeatures(project.id)}>Work</Link>
                        <Link className="underline" to={appPaths.projectLoggedTime(project.id)}>Time</Link>
                    </div>
                ))}
            </div>
        </div>
    )
}

export default HomePage;
