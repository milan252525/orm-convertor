import { Routes } from "@angular/router";
import { MainPageComponent } from "./containers/main-page/main-page.component";
import { DemoPageComponent } from "./containers/demo-page/demo-page.component";
import { LandingPageComponent } from "./containers/landing-page/landing-page.component";

export const routes: Routes = [
  {
    path: "",
    component: MainPageComponent,
  },
  {
    path: "advisor",
    component: DemoPageComponent,
  },
  {
    path: "home",
    component: LandingPageComponent,
  },
];
