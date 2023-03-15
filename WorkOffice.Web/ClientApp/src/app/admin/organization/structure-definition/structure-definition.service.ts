import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import {
  CreateResponse,
  DeleteReply,
  GetResponse,
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

  getAllStructureDefinition(pageNumber: number, pageSize: number): void {
    this.subs.sink = this.httpClient
      .get<GetResponse<StructureDefinitionModel[]>>(
        `api/structuredefinition/GetList?pageNumber=${pageNumber}&pageSize=${pageSize}`
      )
      .subscribe({
        next: (data) => {
          this.isTblLoading = false;
          this.dataChange.next(data.entity);
        },
        error: (error: HttpErrorResponse) => {
          this.isTblLoading = false;
          console.log(error.name + ' ' + error.message);
        },
      });
  }

  getStructureDefinitionById(id: string) {
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
  updateStructureDefinition(data: StructureDefinitionModel): void {
    this.dialogData = data;
  }
  deleteStructureDefinition(id: string) {
    return this.httpClient.delete<DeleteReply>(
      `api/structuredefinition/Delete?=structureDefinitionId=${id}`
    );
  }
  deleteMultipleStructureDefinition(targetIds: string[]) {
    return this.httpClient.post<DeleteReply>(
      `api/structuredefinition/MultipleDelete`, {targetIds: targetIds}
    );
  }
}
