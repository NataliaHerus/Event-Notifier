import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { User } from 'src/app/demo/models/user';
import { AuthService } from 'src/app/demo/services/identity services/auth.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  providers: [MessageService]
})
export class RegisterComponent {

  registerForm: FormGroup;
  errorMessage: string = '';

  constructor(
    public layoutService: LayoutService,
    private fb: FormBuilder,
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(20)]],
      lastName: ['', [Validators.required, Validators.maxLength(20)]],
      email: ['', [Validators.required, Validators.minLength(4), Validators.email, Validators.maxLength(20)]],
      password: ['', [
        (p: AbstractControl) => Validators.required(p),
        Validators.pattern(
          /(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&^_-]).{8,}/
        ),
      ]],
      cofirmedPassword: ['', [Validators.required, this.confirmPasswordValidator()]],
    });
  }

  get registerFormControl() {
    return this.registerForm.controls;
  }

  register() {
    let newUser: User = this.registerForm.value;
    this.authService.register(newUser).subscribe(
      () => this.authService.login(newUser).subscribe(data => {
        this.authService.saveToken(data['token']);
        this.authService.saveCurrentUsername(this.registerForm.value['email']);
        this.router.navigate(['/'], { replaceUrl: true });
      },
        (error) => {
          this.errorMessage = error.errorMessage
        }),
      () => this.messageService.add({ severity: 'error', summary: 'Перевірте дані!' }));

  }


  confirmPasswordValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (control.value && control.value != this.registerFormControl['password'].value) {
        return { 'passwords_do_not_match': true };
      }
      return null;
    };
  }

}

