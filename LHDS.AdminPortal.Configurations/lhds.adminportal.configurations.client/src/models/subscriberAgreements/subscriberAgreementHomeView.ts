export class SubscriberAgreementHomeView {
    public id: string;
    public supplierSharingAgreementShortName: string;;
    public createdDate?: Date;
    public createdBy?: string;
    public updatedDate?: Date;
    public updatedBy?: string;

    constructor(
        id: string,
        supplierSharingAgreementShortName: string,
        createdDate?: Date,
        createdBy?: string,
        updatedDate?: Date,
        updatedBy?: string,
    ) {
        this.id = id;
        this.supplierSharingAgreementShortName = supplierSharingAgreementShortName;
        this.createdDate = createdDate;
        this.createdBy = createdBy;
        this.updatedDate = updatedDate;
        this.updatedBy = updatedBy;
    }
}