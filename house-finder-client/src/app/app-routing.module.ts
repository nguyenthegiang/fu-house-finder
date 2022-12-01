import { StaffHouseDetailComponent } from './Staff/staff-house-detail/staff-house-detail.component';
import { StaffLandlordDetailComponent } from './Staff/staff-landlord-detail/staff-landlord-detail.component';
import { UpdateAccountComponent } from './Admin/update-account/update-account.component';
import { CreateAccountComponent } from './Admin/create-account/create-account.component';
import { ChangePasswordComponent } from './Admin/change-password/change-password.component';
import { ListStaffComponent } from './Admin/list-staff/list-staff.component';
import { RateHouseComponent } from './Landlord/rate-house/rate-house.component';
import { AddHouseComponent } from './Landlord/add-house/add-house.component';
import { UpdateHouseComponent } from './Landlord/update-house/update-house.component';
import { LandlordHouseDetailComponent } from './Landlord/landlord-house-detail/landlord-house-detail.component';
import { HomepageComponent } from './Guest/homepage/homepage.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HouseDetailComponent } from './Guest/house-detail/house-detail.component';
import { LoginComponent } from './Guest/login/login.component';
import { DashboardComponent } from './Landlord/dashboard/dashboard.component';
import { ListRoomComponent } from './Landlord/list-room/list-room.component';
import { DashboardStaffComponent } from './Staff/dashboard/dashboard.component';
import { ListLandlordComponent } from './Staff/list-landlord/list-landlord.component';
import { ListReportComponent } from './Staff/list-report/list-report.component';
import { ListHouseComponent } from './Staff/list-house/list-house.component';
import { ListLandlordSignupRequestComponent } from './Staff/list-landlord-signup-request/list-landlord-signup-request.component';
import { RoomDetailComponent } from './Guest/room-detail/room-detail.component';
import { ListOrderComponent } from './Staff/list-order/list-order.component';
import { AddRoomComponent } from './Landlord/add-room/add-room.component';
import { UpdateRoomComponent } from './Landlord/update-room/update-room.component';
import { StaffRoomDetailComponent } from './Staff/staff-room-detail/staff-room-detail.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomepageComponent },
  { path: 'login', component: LoginComponent},
  { path: 'house-detail/:id', component: HouseDetailComponent},
  { path: 'room-detail/:id', component: RoomDetailComponent},
  { path: 'Landlord/dashboard', component: DashboardComponent},
  { path: 'Landlord/list-room/:id', component: ListRoomComponent},
  { path: 'Landlord/landlord-house-detail/:id', component: LandlordHouseDetailComponent},
  { path: 'Landlord/add-house', component: AddHouseComponent},
  { path: 'Landlord/update-house/:id', component: UpdateHouseComponent},
  { path: 'Landlord/update-room/:id', component: UpdateRoomComponent},
  { path: 'Landlord/rate-house/:id', component: RateHouseComponent},
  { path: 'Landlord/add-room', component: AddRoomComponent},
  { path: 'Staff/dashboard', component: DashboardStaffComponent},
  { path: 'Staff/list-landlord', component: ListLandlordComponent},
  { path: 'Staff/list-report', component: ListReportComponent},
  { path: 'Staff/list-house', component: ListHouseComponent},
  { path: 'Staff/list-landlord-signup-request', component: ListLandlordSignupRequestComponent},
  { path: 'Staff/list-order', component: ListOrderComponent},
  { path: 'Staff/staff-landlord-detail/:id', component: StaffLandlordDetailComponent},
  { path: 'Staff/staff-house-detail/:id', component: StaffHouseDetailComponent},
  { path: 'Staff/staff-room-detail/:id', component: StaffRoomDetailComponent},
  { path: 'Admin/list-staff', component: ListStaffComponent},
  { path: 'Admin/change-password', component: ChangePasswordComponent},
  { path: 'Admin/create-account', component: CreateAccountComponent},
  { path: 'Admin/update-account', component: UpdateAccountComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})

export class AppRoutingModule { }
export const routingComponents = [HomepageComponent]
