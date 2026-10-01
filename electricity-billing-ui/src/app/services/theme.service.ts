import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly storageKey = 'app-theme';

  private darkMode = false;

  constructor() {
    const savedTheme = localStorage.getItem(this.storageKey);

    this.darkMode = savedTheme === 'dark';

    this.applyTheme();
  }

  toggleTheme(): void {
    this.darkMode = !this.darkMode;

    localStorage.setItem(this.storageKey, this.darkMode ? 'dark' : 'light');

    this.applyTheme();
  }

  isDarkMode(): boolean {
    return this.darkMode;
  }

  private applyTheme(): void {
    const body = document.body;

    if (this.darkMode) {
      body.classList.add('dark-theme');
    } else {
      body.classList.remove('dark-theme');
    }
  }
}
