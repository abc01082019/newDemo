import { QueryParameters } from 'src/app/shared/query-parameters';

export class PostParameters extends QueryParameters {
    title?: string;

    /**
     * Initital QueryParameters 
     */
    constructor(init?: Partial<PostParameters>) {
        super(init);
        Object.assign(this, init);
    }
}