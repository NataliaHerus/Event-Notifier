import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { AuthService } from 'src/app/demo/services/identity services/auth.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.scss'],
  providers: [MessageService]
})
export class EmailConfirmationComponent  {
  emailConfirmationForm: FormGroup;
  
  constructor(
    private fb: FormBuilder, 
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router
  ) {
    this.emailConfirmationForm = this.fb.group({
      'email': ['', [Validators.required, Validators.minLength(4), Validators.email, Validators.maxLength(20)]],
    })
   }

   confirmEmail() {
    this.authService.emailConfirmation(this.emailConfirmationForm.value).subscribe( 
      data => {
      this.router.navigate(['auth/email-confirmed'], { replaceUrl: true });
    },
    (error) => {
      let errorMessages = [];
      let errors = (error as HttpErrorResponse).error.errors;
      for (let field in errors) {
        errorMessages.push(errors[field][0]);
      }
      this.messageService.add({ severity: 'error', summary: 'Лист не надіслано!', detail: errorMessages.join(', ') });
    })
  }

  get confirmEmailFormControl() {
    return this.emailConfirmationForm.controls;
  }

}
