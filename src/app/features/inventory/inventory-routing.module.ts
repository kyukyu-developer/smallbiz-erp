import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InventoryComponent } from './inventory.component';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { WarehouseListComponent } from './warehouse-list/warehouse-list.component';
import { WarehouseDetailComponent } from './warehouse-detail/warehouse-detail.component';
import { WarehouseComponent } from './warehouse/warehouse.component';

const routes: Routes = [
  { path: '', component: InventoryComponent },
  { path: 'products', component: ProductListComponent },
  { path: 'products/new', component: ProductDetailComponent },
  { path: 'products/:id', component: ProductDetailComponent },
  { path: 'products/:id/edit', component: ProductDetailComponent },
  { path: 'warehouse', component: WarehouseComponent },
  { path: 'warehouses', component: WarehouseListComponent },
  { path: 'warehouses/new', component: WarehouseDetailComponent },
  { path: 'warehouses/:id', component: WarehouseDetailComponent },
  { path: 'warehouses/:id/edit', component: WarehouseDetailComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventoryRoutingModule { }
