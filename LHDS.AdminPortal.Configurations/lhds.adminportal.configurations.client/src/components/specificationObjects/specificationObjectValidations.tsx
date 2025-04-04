import { Validation } from "../../models/validations/validation";

export const specificationObjectValidations: Array<Validation> = [
    {
        property: "supplierObjectName",
        friendlyName: "Supplier Object Name",
        isRequired: true,
        minLength: 3,
        maxLength: 255,
    },
    {
        property: "ourObjectName",
        friendlyName: "Our Object Name",
        isRequired: true,
        minLength: 3,
        maxLength: 255,
    },
    {
        property: "isPushedToUs",
        friendlyName: "isPushedToUs",
        isRequired: true,
    },
    {
        property: "isPulledByUs",
        friendlyName: "isPulledByUs",
        isRequired: true,
    },
    {
        property: "isSubmissionHeaderObject",
        friendlyName: "isSubmissionHeaderObject",
        isRequired: true,
    },
    {
        property: "isTransactionLog",
        friendlyName: "isTransactionLog",
        isRequired: true,
    },
]
