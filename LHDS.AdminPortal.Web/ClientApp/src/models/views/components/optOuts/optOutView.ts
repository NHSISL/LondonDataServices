import { Guid } from 'guid-typescript';

export class OptOutView {
    public id: Guid;
    public nhsNumber: string;
    public status: string;
    public uniqueReference: string;
    public batchReference: number;
    public cacheTime: Date;
    public lastSentToMesh: Date;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(
        id: Guid,
        nhsNumber: string,
        status: string,
        uniqueReference: string,
        batchReference: number,
        cacheTime: Date,
        lastSentToMesh: Date,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date) 
    {
        this.id = id;
        this.nhsNumber = nhsNumber || "";
        this.status = status || "";
        this.uniqueReference = uniqueReference;
        this.batchReference = batchReference
        this.cacheTime = cacheTime;
        this.lastSentToMesh = lastSentToMesh;
        this.createdBy = createdBy || "";
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy || "";
        this.updatedDate = updatedDate;
    }
}