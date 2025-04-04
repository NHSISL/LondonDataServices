import { ErrorBase } from "../../types/ErrorBase";

export interface SubscriberAgreementErrors extends ErrorBase {
    SupplierSharingAgreementShortName: string;
}

export const subscriberAgreementErrors: SubscriberAgreementErrors = {
    hasErrors: false,
    SupplierSharingAgreementShortName: "",
}