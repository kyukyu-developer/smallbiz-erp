import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { UnitConversion } from '../../domain/entities/unit-conversion.entity';
import { GetAllUnitConversionsUseCase, DeleteUnitConversionUseCase } from '../../application/usecases/unit-conversion.usecase';

@Component({
  selector: 'app-unit-conversion-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, MatButtonModule, MatIconModule, MatMenuModule, MatProgressSpinnerModule],
  templateUrl: './unit-conversion-list.component.html',
  styleUrl: './unit-conversion-list.component.scss'
})
export class UnitConversionListComponent implements OnInit {
  private getAllUseCase = inject(GetAllUnitConversionsUseCase);
  private deleteUseCase = inject(DeleteUnitConversionUseCase);
  private router = inject(Router);

  conversions: UnitConversion[] = [];
  filteredConversions: UnitConversion[] = [];
  searchTerm = '';
  loading = false;

  ngOnInit() { this.loadConversions(); }

  loadConversions() {
    this.loading = true;
    this.getAllUseCase.execute().subscribe({
      next: (r) => { this.conversions = r.conversions; this.filteredConversions = [...this.conversions]; this.loading = false; },
      error: () => { this.loading = false; }
    });
  }

  applyFilter() {
    if (!this.searchTerm) { this.filteredConversions = [...this.conversions]; return; }
    const t = this.searchTerm.toLowerCase();
    this.filteredConversions = this.conversions.filter(c => c.productName?.toLowerCase().includes(t) || c.fromUnitName?.toLowerCase().includes(t) || c.toUnitName?.toLowerCase().includes(t));
  }

  viewConversion(c: UnitConversion) { this.router.navigate(['/inventory/unit-conversions', c.id]); }
  editConversion(c: UnitConversion) { this.router.navigate(['/inventory/unit-conversions', c.id]); }
  deleteConversion(c: UnitConversion) {
    if (confirm(`Delete conversion: ${c.fromUnitName} to ${c.toUnitName}?`)) {
      this.deleteUseCase.execute(c.id).subscribe({ next: () => this.loadConversions(), error: (e) => console.error(e) });
    }
  }

  addNew() { this.router.navigate(['/inventory/unit-conversions/new']); }
}
