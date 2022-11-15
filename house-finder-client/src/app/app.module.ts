import { SafePipe } from './pipe/safePipe';
import { HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; 

import { AppRoutingModule, routingComponents } from './app-routing.module';
import { AppComponent } from './app.component';
import { HouseDetailComponent } from './Guest/house-detail/house-detail.component';
import { LoginComponent } from './Guest/login/login.component';
// import { SocialLoginModule, SocialAuthServiceConfig } from 'angularx-social-login';
// import {
//   GoogleLoginProvider,
//   FacebookLoginProvider
// } from 'angularx-social-login';
import { DashboardComponent } from './Landlord/dashboard/dashboard.component';
import { ListRoomComponent } from './Landlord/list-room/list-room.component';
import { ListReportComponent } from './Staff/list-report/list-report.component';
import { ListHouseComponent } from './Staff/list-house/list-house.component';
import { DashboardStaffComponent } from './Staff/dashboard/dashboard.component';
import { UpdateRoomComponent } from './Landlord/update-room/update-room.component';
import { ListLandlordComponent } from './Staff/list-landlord/list-landlord.component';
import { ListLandlordSignupRequestComponent } from './Staff/list-landlord-signup-request/list-landlord-signup-request.component';
import { RoomDetailComponent } from './Guest/room-detail/room-detail.component';
import { AddHouseComponent } from './Landlord/add-house/add-house.component';
import { UpdateHouseComponent } from './Landlord/update-house/update-house.component';
import { LandlordHouseDetailComponent } from './Landlord/landlord-house-detail/landlord-house-detail.component';
import { RateHouseComponent } from './Landlord/rate-house/rate-house.component';
import { ListOrderComponent } from './Staff/list-order/list-order.component';
import { AddRoomComponent } from './Landlord/add-room/add-room.component';
import { SingleComponent } from './Landlord/add-room/single/single.component';
import { MultipleComponent } from './Landlord/add-room/multiple/multiple.component';
import { MaterialModule } from './materials/material.module';
import { ListStaffComponent } from './Admin/list-staff/list-staff.component';
import { ChangePasswordComponent } from './Admin/change-password/change-password.component';
import { CreateAccountComponent } from './Admin/create-account/create-account.component';
import { UpdateAccountComponent } from './Admin/update-account/update-account.component';
import { StaffSidebarComponent } from './Common/staff-sidebar/staff-sidebar.component';
import { HeaderComponent } from './Common/header/header.component';
import { FooterComponent } from './Common/footer/footer.component';
import { RoleModalComponent } from './Guest/login/role-modal/role-modal.component';
import { StaffHeaderComponent } from './Common/staff-header/staff-header.component';
import { StaffNavbarComponent } from './Common/staff-navbar/staff-navbar.component';
import { StaffLandlordDetailComponent } from './Staff/staff-landlord-detail/staff-landlord-detail.component';
import { StaffHouseDetailComponent } from './Staff/staff-house-detail/staff-house-detail.component';
import { StaffRoomDetailComponent } from './Staff/staff-room-detail/staff-room-detail.component';
import { DeleteHouseComponent } from './Landlord/delete-house/delete-house.component';
import { RegisterComponent } from './Guest/login/register/register.component';
import { ImportComponent } from './Landlord/import/import.component';
import { LandlordDetailInfoComponent } from './Staff/list-landlord/landlord-detail-info/landlord-detail-info.component';
import { StaffLandlordDetailInfoComponent } from './Staff/staff-landlord-detail/staff-landlord-detail-info/staff-landlord-detail-info.component';
import { DashboardInfoComponent } from './Landlord/dashboard/dashboard-info/dashboard-info.component';

@NgModule({
  declarations: [
    AppComponent,
    routingComponents,
    SafePipe,
    HouseDetailComponent,
    LoginComponent,
    DashboardComponent,
    ListRoomComponent,
    DashboardStaffComponent,
    ListRoomComponent,
    ListReportComponent,
    ListHouseComponent,
    UpdateRoomComponent,
    ListLandlordComponent,
    ListLandlordSignupRequestComponent,
    RoomDetailComponent,
    AddHouseComponent,
    UpdateHouseComponent,
    LandlordHouseDetailComponent,
    RateHouseComponent,
    ListOrderComponent,
    AddRoomComponent,
    SingleComponent,
    MultipleComponent,
    ListStaffComponent,
    ChangePasswordComponent,
    CreateAccountComponent,
    UpdateAccountComponent,
    StaffSidebarComponent,
    HeaderComponent,
    FooterComponent,
    RoleModalComponent,
    StaffHeaderComponent,
    StaffNavbarComponent,
    StaffLandlordDetailComponent,
    StaffHouseDetailComponent,
    StaffRoomDetailComponent,
    DeleteHouseComponent,
    RegisterComponent,
    ImportComponent,
    LandlordDetailInfoComponent,
    StaffLandlordDetailInfoComponent,
    DashboardInfoComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    BrowserAnimationsModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
