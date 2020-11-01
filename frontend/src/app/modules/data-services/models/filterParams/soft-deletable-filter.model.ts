import { BaseListFilterModel } from './base-list-filter-model';

export class SoftDeletableFilterModel extends BaseListFilterModel {
  public withTrashed: boolean;
  public deletedBy: string[];
  public deletedFrom: Date;
  public deletedTo: Date;
}
