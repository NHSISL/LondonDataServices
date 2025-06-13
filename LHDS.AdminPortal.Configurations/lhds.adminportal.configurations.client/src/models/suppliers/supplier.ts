export class Supplier {
    public id: string;
    public name?: string;
    public friendlyName?: string;
    public description?: string;
    public isIngestionTracked?: boolean;
    public canDecryptIngestionTracking?: boolean;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(supplier: Supplier) {
        this.id = supplier.id ? supplier.id : "";
        this.name = supplier.name;
        this.friendlyName = supplier.friendlyName;
        this.description = supplier.description;
        this.isIngestionTracked = supplier.isIngestionTracked;
        this.canDecryptIngestionTracking = supplier.canDecryptIngestionTracking === true ? true : false;
        this.createdBy = supplier.createdBy;
        this.createdDate = new Date(supplier.createdDate);
        this.updatedBy = supplier.updatedBy;
        this.updatedDate = new Date(supplier.updatedDate);
    }
}