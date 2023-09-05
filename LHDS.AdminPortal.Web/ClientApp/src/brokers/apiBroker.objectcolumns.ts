import { Guid } from "guid-typescript";
import { ObjectColumn } from "../models/objectColumns/objectColumn";
import ApiBroker from "./apiBroker";

class ObjectColumnBroker {
    relativeObjectColumnUrl = '/api/ObjectColumns';

    private apiBroker: ApiBroker = new ApiBroker();

    async PostObjectColumnAsync(objectColumn: ObjectColumn) {
        return await this.apiBroker.PostAsync(this.relativeObjectColumnUrl, objectColumn)
            .then(result => new ObjectColumn(result.data));
    }
}

export default ObjectColumnBroker;
