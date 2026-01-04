// When we dont have guid we use muliple 

var query = new QueryExpression("new_student")
{
    ColumnSet = new ColumnSet("new_studentid","new_name"),
    Criteria = new FilterExpression
    {
        Conditions =
        {
            new ConditionExpression("new_pluginsopr", ConditionOperator.Equal, parentRef.Id),
            new ConditionExpression("new_standard", ConditionOperator.Equal, newStandard)
        }
    }
};

tracingService.Trace("Executing RetrieveMultiple query...");
var existingStudents = service.RetrieveMultiple(query);
tracingService.Trace($"Number of matching students found: {existingStudents.Entities.Count}");

if (existingStudents.Entities.Count > 0)
{
    tracingService.Trace("Duplicate standard found. Throwing exception.");
    throw new InvalidPluginExecutionException("Student of same standard already exists.");
}

// We use single when we have guid

Entity forceMajeureNotificationData = GetForceMajeureNotification.GetForceMajeureNotificationById(service, tracingService, recordId);
public static Entity GetForceMajeureNotificationById(IOrganizationService service,ITracingService tracingService,Guid forcemajeurenotificationId)
 {
     var columns = new ColumnSet(
         ForceMajeureNotificationTable.Contract,
         ForceMajeureNotificationTable.Name,
         ForceMajeureNotificationTable.EventStartDate,
         ForceMajeureNotificationTable.EventEndDate,
         ForceMajeureNotificationTable.EventOngoing,
         ForceMajeureNotificationTable.Description,
         ForceMajeureNotificationTable.LostUDAs,
         ForceMajeureNotificationTable.LostUOAs,
         ForceMajeureNotificationTable.MitigateAction
     );
     try
     {
         var record = service.Retrieve(ForceMajeureNotificationTable.EntityName, forcemajeurenotificationId, columns);                
         if (record == null) { throw new InvalidPluginExecutionException("Force majeure notification data not found."); }
         return record;
     }
     catch (Exception ex)
     {
         throw new InvalidPluginExecutionException("Force majeure notification data not found.", ex);
     }
}

//Boiler Code
 public void Execute(IServiceProvider serviceProvider)
 {
     ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
     tracingService.Trace("CreateTask plugin execution started.");

     IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

     IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
     IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
    
    throw new NotImplementedException();
 }

// run plugin on create and update
if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity entity &&
   (context.MessageName.ToLower() == "create" || context.MessageName.ToLower() == "update"))

// How to set optionset
try
        {
            var update = new Entity(target.LogicalName) { Id = target.Id };

            // Set to Inactive state + a valid inactive status
            update["statecode"] = new OptionSetValue(1);    // Inactive
            update["statuscode"] = new OptionSetValue(2);   // e.g., 'Inactive' (verify actual value on your table)

            service.Update(update);
            trace.Trace($"Deactivated {target.LogicalName} {target.Id}");
        }

//Using single retrieve to set name
 if (entity.Contains(ServiceLineTable.Service) && entity[ServiceLineTable.Service] != null)
                {
                    
                    EntityReference serviceRef = (EntityReference)entity[ServiceLineTable.Service];

                    Entity serviceEntity = service.Retrieve(serviceRef.LogicalName, serviceRef.Id, new ColumnSet(ServiceTable.Name));


                    if (serviceEntity != null && serviceEntity.Contains(ServiceTable.Name))
                    {
                        string serviceName = serviceEntity.GetAttributeValue<string>(ServiceTable.Name);

                        entity["nhs_name"] = serviceName;
                    }
                }



