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
import { StructureDefinitionModel } from './structure-definition.model';
@Injectable()
export class StructureDefinitionService extends UnsubscribeOnDestroyAdapter {
  isTblLoading = true;
  dataChange: BehaviorSubject<StructureDefinitionModel[]> = new BehaviorSubject<
    StructureDefinitionModel[]
  >([]);
  // Temporarily stores data from dialogs
  dialogData!: StructureDefinitionModel;
  constructor(private httpClient: HttpClient) {
    super();
  }
  get data(): StructureDefinitionModel[] {
    return this.dataChange.value;
  }
  getDialogData() {
    return this.dialogData;
  }

  getAllStructureDefinition(option: SearchCall<SearchParameter>) {
   return this.httpClient
      .post<SearchReply<StructureDefinitionModel[]>>(
        `api/structuredefinition/GetList`, option
      );
  }

  getStructureDefinitionById(id: number) {
    return this.httpClient.get<GetResponse<StructureDefinitionModel>>(
      `api/structuredefinition/Get?structureDefinitionId=${id}`
    );
  }

  addStructureDefinition(data: StructureDefinitionModel) {
    this.dialogData = data;
    return this.httpClient.post<CreateResponse>(
      `api/structuredefinition/Create`,
      data
    );
  }

  deleteStructureDefinition(id: number) {
    return this.httpClient.delete<DeleteReply>(
      `api/structuredefinition/Delete?structureDefinitionId=${id}`
    );
  }

  deleteMultipleStructureDefinition(targetIds: number[]) {
    return this.httpClient.post<DeleteReply>(
      `api/structuredefinition/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
