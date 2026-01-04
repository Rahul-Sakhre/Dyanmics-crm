
"When A new record is added in new_pluggins entity then create a record in task entity as well regarding the plugin record"
 

using Microsoft.Xrm.Sdk;
using System;

namespace CreateTaskOnPlugin
{
    public class CreateTask : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));  ("Plugin ka Debugger")
            tracingService.Trace("CreateTask plugin execution started.");  ("Debugging points set krte h wesa hi same")

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext)); ("Plugin register krte time context set krte h wo h ye")
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory)); ("iski help se CRM m CRUD perform hota h jaise webApi use krte h")
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            // Only run on Create
            if (context.MessageName != "Create")
            {
                tracingService.Trace("Plugin exited: not a Create message.");
                return;
            }

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity entity)
            {
                tracingService.Trace($"Entity Logical Name: {entity.LogicalName}");

                if (entity.LogicalName != "new_plugins")
                {
                    tracingService.Trace("Plugin exited: not the target entity.");
                    return;
                }

                try
                {
                    string pluginName = entity.Contains("new_name") ? entity["new_name"].ToString() : "Unnamed Plugin";

                    Entity task = new Entity("task");
                    task["subject"] = $"Regarding the {pluginName}";
                    task["description"] = $"This task created to take the follow up for {pluginName}";
                    task["new_forplugin"] = new EntityReference("new_plugins", entity.Id);

                    service.Create(task);
                    tracingService.Trace("Task created successfully.");
                }
                catch (Exception ex)
                {
                    tracingService.Trace($"Exception: {ex.Message}");
                    throw new InvalidPluginExecutionException("An error occurred in CreateTask plugin.", ex);
                }
            }

            tracingService.Trace("CreateTask plugin execution completed.");
        }
    }
}