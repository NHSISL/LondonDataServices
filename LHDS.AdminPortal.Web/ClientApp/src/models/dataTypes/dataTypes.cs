import { Guid } from 'guid-typescript';

export class DataType
{
    public id: Guid;
    public name: string;
    public createdDate: Date;
    public createdBy: string;
    public updatedDate: Date;
    public updatedBy: string;

    constructor(dataType: any)
    {
        this.id = dataType.id ? Guid.parse(dataType.id) : Guid.parse(Guid.EMPTY);
        this.name = dataType.name || "";
        this.createdDate = dataType.createdDate ? new Date(dataType.createdDate) : new Date();
        this.createdBy = dataType.createdBy || "";
        this.updatedDate = dataType.updatedDate ? new Date(dataType.updatedDate) : new Date();
        this.updatedBy = dataType.updatedBy || "";
    }
}