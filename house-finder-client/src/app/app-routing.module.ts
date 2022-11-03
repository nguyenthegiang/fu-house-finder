import { UpdateHouseComponent } from './Landlord/update-house/update-house.component';
import { LandlordHouseDetailComponent } from './Landlord/landlord-house-detail/landlord-house-detail.component';
import { HomepageComponent } from './Guest/homepage/homepage.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HouseDetailComponent } from './Guest/house-detail/house-detail.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './Landlord/dashboard/dashboard.component';
import { ListRoomComponent } from './Landlord/list-room/list-room.component';
import { UploadHouseInfoSingleComponent } from './upload-house-info-single/upload-house-info-single.component';
import { DashboardStaffComponent } from './Staff/dashboard/dashboard.component';
import { ListLandlordComponent } from './Staff/list-landlord/list-landlord.component';
import { ListReportComponent } from './Staff/list-report/list-report.component';
import { ListHouseComponent } from './Staff/list-house/list-house.component';
import { ListLandlordSignupRequestComponent } from './Staff/list-landlord-signup-request/list-landlord-signup-request.component';
import { RoomDetailComponent } from './Guest/room-detail/room-detail.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomepageComponent },
  { path: 'login', component: LoginComponent},
  { path: 'house-detail/:id', component: HouseDetailComponent},
  { path: 'room-detail/:id', component: RoomDetailComponent},
  { path: 'Landlord/dashboard', component: DashboardComponent},
  { path: 'Landlord/upload/single', component: UploadHouseInfoSingleComponent},
  { path: 'Landlord/list-room/:id', component: ListRoomComponent},
  { path: 'Landlord/landlord-house-detail/:id', component: LandlordHouseDetailComponent},
  { path: 'Landlord/update-house/:id', component: UpdateHouseComponent},
  { path: 'Staff/dashboard', component: DashboardStaffComponent},
  { path: 'Staff/list-landlord', component: ListLandlordComponent},
  { path: 'Staff/list-report', component: ListReportComponent},
  { path: 'Staff/list-house', component: ListHouseComponent},
  { path: 'Staff/list-landlord-signup-request', component: ListLandlordSignupRequestComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})

export class AppRoutingModule { }
export const routingComponents = [HomepageComponent]
