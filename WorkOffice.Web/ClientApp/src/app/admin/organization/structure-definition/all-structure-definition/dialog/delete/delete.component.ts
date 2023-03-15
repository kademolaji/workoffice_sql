import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { StructureDefinitionService } from '../../../structure-definition.service';
import { StructureDefinitionModel } from '../../../structure-definition.model';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteStructureDefinitionDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteStructureDefinitionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: StructureDefinitionModel,
    public structureDefinitionService: StructureDefinitionService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.structureDefinitionService.deleteStructureDefinition(this.data.structureDefinitionId).subscribe();
  }
}
