import { Guid } from 'guid-typescript';

export class DataTypeView {
    public id: Guid;
    public name: string;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(
        id: Guid,
        name?: string,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,

    ) {
        this.id = id;
        this.name = name || "";
        this.createdBy = createdBy || "";
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy || "";
        this.updatedDate = updatedDate;
    }
}