import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { AuthService } from 'src/app/core/service/auth.service';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';
@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit
{
  forgetPasswordForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  error = '';
  returnUrl!: string;
  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {
    super();
  }
  ngOnInit() {
    this.forgetPasswordForm = this.formBuilder.group({
      email: [
        '',
        [Validators.required, Validators.email, Validators.minLength(5)],
      ],
    });
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }
  get f() {
    return this.forgetPasswordForm.controls;
  }
  onSubmit() {
    this.submitted = true;
    this.loading = true;
    this.error = '';
    // stop here if form is invalid
    if (this.forgetPasswordForm.invalid) {
      this.error = 'Enter a valid email!';
      return;
    } else {
      this.subs.sink = this.authService
        .forgotPassword(this.f['email'].value)
        .subscribe({
          next: (res) => {
            if (res.status) {
              this.loading = false;
              this.router.navigate(['/account/forgot-password-success']);
            } else {
              this.error = 'Invalid email';
            }
          },
          error: (error) => {
            this.error = error;
            this.submitted = false;
            this.loading = false;
          },
        });
    }
  }
}
