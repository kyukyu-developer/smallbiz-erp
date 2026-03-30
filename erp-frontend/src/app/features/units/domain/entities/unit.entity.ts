export interface Unit {
  id: string;
  name: string;
  symbol: string;
  active: boolean;
  createdAt?: Date;
  updatedAt?: Date;
  createdBy?: string;
  updatedBy?: string;
  lastAction?: string;
}

export interface CreateUnitDto {
  name: string;
  symbol: string;
  active?: boolean;
}

export interface UpdateUnitDto extends Partial<CreateUnitDto> {
  id: string;
}
