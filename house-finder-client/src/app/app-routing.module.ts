import { HomepageComponent } from './homepage/homepage.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HouseDetailComponent} from './house-detail/house-detail.component';
import { LoginComponent } from './login/login.component';

const routes: Routes = [
  { path: 'home', component: HomepageComponent },
  { path: 'house-detail', component: HouseDetailComponent},
  { path: 'login', component: LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})

export class AppRoutingModule {}
export const routingComponents = [HomepageComponent]
