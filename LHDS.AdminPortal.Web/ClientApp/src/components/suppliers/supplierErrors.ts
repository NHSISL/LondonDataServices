import { ErrorBase } from "../../types/ErrorBase";

export interface ISupplierErrors extends ErrorBase {
    hasErrors: boolean;
    name: string;
    friendlyName: string;
    description: string;
    isIngestionTracked: string;
    canDecryptIngestionTracking: string;
}

export const supplierErrors: ISupplierErrors = {
    hasErrors: false,
    name: "",
    friendlyName: "",
    description: "",
    isIngestionTracked: "",
    canDecryptIngestionTracking: "",
};
