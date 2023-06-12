import { Check, Ticket, Circle } from "@phosphor-icons/react";

import { TicketSummary, ticketState } from "../../api/models/ticket";

type FeatureTicketsProps = {
    tickets: TicketSummary[];
}

const FeatureTickets = ({ tickets }: FeatureTicketsProps) => {
    if (tickets.length === 0) {
        return (
            <div className="grid grid-cols-[1fr_32px] gap-4 items-center">
                <p className="text-right">Not Refined</p>
                <span />
            </div>
        )
    }

    if (tickets.every(_ => _.state === ticketState.complete)) {
        return (
            <div className="grid grid-cols-[1fr_32px] gap-4 items-center">
                <p className="text-right">Complete</p>
                <Check size={32} weight="bold" className="text-green-600" />
            </div>
        )
    }

    if (tickets.every(_ => _.state === ticketState.new)) {
        return (
            <div className="grid grid-cols-[1fr_32px] gap-4 items-center">
                <p className="text-right">{tickets.length} Ticket{tickets.length === 1 ? "" : "s"}</p>
                <Ticket size={32} weight="fill" className="text-yellow-500" />
            </div>
        )
    }

    const completedTicketCount = tickets.filter(_ => _.state === ticketState.complete).length;
    const percentageComplete = Math.round((completedTicketCount / tickets.length) * 100);

    return (
        <div className="grid grid-cols-[1fr_32px] gap-4 items-center">
            <p className="text-right">{percentageComplete}% Complete</p>
            <Circle size={26} weight="fill" className="text-yellow-500" />
        </div>
    )
}

export default FeatureTickets;
