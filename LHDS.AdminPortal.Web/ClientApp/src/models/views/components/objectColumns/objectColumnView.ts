import { Guid } from 'guid-typescript';
import { SpecificationObject } from '../../../specificationObjects/specificationObject';

export class ObjectColumnView {
    public id: Guid;
    public dataSetObjectId: Guid;
    public supplierColumnName: string;
    public ourColumnName: string;
    public columnDescription: string;
    public ordinalPosition: number;
    public populatedBy: string;
    public sqlDataType: string;
    public length: number;
    public precision: number;
    public scale: number;
    public fhirDataType: string;
    public supplierDateFormat: string;
    public isWatermark: boolean;
    public isSequencing: boolean;
    public isEntityBusinessKey: boolean;
    public isRecordBusinessKey: boolean;
    public isMutable: boolean;
    public isSenderCode: boolean;
    public isAuthorCode: boolean;
    public isDeleteFlag: boolean;
    public isPersonConfidentialData: boolean;
    public typeOfPersonConfidentialData: string;
    public maskingMethod: string;
    public isSensitiveRecordMarker: boolean;
    public codeSystem: string;
    public partitionColumnLevel: string;
    public dataTypeId: Guid;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;
    specificationObject?: SpecificationObject

    constructor(
        id: Guid,
        dataSetObjectId: Guid,
        supplierColumnName: string,
        ourColumnName: string,
        columnDescription: string,
        ordinalPosition: number,
        populatedBy: string,
        sqlDataType: string,
        length: number,
        precision: number,
        scale: number,
        fhirDataType: string,
        supplierDateFormat: string,
        isWatermark: boolean,
        isSequencing: boolean,
        isEntityBusinessKey: boolean,
        isRecordBusinessKey: boolean,
        isMutable: boolean,
        isSenderCode: boolean,
        isAuthorCode: boolean,
        isDeleteFlag: boolean,
        isPersonConfidentialData: boolean,
        typeOfPersonConfidentialData: string,
        maskingMethod: string,
        isSensitiveRecordMarker: boolean,
        codeSystem: string,
        partitionColumnLevel: string,
        dataTypeId: Guid,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
        specificationObject?: SpecificationObject
    ) {
        this.id = id
        this.dataSetObjectId = dataSetObjectId;
        this.supplierColumnName = supplierColumnName;
        this.ourColumnName = ourColumnName;
        this.columnDescription = columnDescription;
        this.ordinalPosition = ordinalPosition;
        this.populatedBy = populatedBy;
        this.sqlDataType = sqlDataType;
        this.length = length;
        this.precision = precision;
        this.scale = scale;
        this.fhirDataType = fhirDataType;
        this.supplierDateFormat = supplierDateFormat;
        this.isWatermark = isWatermark;
        this.isSequencing = isSequencing;
        this.isEntityBusinessKey = isEntityBusinessKey;
        this.isRecordBusinessKey = isRecordBusinessKey;
        this.isMutable = isMutable;
        this.isSenderCode = isSenderCode;
        this.isAuthorCode = isAuthorCode;
        this.isDeleteFlag = isDeleteFlag;
        this.isPersonConfidentialData = isPersonConfidentialData;
        this.typeOfPersonConfidentialData = typeOfPersonConfidentialData;
        this.maskingMethod = maskingMethod;
        this.isSensitiveRecordMarker = isSensitiveRecordMarker;
        this.codeSystem = codeSystem;
        this.partitionColumnLevel = partitionColumnLevel;
        this.dataTypeId = dataTypeId;;
        this.createdBy = createdBy || "";
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy || "";
        this.updatedDate = updatedDate;
        this.specificationObject = specificationObject;
    }
}