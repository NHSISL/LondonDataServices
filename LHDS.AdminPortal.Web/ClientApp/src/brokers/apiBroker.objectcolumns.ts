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

    async GetAllObjectColumnsAsync(queryString: string) {
        const url = this.relativeObjectColumnUrl + queryString;
        
        if (queryString === "/") {
            return undefined;
        }
        
        return await this.apiBroker.GetAsync(url)
            .then(result => result.data.map((optOut: any) => new ObjectColumn(optOut)));
    }
}

export default ObjectColumnBroker;