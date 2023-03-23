import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import {
  CreateResponse,
  DeleteReply,
  GetResponse,
  SearchCall,
  SearchParameter,
  SearchReply,
} from 'src/app/core/utilities/api-response';
import { AddCompanyStructureModel, CompanyStructureModel } from './company-structure.model';

@Injectable()
export class CompanyStructureService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<CompanyStructureModel[]> = new BehaviorSubject<
    CompanyStructureModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: AddCompanyStructureModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): CompanyStructureModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllCompanyStructure(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<CompanyStructureModel[]>>(
        `api/companystructure/GetList`, option
      );
  }

  getCompanyStructureById(id: number) {
    return this.httpClient.get<GetResponse<CompanyStructureModel>>(
      `api/companystructure/Get?companystructureId=${id}`
    );
  }

  addCompanyStructure(data: AddCompanyStructureModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/companystructure/Create`,
      data
    );
  }

  deleteCompanyStructure(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/companystructure/Delete?companystructureId=${id}`
    );
  }
  deleteMultipleCompanyStructure(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/companystructure/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
