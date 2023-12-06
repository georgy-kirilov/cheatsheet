import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-me',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './me.component.html',
})
export class MeComponent {

  private http = inject(HttpClient);

  username: string = '';
  email: string = '';

  ngOnInit(): void {
    this.http.get<any>('api/accounts/me/info').subscribe({
      next: (res: any) => {
        this.username = res.username;
        this.email = res.email;
      },
      error: err => console.error(err)
    });
  }
}
