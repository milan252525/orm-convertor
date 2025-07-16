import { Routes } from "@angular/router";
import { MainPageComponent } from "./containers/main-page/main-page.component";
import { DemoPageComponent } from "./containers/demo-page/demo-page.component";

export const routes: Routes = [
  {
    path: "",
    component: MainPageComponent,
  },
  {
    path: "advisor",
    component: DemoPageComponent,
  },
];
