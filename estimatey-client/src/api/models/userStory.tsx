import { TicketSummary } from "./ticket";

export type UserStory = {
    id: number;
    title: string;
    state: string;
    tickets: TicketSummary[];
}