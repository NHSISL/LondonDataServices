import { Guid } from 'guid-typescript';
import { DataSet } from '../dataSets/dataSet';

export class DataSetSpecification {
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
    public presededBy: string;
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

    constructor(dataSetSpecification: any) {
        this.id = dataSetSpecification.id ? Guid.parse(dataSetSpecification.id) : Guid.parse(Guid.EMPTY);
        this.dataSetId = dataSetSpecification.dataSetId;
        this.supplierSpecificationVersion = dataSetSpecification.name;
        this.ourSpecificationVersion = dataSetSpecification.ourSpecificationVersion;
        this.notes = dataSetSpecification.notes;
        this.isMultiSender = dataSetSpecification.isMultiSender;
        this.entityChangeSynchronisation = dataSetSpecification.entityChangeSynchronisation;
        this.dateReleased = dataSetSpecification.dateReleased;
        this.dateImplemented = dataSetSpecification.dateImplemented;
        this.dateSuperseded = dataSetSpecification.dateSuperseded;
        this.presededBy = dataSetSpecification.presededBy;
        this.supersededBy = dataSetSpecification.dataSourceType;
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