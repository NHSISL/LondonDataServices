import { DataSet } from '../dataSets/dataSet';

export class DataSetSpecification {
    public id: string;
    public dataSetId: string;
    public supplierSpecificationVersion: string;
    public ourSpecificationVersion?: string;
    public notes?: string;
    public isMultiAuthorPerBatch: boolean;
    public entityChangeSynchronisation?: string;
    public dateReleased?: Date;
    public dateImplementation?: Date;
    public dateSuperseded?: Date;
    public presededById?: Guid;
    public supersededById?: Guid;
    public isPublished: boolean;
    public isActive: boolean;
    public activeFrom?: Date;
    public activeTo?: Date;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;
    public dataSet?: DataSet;

    constructor(dataSetSpecification: DataSetSpecification) {
        this.id = dataSetSpecification.id ? dataSetSpecification.id :"";
        this.dataSetId = dataSetSpecification.dataSetId;
        this.supplierSpecificationVersion = dataSetSpecification.supplierSpecificationVersion;
        this.ourSpecificationVersion = dataSetSpecification.ourSpecificationVersion;
        this.notes = dataSetSpecification.notes;
        this.isMultiAuthorPerBatch = dataSetSpecification.isMultiAuthorPerBatch;
        this.entityChangeSynchronisation = dataSetSpecification.entityChangeSynchronisation;
        this.dateReleased = dataSetSpecification.dateReleased;
        this.dateImplementation = dataSetSpecification.dateImplementation;
        this.dateSuperseded = dataSetSpecification.dateSuperseded;
        this.presededById = dataSetSpecification.presededById;
        this.supersededById = dataSetSpecification.supersededById;
        this.isPublished = dataSetSpecification.isPublished;
        this.isActive = dataSetSpecification.isActive;
        this.activeFrom = dataSetSpecification.activeFrom;
        this.activeTo = dataSetSpecification.activeTo;
        this.createdBy = dataSetSpecification.createdBy;
        this.createdDate = new Date(dataSetSpecification.createdDate);
        this.updatedBy = dataSetSpecification.updatedBy;
        this.updatedDate = new Date(dataSetSpecification.updatedDate);

        if (dataSetSpecification.dataSet !== undefined && dataSetSpecification.dataSet !== null) {
            this.dataSet = new DataSet(dataSetSpecification.dataSet);
        }
    }
}