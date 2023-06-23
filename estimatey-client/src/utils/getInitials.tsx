export const getInitials = (displayName?: string): string => {

    try {
        if (!displayName) return "--";

        const initialsArray = displayName?.split(" ").map(word => word ? word[0] : "");
        const firstInitial = initialsArray[0];
        const lastInitial = initialsArray.length > 1 ? initialsArray.slice(-1) : null;
        return firstInitial + (lastInitial || "");
    }
    catch (err) {
        console.error("Failed to parse initials from display name.", err);
        return "--";
    }
};