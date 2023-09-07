import { Guid } from 'guid-typescript';

export class DataSetView {
    public id: Guid;
    public dataSetName?: string;
    public dataSetAliasses?: string;
    public dataSetSupplier?: string;
    public dataSetAuthor?: string;
    public specifiedBy?: string;
    public isNationallySpecified?: boolean;
    public collectedBy?: string;
    public isNationallyCollected?: boolean;
    public dataSourceType?: string;
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
        dataSetAliasses?: string,
        dataSetSupplier?: string,
        dataSetAuthor?: string,
        specifiedBy?: string,
        isNationallySpecified?: boolean,
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
        this.dataSetAliasses = dataSetAliasses || "";
        this.dataSetSupplier = dataSetSupplier || "";
        this.dataSetAuthor = dataSetAuthor || "";
        this.specifiedBy = specifiedBy;
        this.isNationallySpecified = isNationallySpecified === false ? false : true;
        this.collectedBy = collectedBy;
        this.isNationallyCollected = isNationallyCollected === false ? false : true;
        this.dataSourceType = dataSourceType || "";
        this.isActive = isActive === false ? false : true;
        this.activeFrom = activeFrom !== undefined ? new Date(activeFrom) : undefined;
        this.activeTo = activeTo !== undefined ? new Date(activeTo) : undefined;
        this.createdBy = createdBy || "";
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy || "";
        this.updatedDate = updatedDate;
    }
}