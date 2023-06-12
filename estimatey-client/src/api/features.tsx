import { ApiResponse } from "./common/apiResponse";
import useFetch from "./common/useFetch";
import { FeatureSummary } from "./models/feature";

export const useFeatures = (projectId: number) => {
    const { response, isLoading, refetch } =
        useFetch<ApiResponse<FeatureSummary[]>>(
            `api/projects/${projectId}/features`
        );

    return {
        features: response?.content ?? [],
        isLoading,
        refetch,
    };
};