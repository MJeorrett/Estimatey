import { useEffect, useState } from "react";
import config from "../../config.json";

export type AuthenticatedFetchReturn<TResponse> = {
    response?: TResponse;
    isLoading: boolean;
    refetch: () => void;
};

const useFetch = <TResponse,>(
    path: string
): AuthenticatedFetchReturn<TResponse> => {
    const [invokedDateTime, setInvokeDateTime] = useState(new Date());
    const [response, setResponse] = useState<TResponse | undefined>();

    const refetch = () => setInvokeDateTime(new Date());

    useEffect(() => {
        (async () => {
            try {
                const url = `${config.EstimateyApiBaseUrl}/${path}`;

                const response = await fetch(url);
                setResponse(await response.json());
            } catch (error) {
                console.error(error);
            }
        })();
    }, [path, invokedDateTime]);

    return {
        response,
        isLoading: !response,
        refetch,
    };
};

export default useFetch;
