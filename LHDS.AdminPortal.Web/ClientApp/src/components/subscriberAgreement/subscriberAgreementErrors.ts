import { ErrorBase } from "../../types/ErrorBase";

export interface SubscriberAgreementErrors extends ErrorBase {
    SupplierSharingAgreementShortName: string;
    ftpUserName: string;
    ftpPublicKey: string;
    gpgPublicKey: string;
}

export const subscriberAgreementErrors: SubscriberAgreementErrors = {
    hasErrors: false,
    SupplierSharingAgreementShortName: "",
    ftpUserName: "",
    ftpPublicKey: "",
    gpgPublicKey: "",
}