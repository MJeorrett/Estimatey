import { useState } from "react";
import { useAuth } from "react-oidc-context";
import config from "../../config.json";
import ApiError, { buildApiError, isApiError } from "./apiError";

type AuthenticatedApiRequestMethod = "POST" | "PUT" | "DELETE" | "GET";

export type AuthenticatedApiResponse<T> = {
    content: T;
    message: string;
};

export type ApiFetcherResponse<TRes> =
    | {
          success: true;
          content: TRes;
      }
    | {
          success: false;
          message?: string;
          error: ApiError;
      };

export const useAuthenticatedApiRequest = <TReq, TRes>(
    method: AuthenticatedApiRequestMethod,
    path: string
): [
    request: (body?: TReq | undefined) => Promise<ApiFetcherResponse<TRes>>,
    isLoading: boolean
] => {
    const auth = useAuth();
    const [isLoading, setIsLoading] = useState(false);

    const request = async (body?: TReq): Promise<ApiFetcherResponse<TRes>> => {
        try {
            const url = `${config.CarbonApiBaseUrl}/${path}`;
            const token = auth.user?.access_token;

            if (!token) throw new Error("Failed to retrieve access token.");

            setIsLoading(true);
            const response = await fetch(url, {
                method,
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": body
                        ? "application/json"
                        : "text/plain;charset=UTF-8",
                },
                body: body ? JSON.stringify(body) : undefined,
            });

            setIsLoading(false);

            if (!response.ok) {
                const error = await buildApiError(method, url, response);
                throw error;
            }

            const responseContent: AuthenticatedApiResponse<TRes> =
                await response.json();

            return {
                success: true,
                content: responseContent.content,
            };
        } catch (error) {
            if (isApiError(error)) {
                return {
                    success: false,
                    error,
                };
            }

            throw error;
        }
    };

    return [request, isLoading];
};
