import { Guid } from 'guid-typescript';

export class DataType {
    public id: Guid;
    public name: string;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(dataType: any) {
        this.id = dataType.id ? Guid.parse(dataType.id) : Guid.parse(Guid.EMPTY);
        this.name = dataType.name;
        this.createdBy = dataType.createdBy;
        this.createdDate = new Date(dataType.createdDate);
        this.updatedBy = dataType.updatedBy;
        this.updatedDate = new Date(dataType.updatedDate);
    }
}