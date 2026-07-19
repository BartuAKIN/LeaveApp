import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { Auth } from '../../core/services/auth';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  constructor(
    private auth: Auth,
    private router: Router
  ) {}

  isLoggedIn(): boolean {
    return this.auth.isLoggedIn();
  }

  isManager(): boolean {
    return this.auth.isManager();
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
