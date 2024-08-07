import { ErrorBase } from "../../types/ErrorBase";

export interface ResolvedAddressErrors extends ErrorBase {
    organisationName: string;
   
}

export const resolvedAddressErrors: ResolvedAddressErrors = {
    hasErrors: false,
    organisationName: "",
}