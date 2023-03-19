
export interface CompanyStructureModel {
  companyStructureId: string;
  name: string;
  structureTypeId: string;
  structureType: string;
  country: string;
  address: string;
  contactPhone: string;
  contactEmail: string;
  companyHead: string;
  parentID: string;
  parent: string;
}

export interface AddCompanyStructureModel {
  companyStructureId: string;
  name: string;
  structureTypeId: string;
  country: string;
  address: string;
  contactPhone: string;
  contactEmail: string;
  companyHead: string;
  parentID: string;
}
