import { ClipboardText } from "@phosphor-icons/react";

import { UserStory } from "../../api/models/userStory";

export type FeatureUserStoriesProps = {
    userStories: UserStory[];
}

const FeatureUserStories = ({ userStories }: FeatureUserStoriesProps) => {
    return (
        <div className="flex items-center">
            <p className="text-right pl-10 mr-4">{userStories.length} User Stor{userStories.length === 1 ? "y" : "ies"}</p>
            <ClipboardText
                size={32} weight="fill"
                className="text-blue-500 ml-auto" />
        </div>
    )
}

export default FeatureUserStories;
