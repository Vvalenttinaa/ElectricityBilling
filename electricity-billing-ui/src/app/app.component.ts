import { Component } from '@angular/core';

import {
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet,
} from '@angular/router';

import { NgIf } from '@angular/common';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from './services/auth.service';
import { ThemeService } from './services/theme.service';
import { MatIconModule } from '@angular/material/icon';

import { TranslatePipe, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    NgIf,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    TranslatePipe
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  constructor(
    public authService: AuthService,
    private router: Router,
    public themeService: ThemeService,
    private translate: TranslateService
  ) {
    this.translate.addLangs(['sr', 'en']);
    this.translate.use('en');
  }

  toggleTheme(): void {
    this.themeService.toggleTheme();
  }

  changeLanguage(language: string): void {
  this.translate.use(language);
}

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/recommendation']);
  }
}
