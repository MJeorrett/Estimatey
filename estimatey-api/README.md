# Estimatey Api

### Pre-requisites
You will need [Azure DevOps](https://azure.microsoft.com/en-gb/products/devops) and [Float](https://www.float.com/time-tracking/) accounts that you have admin access to.

## Getting started
First you need to create an app registration in Azure and add make it a project reader in your DevOps project.

Then you need to configure your user secrets in the WebApi project as follows:
```JSON
{
  "DevOpsOptions:OrganizationName": "<your devops orgnaization name>",
  "DevOpsOptions:AzureAadTenantId": "<your Azure tenant id>",
  "DevOpsOptions:ClientId": "<client id of the application registration>",
  "DevOpsOptions:ClientSecret": "<secret created against the application registration>"
  "FloatOptions:ApiKey": "<Api key from your float account>"
}
```

Then you need to migrate the database by running the following command in the estimatey-api folder:
```
dotnet ef database update --project .\Estimatey.Infrastructure\ --startup-project .\Estimatey.WebApi\
```

Now you can set the startup project to the WebApi and start debugging.