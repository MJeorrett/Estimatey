import { TicketSummary, ticketState } from "../../api/models/ticket";

type FeatureTicketsProps = {
    tickets: TicketSummary[];
}

const FeatureTickets = ({ tickets }: FeatureTicketsProps) => {
    if (tickets.length === 0) {
        return (
            <p>Not Refined</p>
        )
    }

    if (tickets.every(_ => _.state === ticketState.complete)) {
        return (
            <p>Complete</p>
        )
    }

    if (tickets.every(_ => _.state === ticketState.new)) {
        return (
            <p>{tickets.length} Ticket{tickets.length === 1 ? "" : "s"}</p>
        )
    }

    const completedTicketCount = tickets.filter(_ => _.state === ticketState.complete).length;
    const percentageComplete = Math.round((completedTicketCount / tickets.length) * 100);

    return (
        <p>{percentageComplete}% Complete</p>
    )
}

export default FeatureTickets;
