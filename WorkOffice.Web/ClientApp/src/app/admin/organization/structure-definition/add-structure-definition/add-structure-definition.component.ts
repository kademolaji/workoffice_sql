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
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
import { StructureDefinitionModel } from '../structure-definition.model';
import { StructureDefinitionService } from '../structure-definition.service';

@Component({
  selector: 'app-add-structure-definition',
  templateUrl: './add-structure-definition.component.html',
  styleUrls: ['./add-structure-definition.component.css'],
})
export class AddStructureDefinitionComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  structureDefinitionForm!: UntypedFormGroup;
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
    private structureDefinitionService: StructureDefinitionService,
    private snackBar: MatSnackBar
  ) {
    super();
  }
  ngOnInit() {
    this.structureDefinitionForm = this.fb.group({
      definition: ['', [Validators.required]],
      description: ['', [Validators.required]],
      level: ['', [Validators.required]],
    });
    this.id = +this.route.snapshot.params['id'];
    this.isAddMode = !this.id;
    if(!this.isAddMode){
      this.subs.sink = this.structureDefinitionService
      .getStructureDefinitionById(this.id)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.structureDefinitionForm.setValue({
              definition: res.entity.definition,
              description: res.entity.description,
              level: res.entity.level,
            });
          }
          }
      });
    }



  }
  cancelForm() {
    this.router.navigate(['/admin/company/all-structure-definition']);
  }

  onSubmit() {
    this.submitted = true;
    this.loading = true;
    // stop here if form is invalid
    if (this.structureDefinitionForm.invalid) {
      this.showNotification(
        'snackbar-danger',
        'Add new company structure definition failed...!!!',
        'top',
        'right'
      );
      return;
    } else {
      const structureDefinition: StructureDefinitionModel = {
        structureDefinitionId: this.id,
        definition: this.structureDefinitionForm.value.definition,
        description: this.structureDefinitionForm.value.description,
        level: this.structureDefinitionForm.value.level,
      };
      this.subs.sink = this.structureDefinitionService
        .addStructureDefinition(structureDefinition)
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
              this.router.navigate(['/admin/company/all-structure-definition']);
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
