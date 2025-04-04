import { ErrorBase } from "../../types/ErrorBase";

export interface IDataTypeErrors extends ErrorBase {
    hasErrors: boolean;
    name: string;
}

export const dataTypeErrors: IDataTypeErrors = {
    hasErrors: false,
    name: ""
};
