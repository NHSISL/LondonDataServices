import { ErrorBase } from "../../types/ErrorBase";

export interface DataSetSpecificationErrors extends ErrorBase {
    supplierSpecificationVersion: string;
    ourSpecificationVersion: string;
    notes: string;
    isMultiAuthorPerBatch: string;
    entityChangeSynchronisation: string;
    dateReleased: string;
    dateImplementation: string;
    dateSupersed: string;
    isPublished: string;
    isActive: string;
    activeFrom: string;
    activeTo: string
}

export const dataSetSpecificationErrors: DataSetSpecificationErrors = {
    hasErrors: false,
    supplierSpecificationVersion: "",
    ourSpecificationVersion: "",
    notes: "",
    isMultiAuthorPerBatch: "",
    entityChangeSynchronisation: "",
    dateReleased: "",
    dateImplementation: "",
    dateSupersed: "",
    isPublished: "",
    isActive: "",
    activeFrom: "",
    activeTo: ""
}