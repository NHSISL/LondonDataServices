import { Validation } from "../../models/validations/validation";

export const supplierValidations: Array<Validation> = [
    {
        property: "name",
        friendlyName: "name",
        isRequired: true,
        minLength: 3,
        maxLength: 255,
    },
    {
        property: "friendlyName",
        friendlyName: "friendlyName",
        isRequired: true,
        minLength: 3,
        maxLength: 255,
    },
    {
        property: "description",
        friendlyName: "description",
        isRequired: true,
        minLength: 3,
        maxLength: 255,
    },
    {
        property: "landingManualTriggerUrl",
        friendlyName: "landingManualTriggerUrl",
        isRequired: true,
        minLength: 3,
        maxLength: 255,
    },
    {
        property: "decryptionManualTriggerUrl",
        friendlyName: "decryptionManualTriggerUrl",
        isRequired: true,
        minLength: 3,
        maxLength: 255,
    }
]