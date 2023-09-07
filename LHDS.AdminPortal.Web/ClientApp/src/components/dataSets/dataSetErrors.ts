import { ErrorBase } from "../../types/ErrorBase";

export interface DataSetErrors extends ErrorBase {
    dataSetName: string;
    dataSetAliasses: string;
    dataSetSupplier: string;
    dataSetAuthor: string;
    dataSourceType: string;
    isActive: string;
    activeFrom: string;
    activeTo: string;
}

export const dataSetErrors: DataSetErrors = {
    hasErrors: false,
    dataSetName: "",
    dataSetAliasses: "",
    dataSetSupplier: "",
    dataSetAuthor: "",
    dataSourceType: "",
    isActive: "",
    activeFrom: "",
    activeTo: ""
}