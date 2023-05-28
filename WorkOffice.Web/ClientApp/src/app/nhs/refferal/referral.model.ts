export interface ReferralDtoModel {
  referralId: number;
  patientId: number;
  specialtyId: number;
  consultantId: string;
  documentName: string;
  referralDate:Date;
  }

  export interface ReferralModel {
    referralId: number;
    patientId: number;
    specialtyId: number;
    consultantId: string;
    documentName: string;
    referralDate: Date;
    specialty: string;
    patientName: string;
    consultantName: string;
    }
