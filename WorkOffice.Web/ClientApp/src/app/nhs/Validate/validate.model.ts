export interface PatientValidationDetailsModel {
  patientValidationDetailsId: number;
  patientValidationId: number;
  pathWayStatusId: number;
  specialityId: number;
  date: string;
  consultantId: number;
  endDate: string;
  patientId: number;
  activity: string;
  specialityCode: string;
  specialityName: string;
  pathWayStatusCode: string;
  pathWayStatusName: string;
  consultantCode: string;
  consultantName: string;
}

export interface MergePathwayModel {
  patientValidationDetailsId: number;
  patientValidationId: number;
}
