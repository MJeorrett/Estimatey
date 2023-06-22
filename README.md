# ![Estimatey parrot](./readme-images/estimatey-icon.png) Estimatey
### Takes the "Arrrrrrgh" out of software project planning and estimation.

Estimatey hooks into the tools that you already use** and provides insights that you need to help with project planning and estimation.

** provided that they are [Azure DevOps](https://azure.microsoft.com/en-gb/products/devops) and [Float](https://www.float.com/time-tracking/) :stuck_out_tongue_closed_eyes:

## Limitations
- Azure DevOps syncing service keeps the same token for ever so will probably fall over after about 1 hour at the moment...
- On startup we sync historic logged time from float.  Float has restrictions on the size of date range you can request and so at the moment we can only sync up to 1 year of historic data.
- Assumes that Bugs are managed alongside tickets (not User Stories).

## Road Map
- Last sync / next sync indicator
- See if base workItem table would make things easier.
- See if we can remove DevOpsId and insert id from DevOps into the Id column.
- Basic predictions page
![Basic predictions page](./readme-images/basic-predictions-page.png)
- Normalize work item statuses - take whatever DevOps has and convert to "New", "In progress", "Complete". Configurable mappings.
- Data quality warnings
    - If parent is complete but children aren't.
    - Ticket in progress but no user assigned.
    - Ticket in progress but no branch created.
    - Ticket has been in progress for too long.
    - Ticket or User Story doesn't have a feature.
    - User not specified as Developer, Designer etc.
- PR analytics
    - mean time to complete PRs.
    - Warning when PR open for too long.

## Completed features
- :white_check_mark: Write service to sync logged time from Float.
    - :white_check_mark: Basic implementation fetch all every time.
    - :white_check_mark: Persist historic data - after say a month we can assume that logged time won't change or maybe we can read the `locked` parameter.  We need to do this so we aren't fetching an every growing list of time logs and / or hit the 200 per page limit which means we would have to do paginated fetching. Will also need to support syncing historic data for a newly hooked up project with lots of logged time - this could be tricky...
- :white_check_mark: Write service to sync work items from DevOps including features, user stories, tasks and their tags.
- :white_check_mark: Write service to sync work item relationships from DevOps.
- :white_check_mark: Make deleting more robust by storing last revised date and then only marking deleted if deleted date is after last revised date. NOTE: Turns out I don't think we need this as delete runs after update.  Lets see how we go and tweak if necessary.
- :white_check_mark: Project ticket overview page.
![Project ticket overview page](./readme-images/project-ticket-overview-page.png)

## Future Scope
- Warnings when invalid work item states are found e.g. Tasks or User Stories without a parent
or a User Story that is completed when all it's child tasks are not completed.
- Track date set to in progress and date completed to allow guestimating time to complete work items.