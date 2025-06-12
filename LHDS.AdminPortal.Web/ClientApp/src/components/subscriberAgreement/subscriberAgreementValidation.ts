import { Validation } from "../../models/validations/validation";

export const subscriberAgreementValidation: Array<Validation> = [{
    property: "supplierSharingAgreementShortName",
    friendlyName: "Name",
    isRequired: true,
    minLength: 3,
    maxLength: 150
},
{
    property: "ftpUserName",
    friendlyName: "Ftp User Name",
    isRequired: true,
    minLength: 3,
    maxLength: 150
}
]