import { Guid } from 'guid-typescript';

export class DataSetView {
    public id: Guid;
    public dataSetName: string;
    public dataSetAliases: string;
    public dataSetSupplier: string;
    public dataSetAuthor: string;
    public specifiedBy: string;
    public IsNationallySpecified: boolean;
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

    constructor(
        id: Guid,
        dataSetName?: string,
        dataSetAliases?: string,
        dataSetSupplier?: string,
        dataSetAuthor?: string,
        specifiedBy?: string,
        IsNationallySpecified?: boolean,
        collectedBy?: string,
        isNationallyCollected?: boolean,
        dataSourceType?: string,
        isActive?: boolean,
        activeFrom?: Date,
        activeTo?: Date,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
    ) {
        this.id = id;
        this.dataSetName = dataSetName || "";
        this.dataSetAliases = dataSetAliases || "";
        this.dataSetSupplier = dataSetSupplier || "";
        this.dataSetAuthor = dataSetAuthor || "";
        this.specifiedBy = specifiedBy || "";
        this.IsNationallySpecified = IsNationallySpecified === true ? true : false;
        this.collectedBy = collectedBy || "";
        this.isNationallyCollected = isNationallyCollected === true ? true : false;
        this.dataSourceType = dataSourceType || "";
        this.isActive = isActive === true ? true : false;
        this.activeFrom = activeFrom !== undefined ? new Date(activeFrom) : undefined;
        this.activeTo = activeTo !== undefined ? new Date(activeTo) : undefined;
        this.createdBy = createdBy !== undefined ? createdBy : '';
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy !== undefined ? updatedBy : '';
        this.updatedDate = updatedDate;
    }
}