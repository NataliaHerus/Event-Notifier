import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { AuthService } from 'src/app/demo/services/identity services/auth.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
  providers: [MessageService]
})
export class ResetPasswordComponent implements OnInit{
  resetPasswordForm: FormGroup;
  errorMessage: string = '';
  token: string = '';
  id: string = '';

  constructor(
    public layoutService: LayoutService,
    private fb: FormBuilder,
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute
  ) { 
    this.resetPasswordForm = this.fb.group({
      password: ['', [
        (p: AbstractControl) => Validators.required(p),
        Validators.pattern(
          /(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&^_-]).{8,}/
        ),
      ]],
      confirmPassword: ['', [Validators.required, this.confirmPasswordValidator()]],
    });
  }

  ngOnInit(): void {
    this.token = this.route.snapshot.paramMap.get('token')!;
    this.id = this.route.snapshot.paramMap.get('id')!;
  }

  get resetPasswordFormControl() {
    return this.resetPasswordForm.controls;
  }

  resetPassword() {
    let confirmedPassword =  this.resetPasswordForm.value;
    console.log(confirmedPassword)
    this.authService.resetPassword(confirmedPassword, this.id, this.token).subscribe(
      () => {
        this.authService.logout();
        this.router.navigate(['/auth/login'], { replaceUrl: true} )
      },
        (error) => {
          this.errorMessage = error.errorMessage
        }),
      () => this.messageService.add({ severity: 'error', summary: 'Перевірте дані!' });
  }

  confirmPasswordValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (control.value && control.value != this.resetPasswordFormControl['password'].value) {
        return { 'passwords_do_not_match': true };
      }
      return null;
    };
  }
  
}
