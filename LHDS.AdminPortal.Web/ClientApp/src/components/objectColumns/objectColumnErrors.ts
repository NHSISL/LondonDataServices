import { ErrorBase } from "../../types/ErrorBase";

export interface IObjectColumnErrors extends ErrorBase {
    hasErrors: boolean;
    name: string;
}

export const objectColumnErrors: IObjectColumnErrors = {
    hasErrors: false,
    name: ""
};
