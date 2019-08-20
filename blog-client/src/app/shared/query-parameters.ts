export abstract class QueryParameters {
    pageIndex?: number;
    pageSize?: number;
    fields?: string;
    orderBy?: string;

    /**
     * Query parameters, variables are equals to backend system
     */
    constructor(init?: Partial<QueryParameters>) {
        /**
         * Usage: new QueryParameters({fields: "something", ...});
         */
        Object.assign(this, init);
    }
}