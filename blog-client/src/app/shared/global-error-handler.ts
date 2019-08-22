import { Injectable, ErrorHandler } from "@angular/core";
import { ErrorLoggerService } from './error-logger.service';

@Injectable()
export class GlobalErrorHandler extends ErrorHandler {
    constructor(private errorLoggerService: ErrorLoggerService) {
        super();
    }

    handleError(error: any) {
        this.errorLoggerService.logError(error);
    }
}