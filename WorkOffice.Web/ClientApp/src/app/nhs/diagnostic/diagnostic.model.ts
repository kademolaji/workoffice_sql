
export interface DiagnosticModel {
  diagnosticId: number;
  patientId: number;
  patientName: string
  specialtyId: number;
  specialty: string;
  dtd: Date;
  problem: string;
  status: string;
  consultantName: string;
}

export interface DiagnosticResultModel {
  diagnosticResultId: number;
  diagnosticId: number;
  problem: string;
  consultantName: string;
  documentName: string;
  documentExtension: string;
  documentFile: string;
  testResultDate:Date;
  specialityId: number;
  dateUploaded: Date;
}

