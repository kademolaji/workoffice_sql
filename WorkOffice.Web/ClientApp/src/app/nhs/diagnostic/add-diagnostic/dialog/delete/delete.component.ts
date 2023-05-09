import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { DiagnosticResultModel } from '../../../diagnostic.model';
import { DiagnosticService } from '../../../diagnostic.service';



@Component({
  selector: 'app-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.sass'],
})
export class DeleteDiagnosticResultDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteDiagnosticResultDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DiagnosticResultModel,
    public diagnosticService: DiagnosticService
  ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
  confirmDelete(): void {
    this.diagnosticService.deleteDiagnosticResult(this.data.diagnosticResultId).subscribe((data)=>{
      if(data.status){
        this.dialogRef.close();
      }
    });
  }
}
