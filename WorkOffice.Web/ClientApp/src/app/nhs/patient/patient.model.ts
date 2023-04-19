
export interface PatientModel {
  patientId: number;
  districtNumber: string;
  firstName: string;
  lastName: string;
  middleName: string;
  dob: string;
  age: number;
  address: string;
  phoneNo: string;
  email: string;
  sex: string;
  postalCode: string;
  nhsNumber: string;
  active: boolean;
  fullName: string;
}

export interface PatientDocumentModel {
  patientDocumentId: number;
  pocumentTypeId: number;
  patientId: number;
  physicalLocation: string;
  documentName: string;
  documentExtension: string;
  documentFile: string;
  clinicDate:string;
  specialityId: number;
  consultantName: string;
  dateUploaded: string;
}

