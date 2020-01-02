import { Component } from '@angular/core';

@Component({
  selector: 'app-hot-menu',
  templateUrl: './hot-menu.component.html',
  styleUrls: ['./hot-menu.component.scss']
})
export class HotMenuComponent {
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
