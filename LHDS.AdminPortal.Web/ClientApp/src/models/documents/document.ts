import { Guid } from 'guid-typescript';

export class Document {
    public Url: string;

    constructor(document: any) {
        this.Url = document.Url;
    }
}