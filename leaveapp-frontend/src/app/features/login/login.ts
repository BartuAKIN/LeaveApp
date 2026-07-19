import { Component, signal } from '@angular/core';
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

  errorMessage = signal<string | null>(null);
  isLoading = signal(false);

  constructor(
    private authService: Auth,
    private router: Router) {}

  onSubmit() {
    if (this.loginForm.invalid) {
      console.log('Form geçersiz');
      return;
    }

    this.errorMessage.set(null);
    this.isLoading.set(true);

    const { email, password } = this.loginForm.value;

    this.authService.login(email!,password!).subscribe({
      next: (response) => {
        this.isLoading.set(false);
        console.log('Giriş başarılı', response);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.isLoading.set(false);
        console.error('Giriş hatası:', err);

        if (err.status === 401) {
          this.errorMessage.set('E-posta veya şifre hatalı.');
        } else if (err.status === 0) {
          this.errorMessage.set('Sunucuya bağlanılamadı. Lütfen daha sonra tekrar deneyin.');
        } else {
          this.errorMessage.set(err.error?.message ?? 'Giriş sırasında bir hgata oluştu.')
        }
      }
    });
  }
}
