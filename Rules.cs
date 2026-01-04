using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NHSBSA.PAD.Helper;
using System;
namespace NHSBSA.PAD.Plugin
{
    public class ForceMajeureNotificationRule : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService systemService = serviceFactory.CreateOrganizationService(null);
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            try
            {
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity entity &&
                    (context.MessageName.ToLower() == "create" || context.MessageName.ToLower() == "update"))
                {

                    if (context.Depth > 1)
                    {
                        return;
                    }
                    Entity forceMajeureNotificationPreImage = context.PreEntityImages.Contains("PreForceMajeureNotification") ? context.PreEntityImages["PreForceMajeureNotification"] : null;
                    DateTime? eventStartDate = ColumnValidation.GetDynamicDateTimeValue(entity, forceMajeureNotificationPreImage, ForceMajeureNotificationTable.EventStartDate);
                    DateTime? eventEndDate = ColumnValidation.GetDynamicDateTimeValue(entity, forceMajeureNotificationPreImage, ForceMajeureNotificationTable.EventEndDate);
                    bool isOngoing = ColumnValidation.GetDynamicBoolValue(entity, forceMajeureNotificationPreImage, ForceMajeureNotificationTable.EventOngoing);
                    EntityReference contractRef = ColumnValidation.GetDynamicEntityRefrenceValue(entity, forceMajeureNotificationPreImage, ForceMajeureNotificationTable.Contract);
                    if (eventStartDate == null)
                    {
                        throw new InvalidPluginExecutionException("Event start date is a required field");
                    }
                    if (contractRef == null)
                    {
                        throw new InvalidPluginExecutionException("Contract requires the submission of force majeure notification");
                    }
                    if (!eventEndDate.HasValue && !isOngoing)
                    {
                        throw new InvalidPluginExecutionException("Specify an event end date or set event ongoing to yes.");
                    }
                    if (eventEndDate.HasValue && eventStartDate > eventEndDate.Value)
                    {
                        throw new InvalidPluginExecutionException("The event end date must be on or after the event start date");
                    }
                    if (eventEndDate != null && isOngoing == true)
                    {
                        throw new InvalidPluginExecutionException("Either provide event end date or set event ongoing to yes");
                    }
                    Entity contractEntity = service.Retrieve(contractRef.LogicalName, contractRef.Id, new ColumnSet(ContractCategorizationTable.Name));

                    if (contractEntity != null && contractEntity.Contains(ContractCategorizationTable.Name))
                    {
                        string serviceName = contractEntity.GetAttributeValue<string>(ContractCategorizationTable.Name);
                        entity[ForceMajeureNotificationTable.Name] = serviceName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
