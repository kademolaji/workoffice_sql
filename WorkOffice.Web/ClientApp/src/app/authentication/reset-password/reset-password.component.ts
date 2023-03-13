import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/core/service/auth.service';
import { MustMatch } from 'src/app/core/utilities/must-match.validator';
import { UnsubscribeOnDestroyAdapter } from 'src/app/shared/UnsubscribeOnDestroyAdapter';

enum TokenStatus {
  Validating,
  Valid,
  Invalid
}

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})

export class ResetPasswordComponent  extends UnsubscribeOnDestroyAdapter
implements OnInit
{
resetPasswordForm!: UntypedFormGroup;
submitted = false;
loading = false;
error = '';
returnUrl!: string;
TokenStatus = TokenStatus;
tokenStatus = TokenStatus.Validating;
token = '';
hide = true;
constructor(
  private formBuilder: UntypedFormBuilder,
  private route: ActivatedRoute,
  private router: Router,
  private authService: AuthService
) {
  super();
}
ngOnInit() {
  this.resetPasswordForm = this.formBuilder.group({
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required],
}, {
    validator: MustMatch('password', 'confirmPassword')
});

const token = this.route.snapshot.queryParams['token'];

// remove token from url to prevent http referer leakage
this.router.navigate([], { relativeTo: this.route, replaceUrl: true });
if (token){
this.authService.validateResetToken(token)
    .subscribe((response) => {
            this.token = token;
            if (response.status){
              this.tokenStatus = TokenStatus.Valid;
            } else{
              this.tokenStatus = TokenStatus.Invalid;
            }
        });
} else {
  this.tokenStatus = TokenStatus.Invalid;
}
}
get f() {
  return this.resetPasswordForm.controls;
}
onSubmit() {
  this.submitted = true;
  this.loading = true;
  this.error = '';
  // stop here if form is invalid
  if (this.resetPasswordForm.invalid) {
    this.error = '  Passwords must match!';
    return;
  } else {
    this.subs.sink = this.authService
      .resetPassword(this.token, this.f['password'].value, this.f['confirmPassword'].value)
      .subscribe({
        next: (res) => {
          if (res.status) {
            this.loading = false;
            this.router.navigate(['/account/reset-password-success']);
          } else {
            this.error = 'Reset password failed';
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
