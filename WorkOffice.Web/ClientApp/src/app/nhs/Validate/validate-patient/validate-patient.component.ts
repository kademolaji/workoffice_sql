import { Component, OnInit } from '@angular/core';
import {
  UntypedFormGroup,
  UntypedFormBuilder,
  Validators,
} from '@angular/forms';
import {
  MatSnackBar,
  MatSnackBarVerticalPosition,
  MatSnackBarHorizontalPosition,
} from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import {
  filter,
  distinctUntilChanged,
  debounceTime,
  tap,
  switchMap,
  catchError,
  finalize,
} from 'rxjs';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { CreatePathwayModel } from '../../pathway/pathway.model';
import { PathwayService } from '../../pathway/pathway.service';

@Component({
  selector: 'app-validate-patient',
  templateUrl: './validate-patient.component.html',
  styleUrls: ['./validate-patient.component.css'],
})
export class ValidatePatientComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  pathwayForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  id = 0;
  patientId = 0;
  departmentList: GeneralSettingsModel[] = [];
  consultantList: GeneralSettingsModel[] = [];
  appTypeList: GeneralSettingsModel[] = [];
  hospitalList: GeneralSettingsModel[] = [];
  specialityList: GeneralSettingsModel[] = [];
  pathwayStatusList: GeneralSettingsModel[] = [];
  patientList: GeneralSettingsModel[] = [];
  rttList: GeneralSettingsModel[] = [];
  minLengthTerm = 3;
  isLoading = false;

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private pathwayService: PathwayService,
    private snackBar: MatSnackBar,
    private generalSettingsService: GeneralSettingsService
  ) {
    super();
  }
  ngOnInit() {
    this.pathwayForm = this.fb.group({
      pathWayNumber: [''],
      pathWayCondition: ['', [Validators.required]],
      specialtyId: [0, [Validators.required]],
      pathWayStartDate: ['', [Validators.required]],
      pathWayEndDate: [''],
      nhsNumber: [''],
      pathWayStatusId: ['', [Validators.required]],
      rttId: [],
      patientId: ['', [Validators.required]],
      rttWait: [''],
      specialityName: [''],
    });

    this.subs.sink = this.generalSettingsService
      .getSpecialty()
      .subscribe((response) => {
        this.specialityList = response.entity;
      });

    this.subs.sink = this.generalSettingsService
      .getPathwayStatus()
      .subscribe((response) => {
        this.pathwayStatusList = response.entity;
      });
    this.subs.sink = this.generalSettingsService
      .getRTT()
      .subscribe((response) => {
        this.rttList = response.entity;
      });

    this.id = +this.route.snapshot.params['id'];
    this.patientId = +this.route.snapshot.params['patientId'];
    this.loadPathway(this.id);
  }

  loadPathway(id: number) {
    this.subs.sink = this.pathwayService.getPathwayById(id).subscribe({
      next: (res) => {
        if (res.status) {
          this.pathwayForm.setValue({
            pathWayNumber: res.entity.pathWayNumber,
            pathWayCondition: res.entity.pathWayCondition,
            specialtyId: res.entity.specialtyId,
            pathWayStartDate: res.entity.pathWayStartDate,
            pathWayEndDate: res.entity.pathWayEndDate ? new Date(res.entity.pathWayEndDate) : '',
            nhsNumber: res.entity.nhsNumber,
            pathWayStatusId: res.entity.pathWayStatusId,
            rttId: res.entity.rttId,
            patientId: {
              label: res.entity.patientName,
              value: res.entity.patientId,
            },
            rttWait: res.entity.rttWait,
            specialityName: res.entity.specialityName,
          });
        }
      },
    });
  }

  displayWith(value: any) {
    return value?.label;
  }
  cancelForm(){
  this.router.navigate(['nhs', 'validate-now', this.patientId]);
  }
  refreshLoadPathway(evt: any) {
console.log("Fire Fire", evt)
    this.loadPathway(this.id);
  }
}
