import { Injectable } from '@angular/core';

@Injectable({
    providedIn: "root"
})
export class ErrorLoggerService {
    constructor() { }
    logError(error: any) {
        console.error('Demo', "An error occurred", error);
    }
}