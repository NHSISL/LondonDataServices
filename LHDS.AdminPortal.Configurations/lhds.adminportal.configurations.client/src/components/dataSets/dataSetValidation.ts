import { Validation } from "../../models/validations/validation";

export const dataSetValidation: Array<Validation> = [{
    property: "dataSetName",
    friendlyName: "Name",
    isRequired: true,
    minLength: 3,
    maxLength: 150
}, {
    property: "dataSetAliases",
    friendlyName: "Aliases",
    isRequired: true,
    minLength: 3,
    maxLength: 250
}, {
    property: "dataSetSupplier",
    friendlyName: "Supplier",
    isRequired: true,
    minLength: 3,
    maxLength: 150
}, {
    property: "dataSetAuthor",
    friendlyName: "Author",
    isRequired: true,
    minLength: 3,
    maxLength: 150
}, {
    property: "specifiedBy",
    friendlyName: "Specified By",
    isRequired: true,
    minLength: 3,
    maxLength: 150
}, {
    property: "collectedBy",
    friendlyName: "collectedBy",
    isRequired: true,
    minLength: 3,
    maxLength: 255
}, {
    property: "dataSourceType",
    friendlyName: "Data Source Type",
    isRequired: true,
    minLength: 3,
    maxLength: 150
}, {
    property: "activeFrom",
    friendlyName: "Active From",
    isRequired: true
},
{
    property: "activeTo",
    friendlyName: "Active To",
    isRequired: true
}]