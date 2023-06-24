import { SelectionModel } from '@angular/cdk/collections';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router, ActivatedRoute } from '@angular/router';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { PathwayResponseModel } from '../../pathway/pathway.model';
import { PathwayService } from '../../pathway/pathway.service';

@Component({
  selector: 'app-adhoc-pathway',
  templateUrl: './adhoc-pathway.component.html',
  styleUrls: ['./adhoc-pathway.component.css']
})
export class AdhocPathwayComponent
extends UnsubscribeOnDestroyAdapter
implements OnInit
{
displayedColumns: string[] = [
  'pathWayNumber',
  'pathWayStartDate',
  'pathWayEndDate',
  'specialityName',
];
selection = new SelectionModel<PathwayResponseModel>(true, []);
ELEMENT_DATA: PathwayService[] = [];
isLoading = false;
dataSource: MatTableDataSource<PathwayResponseModel> =
  new MatTableDataSource();
isTblLoading = false;
patientId = 0;

constructor(
  public httpClient: HttpClient,
  public dialog: MatDialog,
  public pathwayService: PathwayService,
  private snackBar: MatSnackBar,
  private router: Router,
  private route: ActivatedRoute
) {
  super();
}

@ViewChild(MatSort, { static: true })
sort!: MatSort;

ngOnInit() {
  this.patientId = +this.route.snapshot.params['id'];
  this.loadData(this.patientId);
}

refresh() {
  this.loadData(this.patientId);
}

selectCall(row: { patientValidationId: number; patientId: number }) {
  this.router.navigate([
    'nhs',
    row.patientId,
    'adhoc',
    row.patientValidationId,
  ]);
}

public loadData(patientId: number) {
  this.isTblLoading = true;
  this.pathwayService.getPathwayByPatientId(patientId).subscribe((res) => {
    this.isTblLoading = false;
    this.dataSource.data = res.entity;
  });
}
}
