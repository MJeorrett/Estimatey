import { ApiResponse } from "./common/apiResponse";
import useFetch from "./common/useFetch";
import { DeveloperLoggedTime } from "./models/developerLoggedTime";

export const useDeveloperLoggedTime = (projectId: number) => {
    const { response, isLoading, refetch } =
        useFetch<ApiResponse<DeveloperLoggedTime[]>>(
            `api/projects/${projectId}/logged-time`
        );

    return {
        loggedTimeByDeveloper: response?.content ?? [],
        isLoading,
        refetch,
    };
};