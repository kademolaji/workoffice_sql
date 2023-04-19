
export interface CreateAppointmentModel {
  appointmentId: number;
  appTypeId: number;
  statusId : number;
  specialityId : number;
   bookDate : string;
  appDate : string;
  appTime : string;
  consultantId : number;
  hospitalId : number;
  wardId : number;
  departmentId : number;
  patientId : number;
  patientValidationId : number;
  comments : string;
  appointmentStatus : string;
  cancellationReason: string;
}

export interface AppointmentResponseModel {
  appointmentId: number;
  appTypeId: number;
  statusId : number;
  specialityId : number;
   bookDate : string;
  appDate : string;
  appTime : string;
  consultantId : number;
  hospitalId : number;
  wardId : number;
  departmentId : number;
  patientId : number;
  patientValidationId : number;
  comments : string;
  appointmentStatus : string;
  cancellationReason: string;
  speciality: string;
  patientUniqueNumber: string;
  pathwayUniqueNumber: string;
}

