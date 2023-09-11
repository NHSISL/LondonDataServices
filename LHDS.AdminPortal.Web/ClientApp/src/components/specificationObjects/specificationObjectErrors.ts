import { ErrorBase } from "../../types/ErrorBase";

export interface ISpecificationObjectErrors extends ErrorBase {
    hasErrors: boolean;
    name: string;
}

export const objectColumnErrors: ISpecificationObjectErrors = {
    hasErrors: false,
    name: ""
};
