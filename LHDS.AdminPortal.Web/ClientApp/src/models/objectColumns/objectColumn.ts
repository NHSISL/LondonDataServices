import { Guid } from 'guid-typescript';
import { DataSetObject } from '../dataSetObjects/dataSetObject';
import { DataType } from '../dataTypes/dataType';

export class ObjectColumn {
    public id: Guid;
    public specificationObjectId: Guid;
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
    public dataType?: DataType;
    public dataSetObject?: DataSetObject;

    constructor(objectColumn: any) {
        this.id = objectColumn.id ? Guid.parse(objectColumn.id) : Guid.parse(Guid.EMPTY);
        this.specificationObjectId = objectColumn.specificationObjectId;
        this.supplierColumnName = objectColumn.supplierColumnName;
        this.ourColumnName = objectColumn.ourColumnName;
        this.columnDescription = objectColumn.columnDescription;
        this.ordinalPosition = objectColumn.ordinalPosition;
        this.populatedBy = objectColumn.populatedBy;
        this.sqlDataType = objectColumn.sqlDataType;
        this.length = objectColumn.length;
        this.precision = objectColumn.precision;
        this.scale = objectColumn.scale;
        this.fhirDataType = objectColumn.fhirDataType;
        this.supplierDateFormat = objectColumn.supplierDateFormat;
        this.isWatermark = objectColumn.isWatermark;
        this.isSequencing = objectColumn.isSequencing;
        this.isEntityBusinessKey = objectColumn.isEntityBusinessKey;
        this.isRecordBusinessKey = objectColumn.isRecordBusinessKey;
        this.isMutable = objectColumn.isMutable;
        this.isSenderCode = objectColumn.isSenderCode;
        this.isAuthorCode = objectColumn.isAuthorCode;
        this.isDeleteFlag = objectColumn.isDeleteFlag;
        this.isPersonConfidentialData = objectColumn.isPersonConfidentialData;
        this.typeOfPersonConfidentialData = objectColumn.typeOfPersonConfidentialData;
        this.maskingMethod = objectColumn.maskingMethod;
        this.isSensitiveRecordMarker = objectColumn.isSensitiveRecordMarker;
        this.codeSystem = objectColumn.codeSystem;
        this.partitionColumnLevel = objectColumn.partitionColumnLevel;
        this.dataTypeId = objectColumn.dataTypeId;;
        this.createdBy = objectColumn.createdBy;
        this.createdDate = new Date(objectColumn.createdDate);
        this.updatedBy = objectColumn.updatedBy;
        this.updatedDate = new Date(objectColumn.updatedDate);

        if (objectColumn.dataType !== undefined && objectColumn.dataType !== null) {
            this.dataType = new DataType(objectColumn.dataType);
        }

        if (objectColumn.dataSetObject !== undefined && objectColumn.dataSetObject !== null) {
            this.dataSetObject = new DataSetObject(objectColumn.dataSetObject);
        }
    }
}