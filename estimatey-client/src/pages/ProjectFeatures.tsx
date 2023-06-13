import { useFeatures } from "../api/features";
import { useProject } from "../api/projects"
import { useProjectId } from "../appPaths"
import FeaturesList from "../components/FeaturesList/FeaturesList";
import Loader from "../components/Loader";
import PageTitle from "../components/PageTitle";

const ProjectFeatures = () => {
    const projectId = useProjectId();
    const { project, isLoading: projectIsLoading } = useProject(projectId)
    const { features, isLoading: featuresAreLoading } = useFeatures(projectId);

    if (projectIsLoading || !project || featuresAreLoading || !features) return <Loader />

    return (
        <div className="p-8">
            <PageTitle>{project.title} Features</PageTitle>
            <FeaturesList features={features} />
        </div>
    )
}

export default ProjectFeatures;
