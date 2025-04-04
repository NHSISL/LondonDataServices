export class DataType {
    public id: string;
    public name: string;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(dataType: DataType) {
        this.id = dataType.id ? dataType.id : "";
        this.name = dataType.name;
        this.createdBy = dataType.createdBy;
        this.createdDate = new Date(dataType.createdDate);
        this.updatedBy = dataType.updatedBy;
        this.updatedDate = new Date(dataType.updatedDate);
    }
}