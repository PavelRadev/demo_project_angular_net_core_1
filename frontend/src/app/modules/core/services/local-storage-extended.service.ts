import {Injectable, OnDestroy} from '@angular/core';
import {Subject} from "rxjs";
import {KeyValue} from "@angular/common";

@Injectable({
  providedIn: 'root'
})
export class LocalStorageExtendedService implements OnDestroy {
  private onSubject = new Subject<KeyValue<string, any>>();
  public OnStorageChange$ = this.onSubject.asObservable();

  constructor() {
    this.SubscribeToLocalStorageChanges();
  }

  ngOnDestroy(): void {
    this.CancelLocalStorageSubscription();
  }

  public Get(key: string): any {
    const localStorageData = localStorage.getItem(key);

    return JSON.parse(localStorageData);
  }

  public Set(key: string, data: any, broadcastToCurrentTab: boolean = false): void {
    localStorage.setItem(key, JSON.stringify(data));
    if (broadcastToCurrentTab) {
      this.onSubject.next({ key, value: data});
    }
  }

  public Remove(key, broadcastToCurrentTab: boolean = false): void {
    localStorage.removeItem(key);
    if (broadcastToCurrentTab) {
      this.onSubject.next({ key, value: null });
    }
  }


  private SubscribeToLocalStorageChanges(): void {
    window.addEventListener('storage', this.onLocalStorageChanged.bind(this));
  }

  private onLocalStorageChanged(event: StorageEvent): void {
    if (event.storageArea == localStorage) {
      let v;
      try { v = JSON.parse(event.newValue); }
      catch (e) { v = event.newValue; }
      this.onSubject.next({ key: event.key, value: v });
    }
  }

  private CancelLocalStorageSubscription(): void {
    window.removeEventListener('storage', this.onLocalStorageChanged.bind(this));
    this.onSubject.complete();
  }
}
