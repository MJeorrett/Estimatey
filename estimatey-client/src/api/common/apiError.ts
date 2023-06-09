export type ApiErrorResponse = {
    message: string;
    errors?: {
        [k: string]: string[];
    };
};

class ApiError extends Error {
    statusCode?: number;
    userMessages: string[] = [];
    isApiError = true;

    constructor(message: string) {
        super(message);
        Object.setPrototypeOf(this, ApiError.prototype);
    }
}

export const buildApiError = async (
    method: string,
    url: string,
    response: Response
) => {
    const error = new ApiError(`Error calling ${method} ${url}.`);

    try {
        const responseContent: ApiErrorResponse = await response.json();
        error.userMessages = parseErrorMessages(responseContent);
        error.statusCode = response.status;
    } catch (_) {
        // Do nothing, we'll make do with just the main error message.
    }

    return error;
};

export const isApiError = (error: unknown): error is ApiError => {
    return (error as ApiError).isApiError !== undefined;
};

const parseErrorMessages = (apiError: ApiErrorResponse): string[] => {
    const messages: string[] = [];

    if (apiError.message) {
        messages.push(apiError.message);
    }

    const errors = apiError.errors;

    if (errors) {
        Object.keys(errors).forEach((errorKey) => {
            const errorMessages = errors[errorKey];
            messages.push(...errorMessages);
        });
    }

    return messages;
};

export default ApiError;
