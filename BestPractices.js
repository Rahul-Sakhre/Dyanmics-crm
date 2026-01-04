if (typeof (nhsForceMajeureApplication) === "undefined") { nhsForceMajeureApplication = { __namespace: true }; }

if (typeof (nhsForceMajeureApplication.GlobalVariables) === "undefined") { nhsForceMajeureApplication.GlobalVariables = { __namespace: true }; }

if (typeof (nhsForceMajeureApplication.FormFunctions) === "undefined") { nhsForceMajeureApplication.FormFunctions = { __namespace: true }; }

if (typeof (nhsForceMajeureApplication.FormFunctions.Load) === "undefined") { nhsForceMajeureApplication.FormFunctions.Load = { __namespace: true }; }

if (typeof (nhsForceMajeureApplication.FormFunctions.OnChange) === "undefined") { nhsForceMajeureApplication.FormFunctions.OnChange = { __namespace: true }; }

if (typeof (nhsForceMajeureApplication.FormFunctions.Save) === "undefined") { nhsForceMajeureApplication.FormFunctions.Save = { __namespace: true }; }

if (typeof (nhsForceMajeureApplication.FormFunctions.PrivateFunctions) === "undefined") { nhsForceMajeureApplication.FormFunctions.PrivateFunctions = { __namespace: true }; }

if (typeof (nhsForceMajeureApplication.FormFunctions.RibbonButtonFunctions) === "undefined") { nhsForceMajeureApplication.FormFunctions.RibbonButtonFunctions = { __namespace: true }; }

nhsForceMajeureApplication.GlobalVariables =
{
    FORCE_MAJEURE_APPLICATION_STATUS: "header_nhs_forcemajeureapplicationstatuscode",
    FORCE_MAJEURE_APPLICATION_STATUS_WITH_NHSBSA: "872730001",
    FORCE_MAJEURE_APPLICATION_STATUS_APPLICATION_RECEIVED: "872730002",
    FORCE_MAJEURE_APPLICATION_STATUS_PEER_PENDING_REVIEW: "872730003",
    FORCE_MAJEURE_APPLICATION_STATUS_TO_REVIEW: "872730004",
    FORCE_MAJEURE_APPLICATION_APPLICATION_CREATED: "872730011",    
    ADDITIONAL_INFORMATION:"nhs_additionalinformation",
    FORCE_MAJEURE_APPLICATION_PRIMARY_USER: "nhs_primaryuserid",
};

nhsForceMajeureApplication.FormFunctions.Load =
{
    ShowHideFormColumns: function (executionContext) {
        "use strict";
        var formContext = executionContext.getFormContext();
        var fields = [nhsForceMajeureApplication.GlobalVariables.ADDITIONAL_INFORMATION];
        fields.forEach(function (field) {
            var fieldControl = formContext.getControl(field);
            if (fieldControl !== null && fieldControl !== undefined) {
                var fieldAttribute = fieldControl.getAttribute();
                var fieldValue = fieldAttribute.getValue();
                if (fieldValue !== null && fieldValue !== undefined) {
                    fieldControl.setVisible(true);
                }
                else {
                    fieldControl.setVisible(false);
                }
            }
        });
    }
};

nhsForceMajeureApplication.FormFunctions.OnChange =
{
};

nhsForceMajeureApplication.FormFunctions.Save =
{
};

nhsForceMajeureApplication.FormFunctions.PrivateFunctions =
{
};

nhsForceMajeureApplication.FormFunctions.PrivateFunctions =
{
    OpenExistingRecord: function (entityGuid, entityName) {
        "use strict";
        Xrm.Utility.showProgressIndicator("Opening..");
        var pageInput = {
            pageType: "entityrecord",
            entityName: entityName,
            entityId: entityGuid
        };

        var navigationOptions = {
            target: 2,
            width: { value: 80, unit: "%" },
            height: { value: 80, unit: "%" }
        };

        Xrm.Navigation.navigateTo(pageInput, navigationOptions).then(

            function sucess() {
                Xrm.Utility.closeProgressIndicator();
            },
            function error() {
                Xrm.Utility.closeProgressIndicator();
            }
        );
    },
    OpenCustomPage: function (entityGuid, entityName, customPage, customPageTitle, formContext, pageWidth, pageHeight) {

        "use strict";

        var pageInput = {
            pageType: "custom",
            name: customPage,
            entityName: entityName,
            recordId: entityGuid,
            title: customPageTitle
        };

        var navigationOptions = {
            target: 2,
            position: 1,
            width: { value: pageWidth, unit: "%" },
            height: { value: pageHeight, unit: "%" }
        };

        Xrm.Navigation.navigateTo(pageInput, navigationOptions).then(
            function sucess() {
                formContext.data.refresh(true);
            },
            function error() {
            }
        );
    },
    ShowNotification: function (title, message) {

        "use strict";

        var alertStrings = { confirmButtonLabel: "OK", text: message, title: title };
        var alertOptions = { height: 200, width: 350 };
        Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
            function (success) {
                console.log("Alert dialog closed");
            },
            function (error) {
                console.log(error.message);
            }
        );
    },
};


