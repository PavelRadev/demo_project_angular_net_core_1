import { SafeSubscriberMixin } from '../component-mixins/safe-subscriber.mixin';

export class BaseSafeSubscriber extends SafeSubscriberMixin()
  implements SafeSubscriberMixin {
}

// tslint:disable-next-line:no-empty-interface
export interface BaseSafeSubscriber extends SafeSubscriberMixin {
}
