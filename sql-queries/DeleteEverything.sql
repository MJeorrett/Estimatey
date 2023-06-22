DELETE FROM TicketTag;
DELETE FROM UserStoryTag;
DELETE FROM FeatureTag;

DELETE FROM Ticket;
DELETE FROM UserStory;
DELETE FROM Feature;
DELETE FROM LoggedTime;
DELETE FROM FloatPerson;

DELETE FROM Tag;

UPDATE Project
SET LinksContinuationToken = '', WorkItemsContinuationToken = '', LoggedTimeHasBeenSyncedUntil = NULL