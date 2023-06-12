import { Trophy } from "@phosphor-icons/react";

import { FeatureSummary } from "../../api/models/feature"
import FeatureTickets from "./FeatureTickets";
import FeatureUserStories from "./FeatureUserStories";

export type FeaturesListProps = {
    features: FeatureSummary[];
}

const FeaturesList = ({ features }: FeaturesListProps) => {
    return (
        <div className="px-16">
            {features.map(feature => (
                <div key={feature.id} className="grid grid-cols-3 items-center py-4 text-2xl">
                    <div className="flex">
                        <Trophy size={32} weight="fill" className="text-purple-700 mr-4" />
                        <p>{feature.title}</p>
                    </div>
                    <FeatureTickets tickets={feature.userStories.flatMap(_ => _.tickets)} />
                    <FeatureUserStories userStories={feature.userStories} />
                </div>
            ))}
        </div>
    )
}

export default FeaturesList;
