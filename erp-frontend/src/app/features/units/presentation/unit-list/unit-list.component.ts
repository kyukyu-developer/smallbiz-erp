import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Unit } from '../../domain/entities/unit.entity';
import { GetAllUnitsUseCase, DeleteUnitUseCase, GetAllUnitsResult } from '../../application/usecases/unit.usecase';

@Component({
  selector: 'app-unit-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, MatButtonModule, MatIconModule, MatMenuModule, MatProgressSpinnerModule],
  templateUrl: './unit-list.component.html',
  styleUrl: './unit-list.component.scss'
})
export class UnitListComponent implements OnInit {
  private getAllUnitsUseCase = inject(GetAllUnitsUseCase);
  private deleteUnitUseCase = inject(DeleteUnitUseCase);
  private router = inject(Router);

  units: Unit[] = [];
  filteredUnits: Unit[] = [];
  searchTerm: string = '';
  loading: boolean = false;
  totalUnits: number = 0;
  activeUnits: number = 0;
  inactiveUnits: number = 0;

  ngOnInit() {
    this.loadUnits();
  }

  loadUnits() {
    this.loading = true;
    this.getAllUnitsUseCase.execute().subscribe({
      next: (result: GetAllUnitsResult) => {
        this.units = result.units;
        this.totalUnits = result.total;
        this.activeUnits = result.units.filter((u: Unit) => u.active).length;
        this.inactiveUnits = this.totalUnits - this.activeUnits;
        this.filteredUnits = [...this.units];
        this.loading = false;
      },
      error: (error: unknown) => {
        console.error('Error loading units:', error);
        this.loading = false;
      }
    });
  }

  applyFilter() {
    if (!this.searchTerm) {
      this.filteredUnits = [...this.units];
      return;
    }
    const term = this.searchTerm.toLowerCase();
    this.filteredUnits = this.units.filter(u =>
      u.name?.toLowerCase().includes(term) ||
      u.symbol?.toLowerCase().includes(term)
    );
  }

  viewUnit(unit: Unit) {
    this.router.navigate(['/inventory/units', unit.id]);
  }

  editUnit(unit: Unit) {
    this.router.navigate(['/inventory/units', unit.id]);
  }

  deleteUnit(unit: Unit) {
    if (confirm(`Are you sure you want to delete unit: ${unit.name}?`)) {
      this.deleteUnitUseCase.execute(unit.id).subscribe({
        next: () => this.loadUnits(),
        error: (error: unknown) => console.error('Error deleting unit:', error)
      });
    }
  }

  addNewUnit() {
    this.router.navigate(['/inventory/units/new']);
  }
}
