import { ErrorBase } from "../../types/ErrorBase";

export interface ISupplierErrors extends ErrorBase {
    hasErrors: boolean;
    name: string;
    friendlyName: string;
    description: string;
    
}

export const supplierErrors: ISupplierErrors = {
    hasErrors: false,
    name: "",
    friendlyName: "",
    description: "",
    
};
