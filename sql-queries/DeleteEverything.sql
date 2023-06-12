DELETE FROM TicketTag;
DELETE FROM UserStoryTag;
DELETE FROM FeatureTag;

DELETE FROM Ticket;
DELETE FROM UserStory;
DELETE FROM Feature;

DELETE FROM Tag;

UPDATE Project
SET LinksContinuationToken = '', WorkItemsContinuationToken = ''