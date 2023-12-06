import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {

  private authService = inject(AuthService);
  private http = inject(HttpClient);

  email: string = '';
  password: string = '';
  errors: string[] = [];

  login(): void {
    const request: any = {
      email: this.email,
      password: this.password,
      storeJwtInCookie: true
    };

    this.http.post('api/accounts/login', request).subscribe({
      next: (res: any) => this.authService.login(res.lifetimeInSeconds),
      error: err => this.errors = [ err.error.errorMessage ]
    });
  }
}
