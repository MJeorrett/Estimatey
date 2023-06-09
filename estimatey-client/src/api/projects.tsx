import { ApiResponse } from "./common/apiResponse";
import useFetch from "./common/useFetch";
import { Project } from "./models/project";

export const useProjects = () => {
    const { response, isLoading, refetch } =
        useFetch<ApiResponse<Project[]>>(
            "api/key-deliverables"
        );

    return {
        projects: response?.content ?? [],
        isLoading,
        refetch,
    };
};
