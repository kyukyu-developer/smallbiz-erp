import { Component } from '@angular/core';

@Component({
  selector: 'app-main-layout',
  standalone: false,
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss'
})
export class MainLayoutComponent {
  sidenavOpened = true;
  sidebarOpen = true;

  toggleSidenav() {
    this.sidenavOpened = !this.sidenavOpened;
  }

  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
  }
}
