import { ErrorBase } from "../../types/ErrorBase";

export interface ISpecificationObjectErrors extends ErrorBase {
    hasErrors: boolean;
    supplierObjectName: string;
    ourObjectName: string;
    objectDescription: string;
    interchangeProtocol: string;
    isPushedToUs: string;
    isPulledByUs: string;
    deletionHandling: string;
    isSubmissionHeaderObject: string;
    isTransactionLog: string;
}

export const specificationObjectErrors: ISpecificationObjectErrors = {
    hasErrors: false,
    supplierObjectName: "",
    ourObjectName: "",
    objectDescription: "",
    interchangeProtocol: "",
    isPushedToUs: "",
    isPulledByUs: "",
    deletionHandling: "",
    isSubmissionHeaderObject: "",
    isTransactionLog: "",
};
