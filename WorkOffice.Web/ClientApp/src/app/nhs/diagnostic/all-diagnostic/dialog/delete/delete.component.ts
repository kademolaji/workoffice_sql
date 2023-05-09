import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { DiagnosticModel } from '../../../diagnostic.model';
import { DiagnosticService } from '../../../diagnostic.service';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteDiagnosticDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteDiagnosticDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DiagnosticModel,
    public diagnosticService: DiagnosticService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.diagnosticService.deleteDiagnostic(this.data.diagnosticId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
