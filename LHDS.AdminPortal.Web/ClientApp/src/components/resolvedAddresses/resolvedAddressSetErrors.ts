import { ErrorBase } from "../../types/ErrorBase";

export interface ResolvedAddressErrors extends ErrorBase {
    alternateUnstructuredPostalAddress: string;
   
}

export const resolvedAddressErrors: ResolvedAddressErrors = {
    hasErrors: false,
    alternateUnstructuredPostalAddress: "",
}