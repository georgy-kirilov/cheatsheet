import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  errors: string[] = [];

  constructor(private http: HttpClient, private router: Router) { }

  register(): void {
    this.http.post('api/accounts/register', {
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword
    }).subscribe({
      next: _ => this.router.navigateByUrl('/'),
      error: err => this.errors = err.error.map((e: any) => e.errorMessage)
    });
  }
}
