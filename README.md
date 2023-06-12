# ![Estimatey parrot](./readme-images/estimatey-icon.png) Estimatey
### Takes the "Arrrrrrgh" out of software project planning and estimation.

Estimatey hooks into the tools that you already use** and provides insights that you need to help with project planning and estimation.

** provided that they are [Azure DevOps](https://azure.microsoft.com/en-gb/products/devops) and [Float](https://www.float.com/time-tracking/) :stuck_out_tongue_closed_eyes:

## Limitations
- When work items are deleted in DevOps this is not reflected in the Estimatey work item cache.
This is because the work item revisions (annoyingly) doesn't tell us when work items are deleted.
It does tell use when work items are restored from the recycle bin though!!
- It keeps the same token for ever so will probably fall over after about 1 hour at the moment...

## Road Map
- :construction: Project ticket overview page.
![Project ticket overview page](./readme-images/project-ticket-overview-page.png)
- Last sync / next sync indicator
- See if base workItem table would make things easier.
- See if we can remove DevOpsId and insert id from DevOps into the Id column.
- Write service to sync time sheets from Float.
- Basic predictions page
![Basic predictions page](./readme-images/basic-predictions-page.png)
- Normalize work item statuses - take whatever DevOps has and convert to "New", "In progress", "Complete". Configurable mappings.

## Completed features
- :white_check_mark: Write service to sync work items from DevOps including features, user stories, tasks and their tags.
- :white_check_mark: Write service to sync work item relationships from DevOps.
- :white_check_mark: Make deleting more robust by storing last revised date and then only marking deleted if deleted date is after last revised date. NOTE: Turns out I don't think we need this as delete runs after update.  Lets see how we go and tweak if necessary.

## Future Scope
- Warnings when invalid work item states are found e.g. Tasks or User Stories without a parent
or a User Story that is completed when all it's child tasks are not completed.
- Track date set to in progress and date completed to allow guestimating time to complete work items.