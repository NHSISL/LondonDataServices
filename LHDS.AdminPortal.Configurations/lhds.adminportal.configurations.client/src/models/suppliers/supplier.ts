
export class Supplier {
    public id: string;
    public name?: string;
    public friendlyName?: string;
    public description?: string;
    public landingManualTriggerUrl?: string;
    public decryptionManualTriggerUrl?: string;
    public canDecryptIngestionTracking?: boolean;
    public canDownloadIngestionTracking?: boolean;
    public canRelandIngestionTracking?: boolean;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(supplier: any) {
        this.id = supplier.id ? supplier.id :"";
        this.name = supplier.name;
        this.friendlyName = supplier.friendlyName;
        this.description = supplier.description;
        this.landingManualTriggerUrl = supplier.landingManualTriggerUrl;
        this.decryptionManualTriggerUrl = supplier.decryptionManualTriggerUrl;
        this.canDecryptIngestionTracking = supplier.canDecryptIngestionTracking === true ? true : false;
        this.canDownloadIngestionTracking = supplier.canDownloadIngestionTracking === true ? true : false;
        this.canRelandIngestionTracking = supplier.canRelandIngestionTracking === true ? true : false;
        this.createdBy = supplier.createdBy;
        this.createdDate = new Date(supplier.createdDate);
        this.updatedBy = supplier.updatedBy;
        this.updatedDate = new Date(supplier.updatedDate);
    }
}