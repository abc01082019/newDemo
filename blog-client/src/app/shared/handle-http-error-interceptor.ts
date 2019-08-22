import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { GlobalErrorHandler } from './global-error-handler';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HandleHttpErrorIntercetor implements HttpInterceptor {
    constructor(private globalErrorHandler: GlobalErrorHandler) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.error instanceof Error) {
                    // client-side or network error
                    const errorToLog = `Http error (client/network). ${error.message}`;
                    this.globalErrorHandler.handleError(errorToLog);
                } else {
                    // tslint:disable-next-line:max-line-length
                    const errorToLog = `Http error (unsuccessful reponse). Message: ${error.message}, status code: ${(error).status}, body: ${JSON.stringify(error.error)} `;
                    this.globalErrorHandler.handleError(errorToLog);
                }
                if (error.status === 422) {
                    return throwError(error.error);
                }
            })
        );
    }

}