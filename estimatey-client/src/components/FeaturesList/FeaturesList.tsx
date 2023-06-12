import { FeatureSummary } from "../../api/models/feature"
import FeatureTickets from "./FeatureTickets";

export type FeaturesListProps = {
    features: FeatureSummary[];
}

const FeaturesList = ({ features }: FeaturesListProps) => {
    return (
        <div>
            {features.map(feature => (
                <div key={feature.id} className="grid grid-cols-3 p-4 space-x-6 text-2xl">
                    <p>{feature.title}</p>
                    <FeatureTickets tickets={feature.userStories.flatMap(_ => _.tickets)} />
                    <p>{feature.userStories.length} User Stor{feature.userStories.length === 1 ? "y" : "ies"}</p>
                </div>
            ))}
        </div>
    )
}

export default FeaturesList;
