import { BaseFilterModel } from './base-filter.model';

export class BaseListFilterModel extends BaseFilterModel {
  public pageSize = 0;
  public page = 0;
  public orderedBy: string;
  public orderReversed: boolean;
}
