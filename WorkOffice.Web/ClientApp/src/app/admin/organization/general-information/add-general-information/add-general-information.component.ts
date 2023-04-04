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
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { GeneralSettingsService } from 'src/app/core/service/general-settings.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { GeneralInformationModel } from '../general-information.model';
import { GeneralInformationService } from '../general-information.service';

@Component({
  selector: 'app-add-general-information',
  templateUrl: './add-general-information.component.html',
  styleUrls: ['./add-general-information.component.css'],
})
export class AddGeneralInformationComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  generalInformationForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;
  countryList: GeneralSettingsModel[] = [];
  ismulticompany= false;

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private generalInformationService: GeneralInformationService,
    private snackBar: MatSnackBar,
    private generalSettingsService: GeneralSettingsService
  ) {
    super();
  }
  ngOnInit() {
    this.generalInformationForm = this.fb.group({
      organisationName:['', [Validators.required]],
      taxid:['', [Validators.required]],
      regno: ['', [Validators.required]],
      phone:['', [Validators.required]],
      email:['', [Validators.required]],
      fax: [''],
      address1: ['', [Validators.required]],
      address2: [''],
      city: ['', [Validators.required]],
      state: ['', [Validators.required]],
      country: ['', [Validators.required]],
      note: [''],
      zipcode: ['', [Validators.required]],
      currency: ['', [Validators.required]],
      ismulticompany:[false, [Validators.required]],
      subsidiary_level:['', [Validators.required]],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if (!this.isAddMode) {
      this.subs.sink = this.generalInformationService
        .getGeneralInformationById(this.id)
        .subscribe({
          next: (res) => {
            if (res.status) {
              this.generalInformationForm.setValue({
                organisationName: res.entity.organisationName,
                taxid: res.entity.taxid,
                regno: res.entity.regno,
                phone: res.entity.phone,
                email: res.entity.email,
                fax: res.entity.fax,
                address1: res.entity.address1,
                address2: res.entity.address2,
                city: res.entity.city,
                state: res.entity.state,
                country: res.entity.country,
                note: res.entity.note,
                zipcode: res.entity.zipcode,
                currency: res.entity.currency,
                ismulticompany: res.entity.ismulticompany,
                subsidiary_level: res.entity.subsidiary_level,
              });
            }
          },
        });
    }
    this.subs.sink = this.generalSettingsService
      .getCountryList()
      .subscribe((response) => {
        this.countryList = response.entity;
      });
  }
  cancelForm() {
    this.router.navigate(['/admin/company/all-general-information']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.generalInformationForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new general information failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const generalInformationModel: GeneralInformationModel = {
        generalInformationId: this.id ? this.id : 0,
        organisationName: this.generalInformationForm.value.organisationName,
        taxid: this.generalInformationForm.value.taxid,
        regno: this.generalInformationForm.value.regno,
        phone: this.generalInformationForm.value.phone,
        email: this.generalInformationForm.value.email,
        fax: this.generalInformationForm.value.fax,
        address1: this.generalInformationForm.value.address1,
        address2: this.generalInformationForm.value.address2,
        city: this.generalInformationForm.value.city,
        state: this.generalInformationForm.value.state,
        country: this.generalInformationForm.value.country,
        note: this.generalInformationForm.value.note,
        zipcode: this.generalInformationForm.value.zipcode,
        currency: this.generalInformationForm.value.currency,
        ismulticompany: this.generalInformationForm.value.ismulticompany,
        subsidiary_level: +this.generalInformationForm.value.subsidiary_level,
      };
      this.subs.sink = this.generalInformationService
        .addGeneralInformation(generalInformationModel)
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
              this.router.navigate(['/admin/company/all-general-information']);
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
