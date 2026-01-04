using Microsoft.Xrm.Sdk;
using NHSBSA.PAD.Helper;
using System;
namespace NHSBSA.PAD.Actions
{
    public class CreateForceMajeureApplication : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService systemService = serviceFactory.CreateOrganizationService(null);
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                if (!context.InputParameters.Contains(ForceMajeureNotificationTable.RecordId))
                {
                    tracingService.Trace("Force majeure notification does not exit");
                    throw new InvalidPluginExecutionException("Input value missing.");
                }

                EntityCollection categorizationResults = GetCategorizationConfiguration.GetActiveCategorizationConfigurationYear(service, tracingService);
                if (categorizationResults.Entities.Count == 0)
                {
                    throw new InvalidPluginExecutionException("No active categorisation record found.");
                }

                Entity activeCategorizationYear = categorizationResults.Entities[0];
                string activeCategorizationConfigurationYear = (activeCategorizationYear.GetAttributeValue<int>(CategorizationConfigurationTable.CategorizationYear)).ToString();

                EntityCollection forceMajeureResults = GetForceMajeureCategorization.GetActiveForceMajeureConfiguration(service, tracingService, activeCategorizationConfigurationYear);
                if (forceMajeureResults.Entities.Count == 0)
                {
                    throw new InvalidPluginExecutionException("No force majeure configuration record found.");
                }

                Entity activeForceMajeureConfiguration = forceMajeureResults.Entities[0];


                Guid recordId = new Guid(context.InputParameters[ForceMajeureNotificationTable.RecordId].ToString());
                Entity forceMajeureNotificationData = GetForceMajeureNotification.GetForceMajeureNotificationById(service, tracingService, recordId);

                if (forceMajeureNotificationData != null)
                {
                    Entity forceMajeureApplication = new Entity(ForceMajeureApplicationTable.EntityName);
                    EntityReference contractRef = forceMajeureNotificationData.GetAttributeValue<EntityReference>(ForceMajeureNotificationTable.Contract) ?? throw new InvalidPluginExecutionException("Contract requires the submission of force majeure application.");
                    forceMajeureApplication[ForceMajeureApplicationTable.Contract] = contractRef;
                    forceMajeureApplication[ForceMajeureApplicationTable.EventStartDate] = forceMajeureNotificationData.GetAttributeValue<DateTime?>(ForceMajeureNotificationTable.EventStartDate);
                    forceMajeureApplication[ForceMajeureApplicationTable.EventEndDate] = forceMajeureNotificationData.GetAttributeValue<DateTime?>(ForceMajeureNotificationTable.EventEndDate);
                    forceMajeureApplication[ForceMajeureApplicationTable.EventOngoing] = forceMajeureNotificationData.GetAttributeValue<bool?>(ForceMajeureNotificationTable.EventOngoing);
                    forceMajeureApplication[ForceMajeureApplicationTable.Description] = forceMajeureNotificationData.GetAttributeValue<string>(ForceMajeureNotificationTable.Description);
                    forceMajeureApplication[ForceMajeureApplicationTable.LostUDAs] = forceMajeureNotificationData.GetAttributeValue<decimal?>(ForceMajeureNotificationTable.LostUDAs);
                    forceMajeureApplication[ForceMajeureApplicationTable.LostUOAs] = forceMajeureNotificationData.GetAttributeValue<decimal?>(ForceMajeureNotificationTable.LostUOAs);
                    forceMajeureApplication[ForceMajeureApplicationTable.MitigateAction] = forceMajeureNotificationData.GetAttributeValue<string>(ForceMajeureNotificationTable.MitigateAction);
                    forceMajeureApplication[ForceMajeureApplicationTable.Name] = forceMajeureNotificationData.GetAttributeValue<string>(ForceMajeureNotificationTable.Name);
                    forceMajeureApplication[ForceMajeureApplicationTable.ForceMajeureNotification] = new EntityReference(ForceMajeureNotificationTable.EntityName, recordId);

                    DateTime? activeMajeureYearEndDate = activeForceMajeureConfiguration.GetAttributeValue<DateTime?>(ForceMajeureConfigurationTable.ForceMajeureApplicationEndDate);
                    forceMajeureApplication[ForceMajeureApplicationTable.DueDate] = activeMajeureYearEndDate;

                    Guid forceMajeureApplicationId = service.Create(forceMajeureApplication);
                    if (forceMajeureApplicationId == null)
                    {
                        throw new InvalidPluginExecutionException("Failed to create force majeure application.");
                    }

                    var updateNotification = new Entity(ForceMajeureNotificationTable.EntityName, recordId);
                    updateNotification[ForceMajeureNotificationTable.ForceMajeureApplication] = new EntityReference(ForceMajeureApplicationTable.EntityName, forceMajeureApplicationId);
                    service.Update(updateNotification);
                }
                else
                {
                    throw new InvalidPluginExecutionException("Force majeure notification data not found.");
                }

            }
            catch (Exception ex)
            {
                tracingService.Trace("Error: " + ex.ToString());
                throw new InvalidPluginExecutionException("Error.", ex);
            }
        }
    }
}
