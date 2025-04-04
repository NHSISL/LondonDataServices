import { Validation } from "../../models/validations/validation";

export const dataTypeValidations: Array<Validation> = [
    {
        property: "name",
        friendlyName: "name",
        isRequired: true,
        minLength: 3,
        maxLength: 255,
    }
]