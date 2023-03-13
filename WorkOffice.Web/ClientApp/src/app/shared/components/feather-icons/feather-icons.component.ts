/* eslint-disable @typescript-eslint/no-empty-function */
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-feather-icons',
  templateUrl: './feather-icons.component.html',
  styleUrls: ['./feather-icons.component.sass'],
})
export class FeatherIconsComponent {
  @Input() public icon?: string;
  @Input() public class?: string;
  constructor() {}
}
