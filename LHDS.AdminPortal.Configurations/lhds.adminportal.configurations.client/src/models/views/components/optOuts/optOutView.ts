import { Guid } from 'guid-typescript';

export class OptOutView {
    public id: Guid;
    public nhsNumber: string;
    public status: string;
    public cacheTime: Date | undefined;
    public lastSentToMesh: Date | undefined;
    public uniqueReference?: string;
    public batchReference?: number;
    public createdBy?: string;
    public createdDate?: Date | undefined;
    public updatedBy?: string;
    public updatedDate?: Date | undefined;

    constructor(
        id: Guid,
        nhsNumber: string,
        status: string,
        cacheTime: Date | undefined,
        lastSentToMesh: Date | undefined,
        uniqueReference?: string,
        batchReference?: number,
        createdBy?: string,
        createdDate?: Date | undefined,
        updatedBy?: string,
        updatedDate?: Date | undefined) 
    {
        this.id = id;
        this.nhsNumber = nhsNumber || "";
        this.status = status || "";
        this.uniqueReference = uniqueReference || "";
        this.batchReference = batchReference
        this.cacheTime = cacheTime;
        this.lastSentToMesh = lastSentToMesh;
        this.createdBy = createdBy || "";
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy || "";
        this.updatedDate = updatedDate;
    }
}