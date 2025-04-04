export class DataSet {
    public id: string;
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

    constructor(dataSet: DataSet) {
        this.id = dataSet.id ? dataSet.id : "";
        this.dataSetName = dataSet.dataSetName || "";
        this.dataSetAliases = dataSet.dataSetAliases || "";
        this.dataSetSupplier = dataSet.dataSetSupplier || "";
        this.dataSetAuthor = dataSet.dataSetAuthor || "";
        this.specifiedBy = dataSet.specifiedBy || "";
        this.IsNationallySpecified = dataSet.IsNationallySpecified === true ? true : false;
        this.collectedBy = dataSet.collectedBy || "";
        this.isNationallyCollected = dataSet.isNationallyCollected === true ? true : false;
        this.dataSourceType = dataSet.dataSourceType || "";
        this.isActive = dataSet.isActive === true ? true : false;
        this.activeFrom = dataSet.activeFrom;
        this.activeTo = dataSet.activeTo;
        this.createdDate = dataSet.createdDate;
        this.createdBy = dataSet.createdBy;
        this.createdDate = new Date(dataSet.createdDate);
        this.updatedBy = dataSet.updatedBy;
        this.updatedDate = new Date(dataSet.updatedDate);
    }
}