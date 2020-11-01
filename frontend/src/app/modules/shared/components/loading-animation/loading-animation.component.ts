import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading-animation',
  templateUrl: './loading-animation.component.html'
})
export class LoadingAnimationComponent {
  @Input() shadeHeight = 'auto';
  @Input() size = '30px';
  @Input() blobSize = '10px';
  @Input() blobColor = '#64b5f6';

  constructor() {
  }
}
