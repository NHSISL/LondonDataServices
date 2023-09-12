import { Guid } from 'guid-typescript';
import { DataSet } from '../../../dataSets/dataSet';

export class DataSetSpecificationView {
    public id: Guid;
    public dataSetId: Guid;
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

    constructor(
        id: Guid,
        dataSetId: Guid,
        supplierSpecificationVersion?: string,
        isPublished?: boolean,
        isMultiAuthorPerBatch?: boolean,
        isActive?: boolean,
        ourSpecificationVersion?: string,
        notes?: string,
        entityChangeSynchronisation?: string,
        dateReleased?: Date,
        dateImplementation?: Date,
        dateSuperseded?: Date,
        supersededById?: Guid,
        presededById?: Guid,
        activeFrom?: Date,
        activeTo?: Date,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
        dataSet?: DataSet,
    ) {
        this.id = id;
        this.dataSetId = dataSetId;
        this.supplierSpecificationVersion = supplierSpecificationVersion || "";
        this.isPublished = isPublished || false;
        this.isMultiAuthorPerBatch = isMultiAuthorPerBatch || false;;
        this.isActive = isActive || false;
        this.ourSpecificationVersion = ourSpecificationVersion || "";
        this.notes = notes || "";
        this.entityChangeSynchronisation = entityChangeSynchronisation || "";
        this.presededById = presededById;
        this.supersededById = supersededById;
        this.activeFrom = activeFrom;
        this.activeTo = activeTo;
        this.dateReleased = dateReleased;
        this.dateImplementation = dateImplementation;
        this.dateSuperseded = dateSuperseded;
        this.createdBy = createdBy || "";
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy || "";
        this.updatedDate = updatedDate;
        this.dataSet = dataSet;
    }
}