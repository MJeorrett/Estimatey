DELETE FROM TicketTag;
DELETE FROM UserStoryTag;
DELETE FROM FeatureTag;
DELETE FROM BugTag;

DELETE FROM Ticket;
DELETE FROM Bug;
DELETE FROM UserStory;
DELETE FROM Feature;
DELETE FROM LoggedTime;
DELETE FROM FloatPerson;

DELETE FROM Tag;

UPDATE Project
SET LinksContinuationToken = '', WorkItemsContinuationToken = '', LoggedTimeHasBeenSyncedUntil = NULL