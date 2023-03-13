import { Component } from '@angular/core';
import { InConfiguration } from 'src/app/core/models/config.interface';
import { DirectionService } from 'src/app/core/service/direction.service';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: [],
})
export class MainLayoutComponent {
  direction: string | undefined;
  public config!: InConfiguration;
  constructor(private directoryService: DirectionService) {
    this.directoryService.currentData.subscribe((currentData) => {
      if (currentData) {
        this.direction = currentData;
      } else {
        if (localStorage.getItem('isRtl')) {
          if (localStorage.getItem('isRtl') === 'true') {
            this.direction = 'rtl';
          } else if (localStorage.getItem('isRtl') === 'false') {
            this.direction = 'ltr';
          }
        } else {
          if (this.config) {
            if (this.config.layout.rtl == true) {
              this.direction = 'rtl';
            } else {
              this.direction = 'ltr';
            }
          }
        }
      }
    });
  }
}
