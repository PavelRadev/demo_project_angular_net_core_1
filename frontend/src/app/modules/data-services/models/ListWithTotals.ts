export class ListWithTotals<T> {
  public list: T[] = [];
  public totalCount: number;
  public currentPage: number;
  public pageSize: number;
  public skippedItems: number;

  public get isEmpty(): boolean {
    return !this.list.length;
  }
}
