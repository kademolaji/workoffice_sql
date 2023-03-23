import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatSnackBar, MatSnackBarVerticalPosition, MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { GeneralSettingsModel } from 'src/app/core/models/general-settings.model';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { CustomIdentityFormatModel } from '../custom-identity-format.model';
import { CustomIdentityFormatService } from '../custom-identity-format.service';

@Component({
  selector: 'app-add-custom-identity-settings',
  templateUrl: './add-custom-identity-settings.component.html',
  styleUrls: ['./add-custom-identity-settings.component.css']
})
export class AddCustomIdentitySettingsComponent
extends UnsubscribeOnDestroyAdapter
implements OnInit
{
customIdentitySettingsForm!: UntypedFormGroup;
hide3 = true;
agree3 = false;
submitted = false;
loading = false;
isAddMode = true;
id = 0;
companyList: GeneralSettingsModel[] = [];
constructor(
  private fb: UntypedFormBuilder,
  private route: ActivatedRoute,
  private router: Router,
  private customIdentityFormatService: CustomIdentityFormatService,
  private snackBar: MatSnackBar
) {
  super();
}
ngOnInit() {
  this.customIdentitySettingsForm = this.fb.group({
    suffix:  ['', [Validators.required]],
    prefix: ['', [Validators.required]],
    digits: [0, [Validators.required]],
    company: ['', [Validators.required]],

  });
  this.id = +this.route.snapshot.params['id'];
  this.isAddMode = !this.id;
  if(!this.isAddMode){
    this.subs.sink = this.customIdentityFormatService
    .getCustomIdentityFormatById(this.id)
    .subscribe({
      next: (res) => {
        if (res.status) {
          this.customIdentitySettingsForm.setValue({
            suffix:  res.entity.suffix,
            prefix: res.entity.prefix,
            digits: res.entity.digits,
            company: res.entity.company
          });
        }
        }
    });
  }



}
cancelForm() {
  this.router.navigate(['/admin/company/all-custom-identity-settings']);
}

onSubmit() {
  this.submitted = true;
  this.loading = true;
  // stop here if form is invalid
  if (this.customIdentitySettingsForm.invalid) {
    this.showNotification(
      'snackbar-danger',
      'Add new custom identity settings failed...!!!',
      'top',
      'right'
    );
    return;
  } else {
    const structureDefinition: CustomIdentityFormatModel = {
      customIdentityFormatSettingId: this.id,
      suffix: this.customIdentitySettingsForm.value.suffix,
      prefix: this.customIdentitySettingsForm.value.prefix,
      digits: this.customIdentitySettingsForm.value.digits,
      company: this.customIdentitySettingsForm.value.company,
    };
    this.subs.sink = this.customIdentityFormatService
      .addCustomIdentityFormat(structureDefinition)
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
            this.router.navigate(['/admin/company/all-custom-identity-settings']);
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

