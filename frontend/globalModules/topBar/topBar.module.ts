import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import { TopBarComponent } from './components/top-bar/top-bar.component';
import {RouterModule} from "@angular/router";
import {MdbCollapseModule} from "mdb-angular-ui-kit/collapse";

@NgModule({
    imports: [CommonModule, RouterModule, MdbCollapseModule],
  declarations: [
    TopBarComponent
  ],
  exports: [TopBarComponent]
})
export class TopBarModule{}

