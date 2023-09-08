import { ErrorBase } from "../../types/ErrorBase";

export interface DataSetErrors extends ErrorBase {
    dataSetName: string;
    dataSetAliases: string;
    dataSetSupplier: string;
    dataSetAuthor: string;
    dataSourceType: string;
    isActive: string;
    activeFrom: string;
    activeTo: string;
    specifiedBy: string;
    IsNationallySpecified: string;
    collectedBy: string;
    isNationallyCollected: string
}

export const dataSetErrors: DataSetErrors = {
    hasErrors: false,
    dataSetName: "",
    dataSetAliases: "",
    dataSetSupplier: "",
    dataSetAuthor: "",
    dataSourceType: "",
    isActive: "",
    activeFrom: "",
    activeTo: "",
    specifiedBy: "",
    IsNationallySpecified: "",
    collectedBy: "",
    isNationallyCollected: ""
}