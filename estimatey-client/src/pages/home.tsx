import { useProjects } from "../api/projects";

const HomePage = () => {
    const { projects } = useProjects();

    return (
        <div>
            <h1>Welcome to Estimatey!</h1>
            <pre>{JSON.stringify(projects, null, 2)}</pre>
        </div>
    )
}

export default HomePage;
