import { Guid } from 'guid-typescript';

export class DataSet {
    public id: Guid;
    public dataSetName: string;
    public dataSetAliasses: string;
    public dataSetSupplier: string;
    public dataSetAuthor: string;
    public specifiedBy: string;
    public isNationallySpecified: boolean;
    public collectedBy: string;
    public isNationallyCollected: boolean;
    public dataSourceType: string;
    public isActive: boolean;
    public activeFrom?: Date;
    public activeTo?: Date;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(dataSet: any) {
        this.id = dataSet.id ? Guid.parse(dataSet.id) : Guid.parse(Guid.EMPTY);
        this.dataSetName = dataSet.name;
        this.dataSetAliasses = dataSet.dataSetAliasses;
        this.dataSetSupplier = dataSet.dataSetSupplier;
        this.dataSetAuthor = dataSet.dataSetAuthor;
        this.specifiedBy = dataSet.specifiedBy;
        this.isNationallySpecified = dataSet.isNationallySpecified;
        this.collectedBy = dataSet.collectedBy;
        this.isNationallyCollected = dataSet.isNationallyCollected;
        this.dataSourceType = dataSet.dataSourceType;
        this.isActive = dataSet.isActive;
        this.activeTo = dataSet.activeTo;
        this.createdDate = dataSet.createdDate;
        this.createdBy = dataSet.createdBy;
        this.createdDate = new Date(dataSet.createdDate);
        this.updatedBy = dataSet.updatedBy;
        this.updatedDate = new Date(dataSet.updatedDate);
    }
}