import { useParams } from "react-router-dom";

const appPaths = {
    home: "/",
    projectFeatures: (projectId: string | number) => `/projects/${projectId}/features`,
    projectLoggedTime: (projectId: string | number) => `/projects/${projectId}/logged-time`,
}

const createUseId = (paramName: string) => (): number => {
    const params = useParams();
    const param = params[paramName];

    if (!param) throw new Error(`No ${paramName} param available.`);

    return parseInt(param);
}

export const useProjectId = createUseId("projectId");

export default appPaths;
