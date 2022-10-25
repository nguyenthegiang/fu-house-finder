import { ListLandlordRequestComponent } from './Staff/list-landlord-request/list-landlord-request.component';
import { HomepageComponent } from './homepage/homepage.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HouseDetailComponent } from './house-detail/house-detail.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './Landlord/dashboard/dashboard.component';
import { ListRoomComponent } from './Landlord/list-room/list-room.component';
import { DashboardStaffComponent } from './Staff/dashboard/dashboard.component';
import { ListLandlordComponent } from './Staff/list-landlord/list-landlord.component';
import { ListReportComponent } from './Staff/list-report/list-report.component';
import { ListHouseComponent } from './Staff/list-house/list-house.component';
import { ListLandlordSignupRequestComponent } from './Staff/list-landlord-signup-request/list-landlord-signup-request.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomepageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'house-detail/:id', component: HouseDetailComponent },
  { path: 'house-detail', component: HouseDetailComponent },
  { path: 'Landlord/dashboard', component: DashboardComponent },
  { path: 'Landlord/list-room/:id', component: ListRoomComponent },
  { path: 'Staff/dashboard', component: DashboardStaffComponent },
  { path: 'Staff/list-landlord', component: ListLandlordComponent },
  { path: 'Staff/list-report', component: ListReportComponent },
  { path: 'Staff/list-house', component: ListHouseComponent },
  { path: 'Staff/list-landlord-request', component: ListLandlordRequestComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})

export class AppRoutingModule { }
export const routingComponents = [HomepageComponent]
