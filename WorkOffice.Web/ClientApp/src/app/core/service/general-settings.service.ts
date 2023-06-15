import { Injectable } from '@angular/core';
import { HttpClient,  } from '@angular/common/http';
import { GetResponse } from '../utilities/api-response';
import { GeneralSettingsModel } from '../models/general-settings.model';

@Injectable()
export class GeneralSettingsService {

  constructor(private httpClient: HttpClient) {
  }


  getStructureDefinitionList() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetStructureDefinitionList`
    );
  }

  getCompanyStructureList() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetCompanyStructureList`
    );
  }

  getCountryList() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetCountryList`
    );
  }

  getUserRoleList() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetUserRoleList`
    );
  }

  getCompanyList() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetCompanyList`
    );
  }
  getNHSActivity() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetNHSActivity`
    );
  }
  getAppType() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetAppType`
    );
  }
  getConsultant() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetConsultant`
    );
  }
  getHospital() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetHospital`
    );
  }
  getPathwayStatus() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetPathwayStatus`
    );
  }
  getRTT() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetRTT`
    );
  }
  getSpecialty() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetSpecialty`
    );
  }
  getWaitingType() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetWaitingType`
    );
  }
  getWard() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetWard`
    );
  }
  getPatientList(search?: string) {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetPatientList?search=${search}`
    );
  }

  getPatientPathWayList(search?: string) {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetPatientPathWayList?search=${search}`
    );
  }

  getPathWayListByPatientId(patientId: number, search?: string) {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetPathWayListByPatientId?patientId=${patientId}&search=${search}`
    );
  }

  getDepartmentList() {
    return this.httpClient.get<GetResponse<GeneralSettingsModel[]>>(
      `api/settings/GetDepartmentList`
    );
  }
}
