import { Guid } from 'guid-typescript';

export class OptOut {
    public id: Guid;
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

    constructor(optout: any) {
        this.id = optout.id ? Guid.parse(optout.id) : Guid.parse(Guid.EMPTY);
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