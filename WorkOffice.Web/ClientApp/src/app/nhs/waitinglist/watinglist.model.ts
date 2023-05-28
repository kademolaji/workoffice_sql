
export interface WaitinglistModel {
  waitinglistId : number;
  waitTypeId: number;
  specialityId : number;
  tciDate : Date;
  waitinglistDate : Date;
  waitinglistTime : string;
  patientId : number;
  patientValidationId: number;
  condition : string;
  waitinglistStatus: string;
  districtNumber: string;
  pathWayNumber: string;
}

