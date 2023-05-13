import { Component, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import {
  MatSnackBar,
  MatSnackBarVerticalPosition,
  MatSnackBarHorizontalPosition,
} from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { AddCompanyStructureModel } from '../company-structure.model';
import { CompanyStructureService } from '../company-structure.service';

@Component({
  selector: 'app-add-company-structure',
  templateUrl: './add-company-structure.component.html',
  styleUrls: ['./add-company-structure.component.css'],
})
export class AddCompanyStructureComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  companyStructureForm!: UntypedFormGroup;
  countryList: GeneralSettingsModel[] = [];
  structureDefinitionList: GeneralSettingsModel[] = [];
  companyStructureParentList: GeneralSettingsModel[] = [];
  hide3 = true;
  agree3 = false;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private companyStructureService: CompanyStructureService,
    private snackBar: MatSnackBar
  ) {
    super();
    this.companyStructureForm = this.fb.group({
      name: ['', [Validators.required]],
      structureTypeId: ['', [Validators.required]],
      country: ['', [Validators.required]],
      address: ['', [Validators.required]],
      contactPhone: ['', [Validators.required]],
      contactEmail: ['', [Validators.required]],
      companyHead: ['', [Validators.required]],
      parentID: [''],
    });
  }
  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.countryList = data['countryList'].entity;
      this.structureDefinitionList = data['structureDefinitionList'].entity;
      this.companyStructureParentList = data['companyStructureParentList'].entity;
    });


    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if (!this.isAddMode) {
      this.subs.sink = this.companyStructureService
        .getCompanyStructureById(this.id)
        .subscribe({
          next: (res) => {
            if (res.status) {
              console.log("res.entity", res.entity)
              this.companyStructureForm.patchValue({
                name: res.entity.name,
                structureTypeId: res.entity.structureTypeId,
                country: res.entity.country,
                address: res.entity.address,
                contactPhone: res.entity.contactPhone,
                contactEmail: res.entity.contactEmail,
                companyHead: res.entity.companyHead,
                parentID: res.entity.parentID,
              });
            }
          },
        });
    }
  }
  cancelForm() {
    this.router.navigate(['/admin/company/all-company-structure']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.companyStructureForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new company structure failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const structureDefinition: AddCompanyStructureModel = {
        companyStructureId: this.id ? +this.id : 0,
        name: this.companyStructureForm.value.name,
        structureTypeId: +this.companyStructureForm.value.structureTypeId,
        country: this.companyStructureForm.value.country,
        address: this.companyStructureForm.value.address,
        contactPhone: this.companyStructureForm.value.contactPhone,
        contactEmail: this.companyStructureForm.value.contactEmail,
        companyHead: this.companyStructureForm.value.companyHead,
        parentID: +this.companyStructureForm.value.parentID,
      };
      this.subs.sink = this.companyStructureService
        .addCompanyStructure(structureDefinition)
        .subscribe({
          next: (res) => {
            if (res.status) {
              this.loading = false;
              this.showNotification(
                'snackbar-success',
                res.message,
                'top',
                'right'
              );
              this.router.navigate(['/admin/company/all-company-structure']);
            } else {
              this.showNotification(
                'snackbar-danger',
                res.message,
                'top',
                'right'
              );
            }
          },
          error: (error) => {
            this.showNotification('snackbar-danger', error, 'top', 'right');
            this.submitted = false;
            this.loading = false;
          },
        });
    }
  }

  showNotification(
    colorName: string,
    text: string,
    placementFrom: MatSnackBarVerticalPosition,
    placementAlign: MatSnackBarHorizontalPosition
  ) {
    this.snackBar.open(text, '', {
      duration: 2000,
      verticalPosition: placementFrom,
      horizontalPosition: placementAlign,
      panelClass: colorName,
    });
  }
}
