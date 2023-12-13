import { Validation } from "../../models/validations/validation";

export const supplierValidations: Array<Validation> = [
    {
        property: "name",
        friendlyName: "supplier name",
        isRequired: true,
        minLength: 3,
        maxLength: 255,
    }
]