nhsForceMajeureApplication.FormFunctions.RibbonButtonFunctions =
{
    SendToPeerReviewButtonVisibility: function (primaryControl) {
        "use strict";
        var formContext = primaryControl;
        var forceMajeureApplicationStatusControl = formContext.getControl(nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS);
        if (forceMajeureApplicationStatusControl !== null && forceMajeureApplicationStatusControl !== undefined) {
            var forceMajeureApplicationStatusValue = forceMajeureApplicationStatusControl.getAttribute().getValue();
            if (forceMajeureApplicationStatusValue != null && forceMajeureApplicationStatusValue != undefined && forceMajeureApplicationStatusValue == nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS_WITH_NHSBSA) {
                return true;
            }
            else {
                return false;
            }
        } else {
            return false;
        }
    },
    SendToPeerReviewButtonAction: function (primaryControl) {
        "use strict";
        var formContext = primaryControl;
        var confirmStrings = {
            text: "Are you sure you want to send the force majeure application for peer review?",
            title: "Confirm",
            confirmButtonLabel: "Yes",
            cancelButtonLabel: "No"
        };
        Xrm.Navigation.openConfirmDialog(confirmStrings, null).then(
            function (success) {
                if (success.confirmed) {
                    var forceMajeureApplicationStatusControl = formContext.getControl(nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS);
                    if (forceMajeureApplicationStatusControl !== null && forceMajeureApplicationStatusControl !== undefined) {
                        forceMajeureApplicationStatusControl.getAttribute().setValue(parseInt(nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS_PEER_PENDING_REVIEW));
                        formContext.data.refresh(true);
                    } else {
                        return false;
                    }
                }
            }
        );
    },
    AskForAdditionalInfoButtonVisibility: function (primaryControl) {
        "use strict";
        var formContext = primaryControl;
        var forceMajeureApplicationStatusControl = formContext.getControl(nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS);
        if (forceMajeureApplicationStatusControl !== null && forceMajeureApplicationStatusControl !== undefined) {
            var forceMajeureApplicationStatusValue = forceMajeureApplicationStatusControl.getAttribute().getValue();
            if (forceMajeureApplicationStatusValue != null && forceMajeureApplicationStatusValue != undefined && forceMajeureApplicationStatusValue == nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS_WITH_NHSBSA ||
                forceMajeureApplicationStatusValue != null && forceMajeureApplicationStatusValue != undefined && forceMajeureApplicationStatusValue == nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS_PEER_PENDING_REVIEW
            ) {
                return true;
            }
            else {
                return false;
            }
        } else {
            return false;
        }
    },
    AskForAdditionalInfoButtonAction: function (primaryControl) {
        "use strict";
        var formContext = primaryControl;
        var entityGuid = formContext.data.entity.getId().replace(/[{}]/g, "");
        var confirmStrings = {
            text: "Are you sure you want to ask additional information?",
            title: "Confirm",
            confirmButtonLabel: "Yes",
            cancelButtonLabel: "No"
        };
        Xrm.Navigation.openConfirmDialog(confirmStrings, null).then(
            function (success) {
                if (success.confirmed) {
                    nhsForceMajeureApplication.FormFunctions.PrivateFunctions.OpenCustomPage(entityGuid, "nhs_forcemajeureapplication", "nhs_forcemajeureapplicationadditionalinformation_78eb2", "", formContext, 30, 43);
                }
            }
        );
    },
    SubmitButtonVisibility: function (primaryControl) {
        "use strict";
        var formContext = primaryControl;

        var primaryUserControl = formContext.getControl(nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_PRIMARY_USER);
        if (primaryUserControl !== null && primaryUserControl !== undefined) {            
            var primaryUserValue = primaryUserControl.getAttribute().getValue();           
            if (primaryUserValue && primaryUserValue.length > 0) {
                var primaryUserId = primaryUserValue[0].id.replace("{", "").replace("}", "");
                var loggedInUserId = Xrm.Utility.getGlobalContext().userSettings.userId;  
                loggedInUserId = loggedInUserId.replace("{", "").replace("}", "");
                if (primaryUserValue != null && primaryUserValue != undefined && primaryUserId == loggedInUserId) {
                    return false;
                }            
            } else {
                return false;
            }
        } else{
            return false;
        }

        var forceMajeureApplicationStatusControl = formContext.getControl(nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS);
        if (forceMajeureApplicationStatusControl !== null && forceMajeureApplicationStatusControl !== undefined) {
            var forceMajeureApplicationStatusValue = forceMajeureApplicationStatusControl.getAttribute().getValue();
            if (forceMajeureApplicationStatusValue != null && forceMajeureApplicationStatusValue != undefined && forceMajeureApplicationStatusValue == nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS_PEER_PENDING_REVIEW) {
                return true;
            }
            else {
                return false;
            }
        } else {
            return false;
        }
    },
    SubmitButtonAction: function (primaryControl) {
        "use strict";
        var formContext = primaryControl;
        var confirmStrings = {
            text: "Are you sure you want to submit the force majeure application to commissioner for review?",
            title: "Confirm",
            confirmButtonLabel: "Yes",
            cancelButtonLabel: "No"
        };
        Xrm.Navigation.openConfirmDialog(confirmStrings, null).then(
            function (success) {
                if (success.confirmed) {
                    var forceMajeureApplicationStatusControl = formContext.getControl(nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS);
                    if (forceMajeureApplicationStatusControl !== null && forceMajeureApplicationStatusControl !== undefined) {
                        forceMajeureApplicationStatusControl.getAttribute().setValue(parseInt(nhsForceMajeureApplication.GlobalVariables.FORCE_MAJEURE_APPLICATION_STATUS_TO_REVIEW));
                        formContext.data.refresh(true);
                    } else {
                        return false;
                    }
                }
            }
        );
    },
}

