import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import {
  CreateResponse,
  DeleteReply,
  GetResponse,
  SearchCall,
  SearchParameter,
  SearchReply,
} from 'src/app/core/utilities/api-response';
import { DiagnosticModel, DiagnosticResultModel } from './diagnostic.model';
@Injectable()
export class DiagnosticService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<DiagnosticModel[]> = new BehaviorSubject<
    DiagnosticModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: DiagnosticModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): DiagnosticModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllDiagnostic(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<DiagnosticModel[]>>(
        `api/diagnostic/GetList`, option
      );
  }

  getDiagnosticById(id: number) {
    return this.httpClient.get<GetResponse<DiagnosticModel>>(
      `api/diagnostic/Get?diagnosticId=${id}`
    );
  }

  addDiagnostic(data: DiagnosticModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/diagnostic/Create`,
      data
    );
  }

  deleteDiagnostic(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/diagnostic/Delete?diagnosticId=${id}`
    );
  }

  deleteMultipleDiagnostic(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/diagnostic/MultipleDelete`, {targetIds: targetIds}
    );
  }

  getAllDiagnosticResult(option: SearchCall<SearchParameter>) {
    return this.httpClient
       .post<SearchReply<DiagnosticResultModel[]>>(
         `api/diagnosticresult/GetList`, option
       );
   }
   deleteDiagnosticResult(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/diagnosticresult/Delete?diagnosticResultId=${id}`
    );
  }
    addDiagnosticResult(formData: FormData) {
      return this.httpClient.post<CreateResponse>('/api/diagnosticresult/create', formData);
    }
    downloadDocument(id: number) {
      return this.httpClient.get(
        `api/diagnosticresult/Download?diagnosticResultId=${id}`
        , {
          reportProgress: true,
          observe: 'events',
          responseType: 'blob'
        });
    }
}
