import { HomepageComponent } from './homepage/homepage.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HouseDetailComponent} from './house-detail/house-detail.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './Landlord/dashboard/dashboard.component';
import { ListRoomComponent } from './Landlord/list-room/list-room.component';
import { DashboardStaffComponent } from './Staff/dashboard/dashboard.component';
import { ListUserComponent } from './Staff/list-user/list-user.component';
import { ListReportComponent } from './Staff/list-report/list-report.component';
import { ListHouseComponent } from './Staff/list-house/list-house.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomepageComponent },
  { path: 'login', component: LoginComponent},
  { path: 'house-detail/:id', component: HouseDetailComponent},
  { path: 'house-detail', component: HouseDetailComponent},
  { path: 'Landlord/dashboard', component: DashboardComponent},
  { path: 'Landlord/list-room/:id', component: ListRoomComponent},
  { path: 'Staff/dashboard', component: DashboardStaffComponent},
  { path: 'Staff/list-user', component: ListUserComponent},
  { path: 'Staff/list-report', component: ListReportComponent},
  { path: 'Staff/list-house', component: ListHouseComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})

export class AppRoutingModule {}
export const routingComponents = [HomepageComponent]
