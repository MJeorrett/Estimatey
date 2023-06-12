import { UserStory } from "./userStory";

export type FeatureSummary = {
    id: number;
    title: string;
    status: string;
    userStories: UserStory[];
}