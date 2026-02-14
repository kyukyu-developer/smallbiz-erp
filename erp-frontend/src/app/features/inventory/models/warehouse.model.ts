/**
 * Warehouse Model
 * Based on INVENTORY_DATABASE_NOTES.txt
 *
 * Database Schema:
 * CREATE TABLE warehouse (
 *     id VARCHAR(50) PRIMARY KEY,
 *     name VARCHAR(50),
 *     city VARCHAR(50),
 *     branch_type VARCHAR(20),         -- 'Main', 'Branch', 'Sub'
 *     is_main_warehouse BOOLEAN,
 *     parent_warehouse_id INT NULL,
 *     is_used_warehouse BOOLEAN DEFAULT TRUE,
 *     active BOOLEAN DEFAULT TRUE,
 *     created_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
 *     modified_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 *     created_by VARCHAR(50),
 *     modified_by VARCHAR(50),
 *     last_action VARCHAR(50),
 *     FOREIGN KEY (parent_warehouse_id) REFERENCES warehouse(id)
 * );
 *
 * Business Rules:
 * 1. ⚠️ Only Main Warehouses (is_main_warehouse = TRUE) can receive stock
 * 2. Hierarchy: Main → Branch → Sub
 * 3. UNIQUE(name, city) to prevent duplicates
 * 4. Branch/Sub warehouses MUST have parent_warehouse_id
 * 5. Main warehouse: parent_warehouse_id = NULL
 */
export interface Warehouse {
  // Core fields (from database schema)
  id?: string;                          // VARCHAR(50) PRIMARY KEY
  name: string;                         // VARCHAR(50) - Warehouse Name
  city: string;                         // VARCHAR(50) - City of Warehouse
  branch_type: 'Main' | 'Branch' | 'Sub';  // VARCHAR(20) - 'Main', 'Branch', 'Sub'
  is_main_warehouse: boolean;           // BOOLEAN - TRUE if main warehouse
  parent_warehouse_id?: string | null;  // VARCHAR(50) NULL - Parent warehouse FK
  is_used_warehouse: boolean;           // BOOLEAN DEFAULT TRUE - Active usage flag
  active: boolean;                      // BOOLEAN DEFAULT TRUE - Active flag

  // Audit fields (from database schema)
  created_on?: Date;                    // TIMESTAMP DEFAULT CURRENT_TIMESTAMP
  modified_on?: Date;                   // TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE
  created_by?: string;                  // VARCHAR(50)
  modified_by?: string;                 // VARCHAR(50)
  last_action?: string;                 // VARCHAR(50) - 'CREATE', 'UPDATE', 'DELETE'

  // UI/Display helper fields (populated via joins, not in DB)
  parent_warehouse_name?: string;       // For display - populated from join

  // Optional contact fields (stored in address/description or separate contact table)
  phone?: string;                       // Contact phone number
  email?: string;                       // Contact email
  address?: string;                     // Full address details
  description?: string;                 // Additional notes/description
}

export interface WarehouseStock {
  warehouse_stock_id: number;
  warehouse_id: number;
  quantity: number;
}

/**
 * DTO for creating new warehouse
 */
export interface CreateWarehouseDto {
  name: string;
  city: string;
  branch_type: 'Main' | 'Branch' | 'Sub';
  is_main_warehouse: boolean;
  parent_warehouse_id?: string | null;
  is_used_warehouse?: boolean;
  active?: boolean;
  description?: string;
  address?: string;
  phone?: string;
  email?: string;
}

/**
 * DTO for updating warehouse
 */
export interface UpdateWarehouseDto extends Partial<CreateWarehouseDto> {
  id: string;
}
