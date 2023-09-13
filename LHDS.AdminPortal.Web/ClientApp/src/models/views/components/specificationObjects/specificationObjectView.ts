import { Guid } from 'guid-typescript';
import { DataSetSpecification } from '../../../dataSetSpecifications/dataSetSpecification';

export class SpecificationObjectView {
    public id: Guid;
    public dataSetSpecificationId: Guid;
    public supplierObjectName: string;
    public ourObjectName: string;
    public objectDescription: string;
    public interchangeProtocol: string;
    public isPushedToUs: boolean;
    public isPulledByUs: boolean;
    public deletionHandling: string;
    public isSubmissionHeaderObject: boolean;
    public isTransactionLog: boolean
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;
    public dataSetSpecification?: DataSetSpecification;

    constructor(
        id: Guid,
        dataSetSpecificationId: Guid,
        supplierObjectName?: string,
        ourObjectName?: string,
        objectDescription?: string,
        interchangeProtocol?: string,
        isPushedToUs?: boolean,
        isPulledByUs?: boolean,
        deletionHandling?: string,
        isSubmissionHeaderObject?: boolean,
        isTransactionLog?: boolean,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
        dataSetSpecification?: DataSetSpecification
    ) {
        this.id = id;
        this.dataSetSpecificationId = dataSetSpecificationId;
        this.supplierObjectName = supplierObjectName || "";
        this.ourObjectName = ourObjectName || "";
        this.objectDescription = objectDescription || "";
        this.interchangeProtocol = interchangeProtocol || "";
        this.isPushedToUs = isPushedToUs || false;
        this.isPulledByUs = isPulledByUs || false;
        this.deletionHandling = deletionHandling || "";
        this.isSubmissionHeaderObject = isSubmissionHeaderObject || false;
        this.isTransactionLog = isTransactionLog || false;
        this.createdBy = createdBy || "";
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy || "";
        this.updatedDate = updatedDate;
        this.dataSetSpecification = dataSetSpecification;
    }
}