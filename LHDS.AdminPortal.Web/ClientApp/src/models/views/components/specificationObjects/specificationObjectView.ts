import { Guid } from 'guid-typescript';
import { DataSetSpecification } from '../../../dataSetSpecifications/dataSetSpecification';

export class SpecificationObjectView {
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
    public dataSetSpecification?: DataSetSpecification;

    constructor(
        id: Guid,
        dataSetSpecificationId: Guid,
        SupplierObjectName: string,
        OurObjectName: string,
        ObjectDescription: string,
        InterchangeProtocol: boolean,
        PushOrPull: string,
        DeletionHandling: string,
        IsSubmissionHeaderObject: boolean,
        IsTransactionLog: boolean,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
        dataSetSpecification?: DataSetSpecification
    ) {
        this.id = id;
        this.dataSetSpecificationId = dataSetSpecificationId;
        this.SupplierObjectName = SupplierObjectName;
        this.OurObjectName = OurObjectName;
        this.ObjectDescription = ObjectDescription;
        this.InterchangeProtocol = InterchangeProtocol;
        this.PushOrPull = PushOrPull;
        this.DeletionHandling = DeletionHandling;
        this.IsSubmissionHeaderObject = IsSubmissionHeaderObject;
        this.IsTransactionLog = IsTransactionLog;
        this.createdBy = createdBy || "";
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy || "";
        this.updatedDate = updatedDate;
        this.dataSetSpecification = dataSetSpecification;
    }
}