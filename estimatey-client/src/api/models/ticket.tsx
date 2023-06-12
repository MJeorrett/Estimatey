export type TicketSummary = {
    id: number;
    title: string;
    state: string;
}

export const ticketState = {
    new: "New",
    inProgress: "Active",
    complete: "Closed",
} as const;