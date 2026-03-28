import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { HeaderComponent } from '../header/header.component';
import { SidebarComponent } from '../sidebar/sidebar.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatSidenavModule,
    HeaderComponent,
    SidebarComponent,
  ],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.scss',
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
