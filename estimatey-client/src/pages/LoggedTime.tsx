import { useDeveloperLoggedTime } from "../api/loggedTime"
import { useProject } from "../api/projects";
import { useProjectId } from "../appPaths";
import Avatar from "../components/Avatar";
import Loader from "../components/Loader";
import PageTitle from "../components/PageTitle"
import { getInitials } from "../utils/getInitials";

const LoggedTimePage = () => {
    const projectId = useProjectId();
    const { project, isLoading: projectIsLoading } = useProject(projectId)
    const { loggedTimeByDeveloper, isLoading: loggedTimeIsLoading } = useDeveloperLoggedTime(projectId);

    const totalLoggedTime = loggedTimeByDeveloper.reduce((total, developer) => total + developer.totalLoggedHours, 0);

    if (projectIsLoading || !project || loggedTimeIsLoading || !loggedTimeByDeveloper) return <Loader />

    return (
        <div className="p-8">
            <PageTitle>{project.title} Logged Time</PageTitle>
            <p className="text-3xl mb-12 text-center text-gray-400">Total logged developer time {totalLoggedTime.toFixed(1)}hrs</p>
            <div className="px-16 max-w-lg mx-auto space-y-4">
                <div className="grid grid-cols-[1fr_100px] gap-8 text-2xl">
                    {loggedTimeByDeveloper.map(({ developerName, totalLoggedHours }) => (
                        <>
                            <div className="flex gap-4">
                                <Avatar initials={getInitials(developerName)} />
                                <p className="whitespace-nowrap">{developerName}</p>
                            </div>
                            <p>{totalLoggedHours.toFixed(1)}hrs</p>
                        </>
                    ))}
                </div>
            </div>
        </div>
    )
}

export default LoggedTimePage;
