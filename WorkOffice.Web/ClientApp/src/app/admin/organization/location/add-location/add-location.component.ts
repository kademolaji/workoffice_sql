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
import { LocationModel } from '../location.model';
import { LocationService } from '../location.service';

@Component({
  selector: 'app-add-location',
  templateUrl: './add-location.component.html',
  styleUrls: ['./add-location.component.css'],
})
export class AddLocationComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  locationForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  isAddMode = true;
  id = 0;
  countryList: GeneralSettingsModel[] = [];

  constructor(
    private fb: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private locationService: LocationService,
    private snackBar: MatSnackBar,
    private generalSettingsService: GeneralSettingsService
  ) {
    super();
  }
  ngOnInit() {
    this.locationForm = this.fb.group({
      name:  ['', [Validators.required]],
      country: ['', [Validators.required]],
      state:  [''],
      city:  ['', [Validators.required]],
      address:  ['', [Validators.required]],
      zipCode:  ['', [Validators.required]],
      phone:  ['', [Validators.required]],
      fax:  [''],
      note:  [''],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if (!this.isAddMode) {
      this.subs.sink = this.locationService.getLocationById(this.id).subscribe({
        next: (res) => {
          if (res.status) {
            this.locationForm.setValue({
              name: res.entity.name,
              country: res.entity.country,
              state: res.entity.state,
              city: res.entity.city,
              address: res.entity.address,
              zipCode: res.entity.zipCode,
              phone: res.entity.phone,
              fax: res.entity.fax,
              note: res.entity.note,
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
    this.router.navigate(['/admin/company/all-location']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.locationForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new location failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const locationModel: LocationModel = {
        locationId: this.id ? this.id : 0,
        name: this.locationForm.value.name,
        country: this.locationForm.value.country,
        state: this.locationForm.value.state,
        city: this.locationForm.value.city,
        address: this.locationForm.value.address,
        zipCode: this.locationForm.value.zipCode,
        phone: this.locationForm.value.phone,
        fax: this.locationForm.value.fax,
        note: this.locationForm.value.note,
      };
      this.subs.sink = this.locationService
        .addLocation(locationModel)
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
              this.router.navigate(['/admin/company/all-location']);
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
