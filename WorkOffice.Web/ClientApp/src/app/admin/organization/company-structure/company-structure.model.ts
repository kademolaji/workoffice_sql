
export interface CompanyStructureModel {
  companyStructureId: number;
  name: string;
  structureTypeId: number;
  structureType: string;
  country: string;
  address: string;
  contactPhone: string;
  contactEmail: string;
  companyHead: string;
  parentID: number;
  parent: string;
}

export interface AddCompanyStructureModel {
  companyStructureId: number;
  name: string;
  structureTypeId: number;
  country: string;
  address: string;
  contactPhone: string;
  contactEmail: string;
  companyHead: string;
  parentID: number;
}
