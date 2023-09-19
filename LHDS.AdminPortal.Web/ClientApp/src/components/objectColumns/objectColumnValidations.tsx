import { Validation } from "../../models/validations/validation";

export const objectColumnValidations: Array<Validation> = [
    {
        property: "supplierColumnName",
        friendlyName: "supplierColumnName",
        isRequired: true,
        minLength: 3,
        maxLength: 255,
    }
    //},
    //{
    //    property: "ourColumnName",
    //    friendlyName: "ourColumnName",
    //    isRequired: true,
    //    minLength: 3,
    //    maxLength: 255,
    //},
    //{
    //    property: "sqlDataType",
    //    friendlyName: "sqlDataType",
    //    isRequired: true,
    //    minLength: 3,
    //    maxLength: 50,
    //},
    //{
    //    property: "sqlDataType",
    //    friendlyName: "sqlDataType",
    //    isRequired: true,
    //    minLength: 3,
    //    maxLength: 50,
    //},
    //{
    //    property: "fhirDataType",
    //    friendlyName: "fhirDataType",
    //    isRequired: true,
    //    minLength: 3,
    //    maxLength: 255,
    //},
    //{
    //    property: "supplierDateFormat",
    //    friendlyName: "supplierDateFormat",
    //    isRequired: true,
    //    minLength: 3,
    //    maxLength: 255,
    //},
    //{
    //    property: "isWatermark",
    //    friendlyName: "isWatermark",
    //    isRequired: true,
    //},
    //{
    //    property: "isSequencing",
    //    friendlyName: "isSequencing",
    //    isRequired: true,
    //},
    //{
    //    property: "isBusinessKey",
    //    friendlyName: "isBusinessKey",
    //    isRequired: true,
    //},
    //{
    //    property: "isUniqueRecordKey",
    //    friendlyName: "isUniqueRecordKey",
    //    isRequired: true,
    //},
    //{
    //    property: "isVersionHashElement",
    //    friendlyName: "isVersionHashElement",
    //    isRequired: true,
    //},
    //{
    //    property: "isSenderCode",
    //    friendlyName: "isSenderCode",
    //    isRequired: true,
    //},
    //{
    //    property: "isAuthorCode",
    //    friendlyName: "isAuthorCode",
    //    isRequired: true,
    //},
    //{
    //    property: "isDeleteFlag",
    //    friendlyName: "isDeleteFlag",
    //    isRequired: true,
    //},
    //{
    //    property: "isPersonConfidentialData",
    //    friendlyName: "isPersonConfidentialData",
    //    isRequired: true,
    //},
    //{
    //    property: "PersonConfidentialDataType",
    //    friendlyName: "PersonConfidentialDataType",
    //    isRequired: true,
    //    minLength: 3,
    //    maxLength: 255,
    //},
    //{
    //    property: "isSensitiveRecordMarker",
    //    friendlyName: "isSensitiveRecordMarker",
    //    isRequired: true,
    //},
    //{
    //    property: "codeSystem",
    //    friendlyName: "codeSystem",
    //    isRequired: true,
    //    minLength: 3,
    //    maxLength: 255,
    //},
]
