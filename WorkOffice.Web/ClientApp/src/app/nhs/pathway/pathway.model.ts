
export interface CreatePathwayModel {
  patientValidationId: number;
  pathWayNumber: string;
  pathWayCondition: string;
  pathWayStatusId: number;
  rttId: number;
  districtNumber : string;
  specialtyId : number;
  pathWayStartDate : string;
  pathWayEndDate : string;
  nhsNumber : string;
  patientId : number;
  patientNumber: string;
  rttWait: string;
  specialityName: string;
}

export interface PathwayResponseModel {
  patientValidationId: number;
  pathWayNumber: string;
  pathWayCondition: string;
  pathWayStatusId: number;
  rttId: number;
  districtNumber : string;
  specialtyId : number;
  pathWayStartDate : string;
  pathWayEndDate : string;
  nhsNumber : string;
  patientId : number;
  patientNumber: string;
  rttWait: string;
  specialityName: string;
}

