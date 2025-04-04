export class OptOut {
    public id: string;
    public nhsNumber: string;
    public status: string;
    public cacheTime: Date | undefined;
    public lastSentToMesh: Date | undefined;
    public uniqueReference?: string;
    public batchReference?: number;
    public createdDate?: Date | undefined;
    public createdBy?: string;
    public updatedDate?: Date | undefined;
    public updatedBy?: string;

    constructor(optout: OptOut) {
        this.id = optout.id ? optout.id : "";
        this.nhsNumber = optout.nhsNumber;
        this.status = optout.status || "";
        this.uniqueReference = optout.uniqueReference || "";
        this.batchReference = optout.batchReference;
        this.cacheTime = optout.cacheTime ? new Date(optout.cacheTime) : undefined;
        this.lastSentToMesh = optout.lastSentToMesh ? new Date(optout.lastSentToMesh) : undefined;
        this.createdDate = optout.createdDate ? new Date(optout.createdDate) : undefined;
        this.createdBy = optout.createdBy || "";
        this.updatedDate = optout.updatedDate ? new Date(optout.updatedDate) : undefined;
        this.updatedBy = optout.updatedBy || "";
    }
}