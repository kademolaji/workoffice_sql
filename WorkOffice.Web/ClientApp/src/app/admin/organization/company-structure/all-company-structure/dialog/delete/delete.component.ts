import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { CompanyStructureModel } from '../../../company-structure.model';
import { CompanyStructureService } from '../../../company-structure.service';


@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteCompanyStructureDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteCompanyStructureDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CompanyStructureModel,
    public structureDefinitionService: CompanyStructureService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.structureDefinitionService.deleteCompanyStructure(this.data.companyStructureId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
