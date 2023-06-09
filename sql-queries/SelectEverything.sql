SELECT * FROM Project
SELECT * FROM Feature
SELECT * FROM UserStory
SELECT * FROM Ticket
SELECT * FROM Tag

SELECT f.Title, tag.[Name] FROM FeatureTag ft
JOIN Tag tag ON tag.TagId = ft.TagId
JOIN Feature f ON f.FeatureId = ft.FeatureId

SELECT us.Title, tag.[Name] FROM UserStoryTag ust
JOIN Tag tag ON tag.TagId = ust.TagId
JOIN UserStory us ON us.UserStoryId = ust.UserStoryId

SELECT t.Title, tag.[Name] FROM TicketTag tt
JOIN Tag tag ON tag.TagId = tt.TagId
JOIN Ticket t ON t.TicketId = tt.TicketId