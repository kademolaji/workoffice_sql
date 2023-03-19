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

}
