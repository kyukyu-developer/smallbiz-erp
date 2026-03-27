import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MainLayoutComponent } from './shared/layout/main-layout/main-layout.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule, MainLayoutComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'erp-project';
  showLayout = false;

  constructor(private router: Router) {}

  ngOnInit() {
    // Listen to navigation changes
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.updateLayoutVisibility(event.urlAfterRedirects);
      });
  }

  private updateLayoutVisibility(url: string): void {
    // Hide layout on login page, show it everywhere else
    this.showLayout = !url.includes('/login');
  }
}
