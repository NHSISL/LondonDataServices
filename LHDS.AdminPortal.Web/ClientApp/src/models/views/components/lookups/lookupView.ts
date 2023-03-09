export class LookupView {
    public id: string;
    public name?: string;

    constructor(id: string, name?: string, isActive?: boolean) {
        this.id = id;
        this.name = name || "";
    }
}
