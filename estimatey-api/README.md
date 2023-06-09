# Estimatey Api

## Getting started
First you need to create an app registration in Azure and add make it a project reader in your DevOps project.

Then you need to configure your user secrets in the WebApi project as follows:
```JSON
{
  "DevOpsOptions:OrganizationName": "<your devops orgnaization name>",
  "DevOpsOptions:AzureAadTenantId": "<your Azure tenant id>",
  "DevOpsOptions:ClientId": "<client id of the application registration>",
  "DevOpsOptions:ClientSecret": "<secret created against the application registration>"
}
```

Then you need to migrate the database by running the following command in the estimatey-api folder:
```
dotnet ef database update --project .\Estimatey.Infrastructure\ --startup-project .\Estimatey.WebApi\
```

Now you can set the startup project to the WebApi and start debugging.