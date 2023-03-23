import { Guid } from 'guid-typescript';

export class OptOut {
    public id: Guid;

    public nhsNumber: string;
    public optOutStatus: string;
    public cacheTime: Date;
    public lastSentToMesh: Date;
    public createdDate: Date;
    public createdBy: string;
    public updatedDate: Date;
    public updatedBy: string;

    constructor(optout: any) {
        this.id = optout.id ? Guid.parse(optout.id) : Guid.parse(Guid.EMPTY);
        this.nhsNumber = optout.nhsNumber;
        this.optOutStatus = optout.optOutStatus;
        this.cacheTime = optout.cacheTime ? new Date(optout.cacheTime) : new Date();
        this.lastSentToMesh = optout.lastSentToMesh ? new Date(optout.lastSentToMesh) : new Date();
        this.createdDate = optout.createdDate ? new Date(optout.createdDate) : new Date();
        this.createdBy = optout.createdBy || "";
        this.updatedDate = optout.updatedDate ? new Date(optout.updatedDate) : new Date();
        this.updatedBy = optout.updatedBy || "";
    }
}