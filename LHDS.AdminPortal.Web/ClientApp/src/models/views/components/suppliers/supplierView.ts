import { Guid } from 'guid-typescript';

export class SupplierView {
    public id: Guid;
    public name?: string;
    public friendlyName?: string;
    public description?: string;
    public isIngestionTracked?: boolean;
    public canDecryptIngestionTracking?: boolean;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(
        id: Guid,
        name?: string,
        friendlyName?: string,
        description?: string,
        isIngestionTracked?: boolean,
        canDecryptIngestionTracking?: boolean,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date) 
    {
        this.id = id;
        this.name = name || "";
        this.friendlyName = friendlyName || "";
        this.description = description || "";
        this.isIngestionTracked = isIngestionTracked === true ? true : false;
        this.canDecryptIngestionTracking = canDecryptIngestionTracking === true ? true : false;
        this.createdBy = createdBy !== undefined ? createdBy : '';
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy !== undefined ? updatedBy : ''
        this.updatedDate = updatedDate;
    }
}