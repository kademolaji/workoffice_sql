export interface ReferralDtoModel {
  referralId: number;
  patientId: number;
  specialtyId: number;
  consultantId: string;
  documentName: string;
  referralDate:string;
  }

  export interface ReferralModel {
    referralId: number;
    patientId: number;
    specialtyId: number;
    consultantId: string;
    documentName: string;
    referralDate: string;
    specialty: string;
    patientName: string;
    consultantName: string;
    }
