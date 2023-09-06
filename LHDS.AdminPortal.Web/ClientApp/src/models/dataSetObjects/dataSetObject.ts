import { Guid } from 'guid-typescript';

export class DataSetObject {
    public id: Guid;
    public dataSetSpecificationId: Guid;
    public SupplierObjectName: string;
    public OurObjectName: string;
    public ObjectDescription: string;
    public InterchangeProtocol: boolean;
    public PushOrPull: string;
    public DeletionHandling: string;
    public IsSubmissionHeaderObject: boolean;
    public IsTransactionLog: boolean
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(dataSetObjects: any) {
        this.id = dataSetObjects.id ? Guid.parse(dataSetObjects.id) : Guid.parse(Guid.EMPTY);
        this.dataSetSpecificationId = dataSetObjects.dataSetSpecificationId;
        this.SupplierObjectName = dataSetObjects.SupplierObjectName;
        this.OurObjectName = dataSetObjects.ourSpecificationVersion;
        this.ObjectDescription = dataSetObjects.ObjectDescription;
        this.InterchangeProtocol = dataSetObjects.InterchangeProtocol;
        this.PushOrPull = dataSetObjects.PushOrPull;
        this.DeletionHandling = dataSetObjects.DeletionHandling;
        this.IsSubmissionHeaderObject = dataSetObjects.IsSubmissionHeaderObject;
        this.IsTransactionLog = dataSetObjects.IsTransactionLog;
        this.createdDate = dataSetObjects.createdDate;
        this.createdBy = dataSetObjects.createdBy;
        this.createdDate = new Date(dataSetObjects.createdDate);
        this.updatedBy = dataSetObjects.updatedBy;
        this.updatedDate = new Date(dataSetObjects.updatedDate);
    }
}