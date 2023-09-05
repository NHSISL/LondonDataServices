import { Guid } from 'guid-typescript';
import { DataSet } from '../../../dataSets/dataSet';

export class DataSetSpecificationView {
    public id: Guid;
    public dataSetId: Guid;
    public supplierSpecificationVersion: string;
    public ourSpecificationVersion: string;
    public notes: string;
    public isMultiSender: boolean;
    public entityChangeSynchronisation: string;
    public dateReleased?: Date;
    public dateImplemented?: Date;
    public dateSuperseded?: Date;
    public presededBy: string
    public supersededBy: string;
    public isPublished: boolean;
    public isActive: boolean;
    public activeFrom: Date;
    public activeTo: Date;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;
    public dataSet?: DataSet;

    constructor(
        id: Guid,
        dataSetId: Guid,
        supplierSpecificationVersion: string,
        ourSpecificationVersion: string,
        notes: string,
        isMultiSender: boolean,
        entityChangeSynchronisation: string,
        presededBy: string,
        supersededBy: string,
        isPublished: boolean,
        isActive: boolean,
        activeFrom: Date,
        activeTo: Date,
        dateReleased?: Date,
        dateImplemented?: Date,
        dateSuperseded?: Date,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
        dataSet?: DataSet,
    ) {
        this.id = id;
        this.dataSetId = dataSetId;
        this.supplierSpecificationVersion = supplierSpecificationVersion;
        this.ourSpecificationVersion = ourSpecificationVersion;
        this.notes = notes;
        this.isMultiSender = isMultiSender;
        this.entityChangeSynchronisation = entityChangeSynchronisation;
        this.presededBy = presededBy;
        this.supersededBy = supersededBy;
        this.isPublished = isPublished;
        this.isActive = isActive;
        this.activeFrom = activeFrom;
        this.activeTo = activeTo;
        this.dateReleased = dateReleased;
        this.dateImplemented = dateImplemented;
        this.dateSuperseded = dateSuperseded;
        this.createdBy = createdBy || "";
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy || "";
        this.updatedDate = updatedDate;
        this.dataSet = dataSet;
    }
}