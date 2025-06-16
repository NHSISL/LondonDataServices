import { Validation } from "../../models/validations/validation";

export const resolvedAddressValidation: Array<Validation> = [{
    property: "organisationName",
    friendlyName: "Organisation Name",
    isRequired: true,
    minLength: 3,
    maxLength: 150
}]