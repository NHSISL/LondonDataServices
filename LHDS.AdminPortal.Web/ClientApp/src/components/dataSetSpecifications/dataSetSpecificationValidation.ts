import { Validation } from "../../models/validations/validation";

export const dataSetSpecificationValidation: Array<Validation> = [{
    property: "supplierSpecificationVersion",
    friendlyName: "supplierSpecificationVersion",
    isRequired: true,
    minLength: 3,
    maxLength: 10
}, {
    property: "ourSpecificationVersion",
    friendlyName: "ourSpecificationVersion",
    isRequired: true,
    minLength: 3,
    maxLength: 10
}, {
    property: "notes",
    friendlyName: "notes",
    minLength: 3,
    maxLength: 4000
}, {
    property: "isMultiAuthorPerBatch",
    friendlyName: "isMultiAuthorPerBatch",
    isRequired: true
}, {
    property: "isPublished",
    friendlyName: "isPublished",
    isRequired: true
},
{
    property: "isActive",
    friendlyName: "Is Active",
    isRequired: true
}]