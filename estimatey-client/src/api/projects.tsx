import { ApiResponse } from "./common/apiResponse";
import useFetch from "./common/useFetch";
import { Project } from "./models/project";

export const useProjects = () => {
    const { response, isLoading, refetch } =
        useFetch<ApiResponse<Project[]>>(
            "api/projects"
        );

    return {
        projects: response?.content ?? [],
        isLoading,
        refetch,
    };
};

export const useProject = (projectId: number) => {
    const { response, isLoading, refetch } =
        useFetch<ApiResponse<Project>>(
            `api/projects/${projectId}`
        );

    return {
        project: response?.content,
        isLoading,
        refetch,
    };
};
