import { OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

export interface SafeSubscriberMixin {
  subscriptions: Subscription;

  registerSubscription(subscription: Subscription): Subscription;

  unsubscribeAll(): void;

  unsubscribe(subscription: Subscription): void;
}

type Constructor<T> = new(...args: any[]) => T;

export function SafeSubscriberMixin<T extends Constructor<{}>>(Base: T = (class {
} as any)): any {
  return class extends Base implements OnDestroy {
    private subscriptions = new Subscription();

    registerSubscription(subscription: Subscription): Subscription {
      this.subscriptions.add(subscription);
      return subscription;
    }

    ngOnDestroy(): void {
      this.unsubscribeAll();
    }

    unsubscribeAll(): void {
      this.subscriptions.unsubscribe();
    }

    unsubscribe(subscription: Subscription): void {
      this.subscriptions.remove(subscription);
      subscription.unsubscribe();
    }
  };
}
