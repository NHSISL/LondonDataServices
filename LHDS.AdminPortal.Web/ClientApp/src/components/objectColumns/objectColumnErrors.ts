import { ErrorBase } from "../../types/ErrorBase";

export interface IObjectColumnErrors extends ErrorBase {
    hasErrors: boolean;
    supplierColumnName: string;
    ourColumnName: string;
    sqlDataType: string;
    fhirDataType: string;
    isWatermark: string;
    isSequencing: string;
    isBusinessKey: string;
    isVersionHashElement: string;
    isSenderCode: string;
    isAuthorCode: string;
    isDeleteFlag: string;
    isPersonConfidentialDataType: string;
    suplierDateFormat: string;}

export const objectColumnErrors: IObjectColumnErrors = {
    hasErrors: false,
    supplierColumnName: "",
    ourColumnName: "",
    sqlDataType: "",
    fhirDataType: "",
    isWatermark: "",
    isSequencing: "",
    isBusinessKey: "",
    isVersionHashElement: "",
    isSenderCode: "",
    isAuthorCode: "",
    isDeleteFlag: "",
    isPersonConfidentialDataType: "",
    suplierDateFormat: "",
};
