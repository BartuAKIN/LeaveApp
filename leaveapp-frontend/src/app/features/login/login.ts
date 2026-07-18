import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators} from '@angular/forms'
import { Router } from '@angular/router';
import { Auth } from '../../core/services/auth';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(
    private authService: Auth,
    private router: Router) {}

  onSubmit() {
    if (this.loginForm.invalid) {
      console.log('Form geçersiz');
      return;
    }

    const { email, password } = this.loginForm.value;

    this.authService.login(email!,password!).subscribe({
      next: (response) => {
        console.log('Giriş başarılı', response);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.error('Giriş hatası:', err);
      }
    });
  }
}
